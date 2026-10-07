using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_DB
{
    class Program
    {
        static string conn_str = Connect();
        static void Main()
        {
            AddGroup(conn_str, "РПО-24/2");
            AddGroup(conn_str, "РПО-24/1");
            ShowAllStudents(conn_str);
            Console.WriteLine("log(good)");
            Console.ReadKey();
        }

        static string Connect()
        {
            string connectionString =
                "Data Source=COMP11A1\\SQLEXPRESS;" +
                "Initial Catalog=DataBase;" +
                "Integrated Security=True;" +
                "TrustServerCertificate=True;";
            return connectionString;
        }

        static void AddGroup(string conn_str, string name)
        {
            SqlConnection connection = new SqlConnection(conn_str);
            connection.Open();
            string sql = "INSERT INTO dbo.Groups(GroupName) VALUES(@name)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        static void ShowAllStudents(string conn_str)
        {
            SqlConnection connection = new SqlConnection(conn_str);
            connection.Open();
            string sql = "SELECT * FROM dbo.Students";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string first_name = reader.GetString(1);
                string last_name = reader.GetString(2);
                int age = reader.GetInt32(3);
                int group_id = reader.GetInt32(4);

                Console.WriteLine
                (
                    $"id {id}" +
                    $"first name {first_name}"
                 );

                Console.WriteLine
                (
                    $"{reader[0]} | {reader[1]}"
                );
            }
            connection.Close();
        }
    }
}
