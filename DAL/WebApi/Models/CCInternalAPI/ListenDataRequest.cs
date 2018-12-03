using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    #region Public Properties
    /// <summary>
    /// ListenDataRequest
    /// </summary>
    public class ListenDataRequest
    {
        public ListenDataPost LD;
        public List<FormQScores> FQS;
        public List<FormQResponses> FQR;
        public List<FormQScoresOptions> FQSO;
        public List<SystemComments> SC;
        public List<ClerkedData> CD;
        public List<CommentData> Comments;
        public bool is_practice;
        public string bad_reason;
    }
    #endregion ListenDataRequest
}