namespace DAL.Models
{
    public class AppSettings
    {
        public int? naAffect { get; set; }
        public bool showSectionScore { get; set; }
        public int budget { get; set; }
        public int rejectionProfile { get; set; }
        public int id { get; set; }
        public string appname { get; set; }
        public string contactName { get; set; }
        public string contactPhone { get; set; }
        public string contactEmail { get; set; }
        public bool active { get; set; }/*--na Scoring*/
        public string firstNotificationAssigee { get; set; }
        public int? notificationProfileId { get; set; }
        public bool streamOnly { get; set; }
        public string logo { get; set; }
        public string smallLogo { get; set; }
        public string recordingUrl { get; set; }
        public string recordingUser { get; set; }
        public string recordPassword { get; set; }
        public string recordFormat { get; set; }
        public string recordingDirictories { get; set; }
        public float transcriptRate { get; set; }
        public int minimumMinutes { get; set; }
    }
}