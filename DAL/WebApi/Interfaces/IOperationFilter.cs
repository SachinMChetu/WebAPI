using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace WebApi.Interfaces
{
    public interface IOperationFilter
    {
        void Apply(Operation operation,
          SchemaRegistry schemaRegistry,
          ApiDescription apiDescription);
    }
}
