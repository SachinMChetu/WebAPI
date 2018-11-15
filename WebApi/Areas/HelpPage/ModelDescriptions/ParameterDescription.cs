using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebApi.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// ParameterDescription
    /// </summary>
    public class ParameterDescription
    {
        /// <summary>
        /// ParameterDescription
        /// </summary>
        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }
        /// <summary>
        /// Annotations
        /// </summary>
        public Collection<ParameterAnnotation> Annotations { get; private set; }
        /// <summary>
        /// Documentation
        /// </summary>
        public string Documentation { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// TypeDescription
        /// </summary>
        public ModelDescription TypeDescription { get; set; }
    }
}