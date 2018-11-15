using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CDService
{
    public class Media
    {
        public string mediaid;
        public string status;
        public Metadata metadata;
        public Transcripts transcripts;

        public Job job;
        public string dateCreated;
        public Keyword keywords;
        public Topic topics;
        public object predictions;
    }
}