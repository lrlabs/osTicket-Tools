using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace DBLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("");
            Console.Title = "Collector";
            string ConnectString = "Server=localhost;Database=glpiservice;Uid=root;Pwd=root";
            string query = "SELECT id,name FROM glpi_groups";

            string filePath = "./logs";
            Directory.CreateDirectory(filePath);

            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            MySqlConnection connection = new MySqlConnection(ConnectString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                // Filename
                ostrm = new FileStream("./logs/" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "_" + DateTime.Now.Hour  + "" + DateTime.Now.Minute + "" +               DateTime.Now.Second
                    + ".log",
                    FileMode.OpenOrCreate,
                    FileAccess.Write);
                // Write
                writer = new StreamWriter(ostrm);
                cmd = connection.CreateCommand();
                cmd.CommandText = query;
                MySqlDataReader Reader;
                Reader = cmd.ExecuteReader();
                Console.SetOut(writer);
                while (Reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        row += Reader.GetValue(i).ToString() + " | ";
                    Console.WriteLine(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " -> " + row);
                }
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
