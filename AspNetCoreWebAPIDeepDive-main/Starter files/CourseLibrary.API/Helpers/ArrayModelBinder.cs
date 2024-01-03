using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace CourseLibrary.API.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //checks if our binder works only on enumerble types
            if (bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //Get inputed value through the value provider
            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).ToString();
           
            //If the value is null or white space set it to null
            if (string .IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;

            }
            //the value is not null or white space
            //and the type of the model is enumrable
            //get the enumerables type and a converter
            var elementType = bindingContext.ModelType.GetTypeInfo()
                .GetGenericArguments()[0];
            var converter = TypeDescriptor.GetConverter(elementType);   
            //convert each item in the value list to enumrable type
            var values = value.Split(new[] {
        }
        
    }
}
