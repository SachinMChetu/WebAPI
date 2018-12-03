using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.SettingsModels
{
    public class LinkedItemModel
    {
        public int id { get; set; }
        public int? linkedParentQuestion { get; set; }
        public int? linkedToQuestion { get; set; }
        public string linkedType { get; set; }
        public int? linkedItemId { get; set; }
        public bool? initialyVisible { get; set; }
    }
}
