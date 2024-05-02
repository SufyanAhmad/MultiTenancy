using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiTenancy.Helper
{
    public class MyHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "tenant",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "String", },
                Required = false // set to false if this is optional
            });
        }
    }
}
