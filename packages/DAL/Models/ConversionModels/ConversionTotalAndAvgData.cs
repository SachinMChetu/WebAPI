using System;
using System.Collections.Generic;
using System.Data;
using DAL.Extensions;

namespace DAL.Models.ConversionModels
{
    /// <summary>
    ///   Returned by GetConversionStatsData  
    /// </summary>
    public class ConversionTotalAndAvgData
    {
        public int totalCalls { get; set; }
        public int nonLead { get; set; } 
        public decimal connectivity { get; set; } 
        public int notConnected { get; set; } 
        public int qualifiedLeads { get; set; } 
        public decimal qualifiedLeadPercent { get; set; } 
        public decimal conversionPercent { get; set; } 
        public int bookedLeads { get; set; } 
        public int missedOpps { get; set; }
        public static ConversionTotalAndAvgData Create(IDataRecord record)
        {
            return new ConversionTotalAndAvgData
            {
                bookedLeads = record.Get<int>("bookedLeads"),
                connectivity = record.Get<decimal>("connectivity"),
                conversionPercent = record.Get<decimal>("conversionPercent"),                
                missedOpps = record.Get<int>("missedOpps"), 
                nonLead = record.Get<int>("nonLead"),
                notConnected = record.Get<int>("notConnected"),
                qualifiedLeadPercent = record.Get<decimal>("qualifiedLeadPercent"),
                qualifiedLeads = record.Get<int>("qualifiedLeads"), 
                totalCalls = record.Get<int>("totalCalls")                
            };
        }
    }

    public class ConversionChartDataItem
    {
		public ConversionUserInfo userInfo;
		public ConversionTotalAndAvgData data;		
		public ConversionChartDataItem()
		{
			data = new ConversionTotalAndAvgData();
			userInfo = new ConversionUserInfo();
		}
        public static ConversionChartDataItem Create(IDataRecord record)
        {
            return new ConversionChartDataItem
            {
                userInfo = ConversionUserInfo.Create(record),
                data = ConversionTotalAndAvgData.Create(record)
            };
        }
    }

	public class ConversionUserInfo
	{
		public string id;
		public string name;
        public static ConversionUserInfo Create(IDataRecord record)
        {
            return new ConversionUserInfo
            {
                id = record.GetValueOrDefault<string>("id",""),
                name = record.GetValueOrDefault<string>("id","")
            };
        }
    }

    public class ConversionChartData
    {
        public List<ConversionChartDataItem> items;
		public int itemsTotal { get; set; } = 0;
		public ConversionChartData()
		{
			items = new List<ConversionChartDataItem>();
		}
		public static ConversionChartData Create(IDataReader reader)
		{
			var crdr = new ConversionChartData();
			try
			{
				var gotTotalRows = false;
				while (reader.Read())
				{
					if (!gotTotalRows)
					{
						crdr.itemsTotal = reader.Get<int>("TotalRows");
						gotTotalRows = true;
					}
					var item = ConversionChartDataItem.Create(reader);				
					crdr.items.Add(item);
				}

			} catch (Exception ex)
			{
				throw ex;
			}
			return crdr;
		}

	}
}
