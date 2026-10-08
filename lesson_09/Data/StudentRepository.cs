using System;
using System.Collections.Generic;
using College.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;

namespace College_DB.Data
{
    class StudentRepository
    {
        private readonly string _conn_str;
        public StudentRepository(string connection_string)
        {
            _conn_str = connection_string;
        }
        public List<Student> GetAllStudents2()
        {
            var students = new List<Student>();
            var connection = new SqlConnection(_conn_str);
            connection.Open();
            string sql = "SELECT StudentId, FirstName, LastName, Age FROM Students";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(
                    new Student
                    {
                        StudentId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Age = reader.GetInt32(3)
                    }
                );
            }
            connection.Close();
            return students;
        }

        public Student GetStudentById2(string id)
        {
            var student = new Student();
            var connection = new SqlConnection(_conn_str);
            connection.Open();
            string sql = "SELECT StudentId, FirstName, LastName, Age FROM Students WHERE StudentId = @id";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                student = new Student
                {
                    StudentId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3)
                };
            }
            connection.Close();
            return student;
        }

        public void CreateStudent2(string first_name, string last_name, string age, string group_id)
        {
            var connection = new SqlConnection(_conn_str);
            connection.Open();
            string sql = "INSERT INTO Students(FirstName, LastName, Age, GroupId) VALUES(@firstName, @lastName, @age, @groupId)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@firstName", first_name);
            command.Parameters.AddWithValue("@lastName", last_name);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@groupId", group_id);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Student> GetAllStudents()
        {
            using(var connection = new SqlConnection(_conn_str))
            {
                return connection
                    .Query<Student>("SELECT StudentId, FirstName, LastName, Age FROM Students")
                    .ToList();
            }
        }

        public Student GetStudentById(string id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection
                    .QueryFirstOrDefault<Student>("SELECT StudentId, FirstName, LastName, Age FROM Students WHERE StudentId = @id", 
                    new { @id = id });
            }
        }

        public void CreateStudent(string first_name, string last_name, string age, string group_id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                var sql = "INSERT INTO Students(FirstName, LastName, Age, GroupId) " +
                    "VALUES(@firstName, @lastName, @age, @groupId);";

                connection.QuerySingle(sql);
            }
        }
    }
}
