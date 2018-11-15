using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    public class TrainingCallRecord
    {
        public AllCallRecord acr;
        public List<training_item> training_items;
        public bool passed_training;
    }

}