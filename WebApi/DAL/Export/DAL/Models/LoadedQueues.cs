using System;

namespace DAL.Models
{
    public class LoadedQueues
    {
        public bool pending { get; set; }
        public DateTime date { get; set; }
        public string fileOwner { get; set; }
        public string url { get; set; }
        public string fileName { get;  set; }
        public int id { get; set; }
        public string type { get; set; }
    }
}