using DAL.Code;
using DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.Models
{
    public class CallDetails
    {
        public List<CallDetail> calls;
        public long itemsTotal;

        public static CallDetails Create(IDataReader reader)
        {
            List<CallDetail> calldetail = new List<CallDetail>();
            while (reader.Read())
            {

                var metaDataItem = CallMetaData.Create(reader);
                var systemDataItem = CallSystemData.Create(reader);

                calldetail.Add(new CallDetail
                {
                    metaData = metaDataItem,
                    systemData = systemDataItem
                });
            }

            long count = 0;
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    count = reader.Get<long>("callsCount");//long.Parse(reader.GetValue(reader.GetOrdinal("callsCount")).ToString());
                }
            }
            var questionDetails = new List<QuestionDetails_v2>();
            if (reader.NextResult())
            {
                try
                {
                    //Mapping for Question Details using factory method
                    questionDetails = QuestionDetails_v2.Create(reader);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (reader.NextResult())
            {
                DataTable table = new DataTable();
                table.Load(reader);

                var dynamicTable = table.ToDynamic();
                foreach (var call in calldetail)
                {
                    call.customData = dynamicTable.Where(m => m.callId == call.systemData.callId).First();
                }
            }

            foreach (var call in calldetail)
            {
                call.callMissedItems = (from a in questionDetails where a.callId == call.systemData.callId select a).ToList();
                //call.missedItemsText = (from a in questionDetails where a.callId == call.systemData.callId select a.questionShortName).ToList();
            }
            var CallDetailsLst = new CallDetails
            {
                itemsTotal = count,
                calls = calldetail
            };
            return CallDetailsLst;
        }
    }
    public class CallDetail
    {
        public CallSystemData systemData { get; set; }
        public CallMetaData metaData { get; set; }
        public dynamic customData { get; set; }
        public List<QuestionDetails_v2> callMissedItems { get; set; }
        //public List<string> missedItemsText { get; set; }
        public CallDetail()
        {
            systemData = new CallSystemData();
            metaData = new CallMetaData();
            customData = new CallMetaData();
            callMissedItems = new List<QuestionDetails_v2>();
        }

       
    }


    public class CallMissedItem
    {
        public int callId;
        public float position;
        public string itemDescription;
    }
    public class Comment
    {
        public string comment;
        public string type;
        public int callID;
    }
    #region V2   

    public class CallMetaData
    {
        public DateTime? callDate { get; set; }
        public String agentName { get; set; }
        public String agentId { get; set; }
        public String agentGroup { get; set; }
        public String campaign { get; set; }
        public String sessionId { get; set; }
        public String profileId { get; set; }
        public String prospectFirstName { get; set; }
        public String prospectLastName { get; set; }
        public String prospectPhone { get; set; }
        public String prospectEmail { get; set; }

        public static CallMetaData Create(IDataRecord reader)
        {
            return new CallMetaData
            {
                callDate = reader.GetValueOrDefault<DateTime?>("callDate"),
                agentId = reader.Get<String>("agentId"),
                agentGroup = reader.Get<String>("agentGroup"),
                campaign = reader.Get<String>("campaign"),
                agentName = reader.Get<String>("agentName"),
                sessionId = reader.Get<String>("sessionId"),
                profileId = reader.Get<String>("profileId"),
                prospectFirstName = reader.Get<String>("prospectFirstName"),
                prospectLastName = reader.Get<String>("prospectLastName"),
                prospectPhone = reader.Get<String>("prospectPhone"),
                prospectEmail = reader.Get<String>("prospectEmail")
            };
        }
    }

    public class CallDetailV2
    {
        public CallSystemData systemData { get; set; }
        public CallMetaData metaData { get; set; }
        public List<CallMissedItem> callMissedItems { get; set; }
    }

    public class CallDetailsResponseData
    {
        public List<CallDetailV2> calls { get; set; }
        public int itemsTotal { get; set; }
    }


    public class CallSystemMetaData
    {
       
        public List<CallDetail> callDetails;
        public CallSystemMetaData()
        {

            callDetails = new List<CallDetail>();
        }

        public static CallSystemMetaData Create(IDataReader reader)
        {
            CallSystemMetaData callSystemMetaData = new CallSystemMetaData();
            while (reader.Read())
            {

                var metaDataItem = CallMetaData.Create(reader);
                var systemDataItem = CallSystemData.Create(reader);

                callSystemMetaData.callDetails.Add(new CallDetail
                {
                    metaData = metaDataItem,
                    systemData = systemDataItem
                });
            }
            return callSystemMetaData;
        }
    }
    #endregion
}