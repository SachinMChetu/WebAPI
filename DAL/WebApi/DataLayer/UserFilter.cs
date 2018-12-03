namespace WebApi.DataLayer
{

    /// <summary>
    /// default object state is null this object can be not null only for USER TAB FILTER
    /// </summary>
    public class UserFilter
    {
        public string userId { get; set; }

        /// <summary>
        /// assignedto,closedby,openedby
        /// </summary>
        public string action { get; set; }
    }
}