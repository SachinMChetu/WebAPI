namespace DAL.Models
{
    public class AnswerDetails
    {
        public int answerId { get; set; }
        public string answerText { get; set; }
        public bool isRightAnswer { get; set; }
        public int position { get; set; }        
    }
}