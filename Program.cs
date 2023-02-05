using System.Configuration.Internal;
using System.Data.Entity;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<User> activeUser = Database.RunProgram();
            List<Account> activeAccount = Database.UserAccount(activeUser);


            Database.Loan(activeAccount);
            Database.Transfer(activeAccount);
            Database.ExternalTransfer(activeAccount);
            Database.UserAccount(activeUser);

            //User newUser = new User("Frank", "Sinatra", "1111");
            //database.SaveBankUser(newUser);
            //Console.WriteLine("User added");
            //Functions.LogIn();
        }
    }
}