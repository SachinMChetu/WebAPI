namespace DAL.Models
{
    /// <summary>
    /// typical class for all dropdowns
    /// can be set to any dropdown
    /// </summary>
   public class DropdownInfo
    {
        /// <summary>
        /// ID's of app or scorecard or notification or something else
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Name of app or scorecard or notification or something else
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// flag to show active inactive status
        /// </summary>
        public bool active { get; set; }
    }


}