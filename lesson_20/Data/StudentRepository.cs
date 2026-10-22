using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class StudentRepository
    {
        private readonly string _conn_str;

        public StudentRepository(string connection_string)
        {
            _conn_str = connection_string;
        }

        public List<Student> GetAllStudents()
        {
            using var connection = new SqlConnection(_conn_str);
            return connection.Query<Student>(
                "SELECT StudentId, FirstName, LastName, Age, GroupId FROM Students ORDER BY StudentId").ToList();
        }

        public int CreateStudent(Student student)
        {
            using var connection = new SqlConnection(_conn_str);
            return connection.QuerySingle<int>(
                @"INSERT INTO Students (FirstName, LastName, Age, GroupId)
                  OUTPUT INSERTED.StudentId
                  VALUES (@FirstName, @LastName, @Age, @GroupId)", student);
        }

        public bool UpdateStudent(int id, Student student)
        {
            using var connection = new SqlConnection(_conn_str);
            return connection.Execute(
                @"UPDATE Students
                  SET FirstName=@FirstName, LastName=@LastName, Age=@Age, GroupId=@GroupId
                  WHERE StudentId=@Id",
                new { student.FirstName, student.LastName, student.Age, student.GroupId, Id = id }) > 0;
        }

        public void DeleteStudent(int id)
        {
            using var connection = new SqlConnection(_conn_str);
            var sql = "DELETE FROM Students WHERE StudentId=@id";
            connection.Execute(sql, id);
        }
    }
}
