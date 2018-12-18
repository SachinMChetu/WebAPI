using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{

    public class ScorecardData
    {
        public List<SectionData> Sections;
        public UserObject ScorecardUser;
        public string Score;
        public CallScores CallScore;
    }

}