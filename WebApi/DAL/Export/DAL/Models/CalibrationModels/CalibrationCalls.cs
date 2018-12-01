using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Models.CalibrationModels
{
    public class CalibrationCalls
    {
        public DateTime dateAdded { get; set; }
        public CalibrationStatus ccSide { get; set; }
        public CalibrationStatus clientSide { get; set; }
        public CalibrationStatus status { get; set; }
        public int calibrationId { get; set; }
        public Scorecard scorecard { get; set; }
        public string phone { get; set; }
        public DateTime callDate { get; set; }
        public DateTime weekEndDate { get; set; }
        public float callLength { get; set; }
        public string callType { get; set; }
         
        public int callId { get; set; }
        public bool ownedCall { get; set; }
        public int? reviewTime { get; set; }
        public List<CompletedUserList> completedUserList { get; set; }
    }
}