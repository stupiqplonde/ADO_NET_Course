
namespace College_DB.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Score { get; set; }
        public DateTime GradeDate { get; set; }

        public Student? Student { get; set; }
        public Subject? Subject { get; set; }
    }
}
