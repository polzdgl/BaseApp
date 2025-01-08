using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseApp.Server.Settings
{
    public class SwaggerSetting : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Add custom filter
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "X-Filter",
                In = ParameterLocation.Header,
                Required = false,
            });
        }
    }
}
