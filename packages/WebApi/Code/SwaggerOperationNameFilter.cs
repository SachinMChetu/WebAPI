using Swashbuckle.Swagger;
using System;
using System.Web.Http.Description;

namespace WebApi.Code
{
    public class SwaggerOperationNameFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.operationId = "???";
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerOperationAttribute : Attribute
    {
        public SwaggerOperationAttribute(string operationId)
        {
            this.OperationId = operationId;
        }

        public string OperationId { get; private set; }
    }

}