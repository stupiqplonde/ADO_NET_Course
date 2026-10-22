
namespace College_DB.Models
{
    public class Student
    {
        public int StudentId { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

    }
}