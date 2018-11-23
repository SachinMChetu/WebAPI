using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    #region Public Properties
    /// <summary>
    /// TrainingCall
    /// </summary>
    public class TrainingCall
    {
        public ListenCall LC;
        public List<training_item> training_items;
    }
    #endregion TrainingCall
}