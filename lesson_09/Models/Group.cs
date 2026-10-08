namespace College.Models
{
    public class Group
    {
        public int StudentId { get; set; }
        public string GroupName { get; set; }

        public override string ToString()
        {
            return $" |{StudentId}| группа {GroupName} ";
        }
    }
}
