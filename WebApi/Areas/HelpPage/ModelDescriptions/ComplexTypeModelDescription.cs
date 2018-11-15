using System.Collections.ObjectModel;

namespace WebApi.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// ComplexTypeModelDescription
    /// </summary>
    public class ComplexTypeModelDescription : ModelDescription
    {
        /// <summary>
        /// ComplexTypeModelDescription
        /// </summary>
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }
        /// <summary>
        /// Properties
        /// </summary>
        public Collection<ParameterDescription> Properties { get; private set; }
    }
}