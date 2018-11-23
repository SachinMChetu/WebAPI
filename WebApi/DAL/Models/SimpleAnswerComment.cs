namespace DAL.Models
{
    public class SimpleAnswerComment
    {
        public int questionID { get; set; }
        public string commentText { get; set; }
        public int callId { get; set; }
    }
}