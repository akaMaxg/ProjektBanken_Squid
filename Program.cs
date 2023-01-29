using System.Collections.Generic;
using System.Data.SQLite;
using static System.Net.Mime.MediaTypeNames;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to Squid-bank");
            List <User> users = database.LoadBankUsers();

            foreach (User user in users)
            {
                Console.WriteLine($"Hello {user.first_name} your last name is {user.pin_code}");
            }

            Console.Write("Enter name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter pincode: ");
            string pinCode = Console.ReadLine();
            List <User> activeUser = database.CheckLogin(firstName, pinCode);
            Console.WriteLine(activeUser[0].last_name);
            foreach (User user in activeUser)
            {
                Console.WriteLine($"Log in successfull, welcome {user.first_name} {user.last_name}");

            }
            List<Account> accounts = database.UserAccount(activeUser[0].id);

            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.name}, {item.interest_rate}, {item.balance}");
            }

            Console.WriteLine("Type the account you want to transfer from: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Type the account you want to transfer to: ");
            int id2 = int.Parse(Console.ReadLine());
            Console.WriteLine("And how much money do you want to transfer? ");
            int amount = int.Parse(Console.ReadLine());
            database.Transfer(id, id2, amount);
            Console.WriteLine("Successfull transfer");


            //User newUser = new User("Frank", "Sinatra", "1111");
            //database.SaveBankUser(newUser);
            //Console.WriteLine("User added");
            //Functions.LogIn();
            //Functions.LogIn();

        }
    }
}