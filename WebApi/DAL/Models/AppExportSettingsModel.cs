using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AppExportSettingsModel
    {
        public List<ExportSettings> exportSettings { get; set; }
        public List<ExportCustomClumns> customClumns { get; set; }
    }
    public class AppExportSetting
    {
        public ExportSettings exportSettings { get; set; }
        public ExportCustomClumns customClumns { get; set; }
    }
    public class ExportCustomClumns
    {
        public int columnId { get; set; }
        public string columnName { get; set; }
        public bool sortable { get; set; }
    }
    public class ExportSettings
    {
        public int id { get; set; }
        public string appname { get; set; }
        public string field { get; set; }
        public string sp { get; set; }
    }
}
