using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.UserGroupModels
{
    public class EditPasswordModel
    {
        public User user { get; set; }
        public string oldPassword { get; set; }
        public string password { get; set; } 
    }
}
