using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CCInternalAPI
{
    public class UOData
    {
        public string form_id;
        public string option_list;
        public string QID;
        public string website_link;
        public string custom_comment;
        public List<OptionObjects> option_array;
    }
}