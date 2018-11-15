using System;
using System.Collections.Generic;

namespace DAL.Models.UserGroupModels
{
    public class UserInfo
    {
        public EditUserModel user { get; set; }
        public List<string> roles { get; set; }
        public List<Models.User> managers { get; set; }
    }
}