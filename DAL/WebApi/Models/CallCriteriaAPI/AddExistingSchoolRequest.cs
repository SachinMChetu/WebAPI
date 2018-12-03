using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CallCriteriaAPI
{
    public class AddExistingSchoolRequest
    {
        public string SESSION_ID;
        public SchoolItem[] Schools;
    }
}