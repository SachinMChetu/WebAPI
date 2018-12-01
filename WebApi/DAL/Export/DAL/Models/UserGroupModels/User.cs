using System;
using System.Collections.Generic;

namespace DAL.Models.UserGroupModels
{
    public class EditUserModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string defaultPage { get; set; }
        public string userRole { get; set; }
        public string userManager { get; set; }
        public bool? nonEdit { get; set; }
        public bool? nonCalibrating { get; set; }
        public bool? nondashboardAccess { get; set; }
        public bool? updateOlderData { get; set; }
        public bool? forceRewiew { get; set; }
        public bool? excludeCalls { get; set; }
        public DateTime? lastLogin { get; set; }
        public string whoAdded { get; set; }
        public string password { get; set; }
        public DateTime? dateAdded { get; set; }
        public bool? active { get; set; }
        //public GroupInfo group { get; set; }
        //public int? scorecardId { get; set; }
        
    }

    public class UserModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string defaultPage { get; set; }
        public string userRole { get; set; }
        public string userManager { get; set; }
        //public string supervisor { get; set; }
        public string client { get; set; }
        public bool? nonEdit { get; set; }
        public bool? nonCalibrating { get; set; }
        public bool? nondashboardAccess { get; set; }
        public bool? updateOlderData { get; set; }
        public bool? forceRewiew { get; set; }
        public bool? excludeCalls { get; set; }
        public DateTime? lastLogin { get; set; }
        public string whoAdded { get; set; }
        public DateTime? dateAdded { get; set; }
        public string password { get; set; }
        public int scorecardId { get; set; }
        public string appName { get; set; }
        public int groupId { get; set; }
    }
}