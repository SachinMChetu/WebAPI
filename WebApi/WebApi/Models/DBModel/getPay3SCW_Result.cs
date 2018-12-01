using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DBModel
{
    public class getPay3SCW_Result
    {
    public string reviewer { get; set; }
    public DateTime? startdate { get; set; }
    public float Base{ get; set; }
    public string calltime { get; set; }
    public string reviewtime { get; set; }
    public double efficiency { get; set; }
   
    public string appname { get; set; }
    public int num_calls { get; set; }

    public double cal_percent { get; set; }
    public decimal eff_percent { get; set; }
    public decimal calibration_score { get; set; }
    public int num_calibrations { get; set; }
    public int num_disputes { get; set; }
     public int scorecard { get; set; }
      public decimal deduct { get; set; }
        public string short_name { get; set; }
    //public float app_difficulty { get; set; }
    public string sc_date { get; set; }
    public int websites { get; set; }
    public double website_pay { get; set; }
}
}