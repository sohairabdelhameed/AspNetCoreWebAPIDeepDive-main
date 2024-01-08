using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace CourseLibrary.API;

internal static class StartupHelperExtensions
{ 
    // Add services to the container
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
     //add framework services when building APIs to the container so they can be used by DI

        builder.Services.AddControllers( configure =>
        {
            //returning 406 status: unsupported media-type
            configure.ReturnHttpNotAcceptable = true;

            configure.CacheProfiles.Add("240SecondsCacheProfile",
                new() { Duration = 240 });
           
            
        })
        .AddNewtonsoftJson(setupAction =>
         {
            setupAction.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver();
        })
        .AddXmlDataContractSerializerFormatters()
        .ConfigureApiBehaviorOptions(setupAction =>
        {
            setupAction.InvalidModelStateResponseFactory = context =>
            {
                // Create a validation problem details
                var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                    context.HttpContext,
                    context.ModelState);

                // Add additional info not added by default
                validationProblemDetails.Detail = "see the errors field for details";
                validationProblemDetails.Instance = context.HttpContext.Request.Path;

                //report invalid model state response as validation issues
                validationProblemDetails.Type =
                 "https://courseLibrary.com/modelvalidationproblem";
                validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                validationProblemDetails.Title ="One or more validation errors occured.";

                return new UnprocessableEntityObjectResult(validationProblemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });
        builder.Services.Configure<MvcOptions>(config =>
        {
            var newtonsoftJsonOutputFormatter = config.OutputFormatters
                  .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

            if (newtonsoftJsonOutputFormatter != null)
            {
                newtonsoftJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.marvin.hateoas+json");
            }
        });
        //registring the propertymappingservice
        builder.Services.AddTransient<IPropertyMappingService,
            PropertyMappingService>();
        //registring the property service
        builder.Services.AddTransient<IPropertyCheckerService,
           PropertyCheckerService>();

        builder.Services.AddScoped<ICourseLibraryRepository, 
            CourseLibraryRepository>(); 

        builder.Services.AddDbContext<CourseLibraryContext>(options =>
        {
            options.UseSqlite(@"Data Source=library.db");
        });
        //Mapper
        builder.Services.AddAutoMapper(
            AppDomain.CurrentDomain.GetAssemblies());
        //cachings
        builder.Services.AddResponseCaching();
        //etags
        builder.Services.AddHttpCacheHeaders(
            (expirationModelOptions) =>
            {
                expirationModelOptions.MaxAge = 60;
                expirationModelOptions.CacheLocation =
                    Marvin.Cache.Headers.CacheLocation.Private;
            },
            (validationModelOptions) =>
            {
                validationModelOptions.MustRevalidate = true;
            });

        return builder.Build();
    }
    
   
    // Configure the request/response pipelien
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(
                        " An Unexpected fault happened.Try again later."
                        );
                });
            });
        }

        app.UseResponseCaching();
       
        app.UseHttpCacheHeaders();
 
        app.UseAuthorization();

        app.MapControllers(); 
         
        return app; 
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetService<CourseLibraryContext>();
                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        } 
    }
}