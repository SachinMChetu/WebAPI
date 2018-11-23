namespace DAL.Models
{
    public class UserInformation
    {
        public Role userRole { get; set; }
        public User userData { get; set; }

    }

    public class UserInformationSimple
    {
        public string userRole { get; set; }
        public string userName{ get; set; }
        public int userId { get; set; }

    }
}