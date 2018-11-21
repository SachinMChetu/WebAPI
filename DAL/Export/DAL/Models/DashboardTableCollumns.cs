using System.Runtime.Serialization;

namespace DAL.Models
{
    [DataContract]
    public class DashboardTableCollumns
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public bool @checked { get; set; }
        [DataMember]
        public string isRequired { get; set; }
        [DataMember]
        public string sortableValue { get; set; }
        [DataMember]
        public string sortable { get; set; }
    }
}