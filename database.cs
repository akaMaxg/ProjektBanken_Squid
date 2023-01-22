using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBankenSquid2
{
    public class database
    {
        public static void AddUsers() //Ta två argument, string och int för username och pin
        {
            string connectionString = "Data Source=F:\\Min enhet\\Personligt\\Skolrelaterat\\Chas Academy, Fullstack C#\\egetKodaHär\\selfProjects\\ProjektBankenSquid2\\dbsquid.db"; //Set to your local db folder. //Varför behöver vi a connection string i funktionen och inte i 'public class'?
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO users(userName, pincode) VALUES('User1', 1111);"; //Eventuellt ändra User1 och 1111 till användarens input
                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<string> GetUsers() //Hämtar och lägger alla användare in en lista
        {
            string connectionString = "Data Source=F:\\Min enhet\\Personligt\\Skolrelaterat\\Chas Academy, Fullstack C#\\egetKodaHär\\selfProjects\\ProjektBankenSquid2\\dbsquid.db"; //Set to your local db folder. //Varför behöver vi a connection string i funktionen och inte i 'public class'?

            List<string> users = new List<string>();
            List<int> pins = new List<int>();


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM users", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string userName = reader["userName"].ToString();
                            int pincode = Convert.ToInt32(reader["pincode"]);
                            users.Add(userName); //alla users
                            pins.Add(pincode); //alla pins????

                        }
                    }
                }
            }
            return users;
        }
    }
}