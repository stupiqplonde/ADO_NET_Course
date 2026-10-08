using System;
using College_DB.Data;
using Microsoft.Data.SqlClient;


namespace College_DB
{
    class Program
    {
        static string conn_str =
                "Data Source=COMP11A1\\SQLEXPRESS;" +
                "Initial Catalog=College;" +
                "Integrated Security=True;" +
                "TrustServerCertificate=True;";

        static bool is_running = true;

        static StudentRepository student_repo = new StudentRepository(conn_str);

        static GroupRepository group_repo = new GroupRepository(conn_str);

        static void Main()
        {
            Console.WriteLine("Добро пожаловать в пункт принятия решений!");
            Console.WriteLine("Введите номер задачи:");
            while (is_running)
            {
                Console.WriteLine("1. список всех студентов");
                Console.WriteLine("2. список всех групп");
                Console.WriteLine("3. ");
                Console.WriteLine("4. ");
                Console.WriteLine("5. ");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");
                string choise = Console.ReadLine();
                switch (choise)
                {
                    case "1":
                        ShowAllStudents();
                        break;
                    case "2":
                        ShowAllGroups();
                        break;
                    case "0":
                        is_running = false;
                        break;
                    default:
                        Console.WriteLine("Такой команды нет");
                        break;
                }

                if (is_running)
                {
                    Console.WriteLine("Нажмите любую кнопку для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void ShowAllStudents()
        {
            var students = student_repo.GetAllStudents();
            foreach (var student in students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        static void ShowAllGroups()
        {
            var groups = group_repo.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine(group.ToString());
            }
        }
    }
}
