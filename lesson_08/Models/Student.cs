namespace College.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $" |{StudentId}| студент {FirstName} {LastName}. {Age} лет";
        }
    }
}