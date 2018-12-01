using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.NotificationModels
{
    public class DisputeModel
    {
        public DateTime? dateClosed { get; set; }
        public string comment { get; set; }
        public DateTime? dateCreated { get; set; }
        public string role { get; set; }
        public string closedBy { get; set; }
        public int? notificationId { get; set; }
        public string assignedTo { get; set; }
    }
}
