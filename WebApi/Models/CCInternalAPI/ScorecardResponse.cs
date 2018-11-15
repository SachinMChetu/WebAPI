using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    /// <summary>
    /// ScorecardResponse
    /// </summary>
    public class ScorecardResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int QID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RightAnswer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int QAPoints { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ViewLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> QComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Comment> SCRComments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CheckItems> QTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool comments_allowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sentence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OptionVerb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool LeftColumnQuestion { get; set; }
    }
}