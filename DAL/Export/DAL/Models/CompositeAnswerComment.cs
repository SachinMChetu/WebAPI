namespace DAL.Models
{
    public class CompositeAnswerComment
    {
        public int commentId { get; set; }
        public string commentText { get; set; }
        public bool isChecked { get; set; }
        public int? position { get; set; }
        public int questionID { get; set; }
        public int callId { get; set; }
    }
}