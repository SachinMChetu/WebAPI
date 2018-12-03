using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;

namespace DAL.Code
{
    public class ExportCodeHelper
    {
        public static dynamic GetCSVFromList<T>(List<T> item)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var a in item)
            {
                sb.Append(a.ToInvariantString() + " ");
            }
            return sb.ToString().TrimEnd(' ');

        }
        public static dynamic ConvertToGlobalizationUS(int number)
        {
            return number.ToString("N1", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}