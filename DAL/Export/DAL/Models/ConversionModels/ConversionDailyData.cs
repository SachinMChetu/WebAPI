using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ConversionModels
{
    public class ConversionRangeDataResponse
    {
        public List<ConversionDailyData> scores { get; set; }
        public ConversionChartRange chartType { get; set; } = ConversionChartRange.Daily;		
		public ConversionRangeDataResponse()
        {
            scores = new List<ConversionDailyData>();
        }
        public static ConversionRangeDataResponse Create(IDataReader reader)
        {
            var crd = new ConversionRangeDataResponse();
			try
			{
				if (reader.Read())
				{
					crd.chartType = (ConversionChartRange)reader.Get<int>("ChartType");
				}

				if (reader.NextResult())
				{
					while (reader.Read())
					{
						crd.scores.Add(ConversionDailyData.Create(reader));
					}
				}
			} catch (Exception ex)
			{
				// exception handling coming, we are stopping ignoring exceptions, at the very least we will log them except in very limited instances.
				throw ex;
			}
            return crd;
        }
    }
    [Flags]
    public enum ConversionChartRange { Daily=0 , Weekly=1, Monthly=2}
    public class ConversionDailyData
    {
        public decimal conversionRate { get; set; }
        public int qualifiedLeads { get; set; }
        public DateTime? date { get; set; }
        public static ConversionDailyData Create(IDataRecord record)
        {
            var bl = record.Get<decimal>("bookedLeads");  
            var ql = record.Get<int>("qualifiedLeads");
            return new ConversionDailyData
            {
                conversionRate = (ql == 0) ? 0 : Math.Truncate(100 * Decimal.Divide(bl, ql)) / 100,
                qualifiedLeads = ql,
                date = record.Get<DateTime>("DateAndTime")
            };
        }
    }
}
