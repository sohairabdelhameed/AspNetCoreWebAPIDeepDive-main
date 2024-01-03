using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace CourseLibrary.API.Helpers
{
    //This custom binder is particularly useful wh en
    //dealing with arrays or collections within incoming HTTP requests
    //and allows for custom logic to bind these data structures
    //to the respective models in an ASP.NET Core MVC application.
    public class ArrayModelBinder : IModelBinder
    {
        // Method implementing the IModelBinder interface.
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Checks if the type is an enumerable type. If not, binding fails.
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            // Gets the value from the value provider for the specified model name.
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            // If the value is null or white space, set the model to null.
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            // Gets the enumerable's element type and converter.
            var elementType = bindingContext.ModelType.GetTypeInfo().GetGenericArguments()[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            // Converts each item in the value list to the enumerable type.
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            // Creates an array of that type and sets it as the Model value.
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            // Returns a successful result (assigning the model) to complete the binding process.
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }

}
