using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class UserObject
    {
        public string UserRole;
        public List<LinkList> UserLinks;
        public string UserTitle;
        public string UserName;
        public string SpeedInc;
        public bool StartImmediately;
        public List<UserModule> modules;
        public List<MyScorecards> scorecards;
    }
}