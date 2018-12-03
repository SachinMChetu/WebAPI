using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Code
{
    public static class Extentions
    {
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            List<PropertyInfo> differences = new List<PropertyInfo>();
            FieldInfo[] fi = val1.GetType().GetFields();
            foreach (PropertyInfo property in val1.GetType().GetProperties())
            {
                if ((property.GetValue(val1, null) is null) || (property.GetValue(val2, null) is null))
                {                    
                    continue;
                }
                else
                {
                    object value1 = property.GetValue(val1, null);
                    object value2 = property.GetValue(val2, null);
                    if (!value1.Equals(value2))
                    {
                        differences.Add(property);
                        variances.Add(new Variance
                        {
                            Prop = property.Name,
                            oldValue = value2,
                            newValue = value1
                        });
                    }

                }
                
            }
            return variances;
        }
    }
    public class Variance
    {
        public string Prop { get; set; }
        public object oldValue { get; set; }
        public object newValue { get; set; }
    }
}
