using System.Collections.Generic;
using System.Data.SQLite;
using static System.Net.Mime.MediaTypeNames;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //.....Change the pathfile in the database clase to your local db folder......


            Console.WriteLine("Enter your id");
            int userid = int.Parse(Console.ReadLine());

            List<Account> accounts = database.UserAccount(userid);

            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.name} {item.interest_rate}, {item.balance}");
            }
            

            //Console.WriteLine("Welcome to Squid-bank");
            //List <User> users = database.LoadBankUsers();

            //foreach (User user in users)
            //{
            //    Console.WriteLine($"Hello {user.first_name} your last name is {user.last_name}");
            //}

            //Console.Write("Enter name: ");
            //string firstName = Console.ReadLine();
            //Console.Write("Enter pincode: ");
            //string pinCode = Console.ReadLine();
            //users = database.CheckLogin(firstName, pinCode);
            //foreach (User user in users)
            //{
            //    Console.WriteLine($"Log in successfull, welcome {user.first_name} {user.last_name}");
            //}

           

            //database.Transfer(users[0].id, 4, 1000, 1000);

            //User newUser = new User("Frank", "Sinatra", "1111");
            //database.SaveBankUser(newUser);
            //Console.WriteLine("User added");
            //Functions.LogIn();
            //Functions.LogIn();

        }
    }
}