
namespace College_DB.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Grade> Grades { get; set; } = new();
    }
}
