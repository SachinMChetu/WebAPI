//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Entities
{
    using System;
    
    public partial class GetXccReportNewAudioData_Result
    {
        public int theID { get; set; }
        public string recording_user { get; set; }
        public string record_password { get; set; }
        public string audio_port { get; set; }
        public Nullable<int> file_order { get; set; }
        public int failed_downloads { get; set; }
        public string record_format { get; set; }
        public Nullable<System.DateTime> call_date { get; set; }
        public string file_name { get; set; }
        public string local_file { get; set; }
        public string appname { get; set; }
        public string session_id { get; set; }
        public string theDate { get; set; }
        public string profile_ID { get; set; }
    }
}
