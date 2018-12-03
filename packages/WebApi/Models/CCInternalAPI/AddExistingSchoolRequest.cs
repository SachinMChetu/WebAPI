using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    #region Public Properties
    /// <summary>
    /// AddExistingSchoolRequest
    /// </summary>
    public class AddExistingSchoolRequest
    {
        public string SESSION_ID;
        public SchoolItem[] Schools;
    }
    #endregion AddExistingSchoolRequest
}