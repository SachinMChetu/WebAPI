using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getCoachingQueueJson_Result
    {
        public string NotificationID { get; set; }
        public string non_edit { get; set; }
        public string sup_override { get; set; }
        public string agent { get; set; }
        public string total_score { get; set; }
        public string call_date { get; set; }
        public string dateadded { get; set; }
        public int id { get; set; }
        public string phone { get; set; }
        public string notificationStep { get; set; }
        public int? form_id { get; set; }
        public string form_id_plus { get; set; }
        public string first_error { get; set; }
        public string OwnedNotification { get; set; }
        public string AllComments { get; set; }
    }
}