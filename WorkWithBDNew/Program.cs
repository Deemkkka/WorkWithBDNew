using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WorkWithBDNew
{
    internal class Program
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentsDB"].ConnectionString;
        private static SqlConnection sqlConnection = null;
        
        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("StudentsApp");

            SqlDataReader sqlDataReader = null;
            string command = string.Empty;

            while (true)
            {
                Console.Write("> ");
                command = Console.ReadLine();

                #region Exit
                if (command.ToLower().Equals("exit"))
                {
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                    if (sqlDataReader != null)
                    {
                        sqlDataReader.Close();
                    }
                    break;
                }
                #endregion

                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                try
                {
                    switch (command.Split(' ')[0].ToLower())
                    {
                        case "select":
                            Console.ForegroundColor = ConsoleColor.Green;
                            sqlDataReader = sqlCommand.ExecuteReader();
                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]}  {sqlDataReader["FIO"]}"
                                    + $"  {sqlDataReader["Birthday"]}   {sqlDataReader["University"]}" +
                                    $"  {sqlDataReader["GroupNumber"]}   {sqlDataReader["Course"]}   {sqlDataReader["AverageScore"]}");
                                Console.WriteLine(new string('-', 30));
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }
                            break;
                        case "insert":

                            Console.WriteLine($"Добавлено: {sqlCommand.ExecuteNonQuery()} строк");

                            break;
                        case "update":

                            Console.WriteLine($"Изменено: {sqlCommand.ExecuteNonQuery()} строк");

                            break;
                        case "delete":

                            Console.WriteLine($"Удалено: {sqlCommand.ExecuteNonQuery()} строк");

                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Команда: {command} некорректна");
                            break;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
               
            }

            Console.WriteLine("Для продолжения нажми на любую клавишу");
            Console.ReadKey();
        }
    }
}
