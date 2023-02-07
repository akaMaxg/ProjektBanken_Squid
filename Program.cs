using System.Configuration.Internal;
using System.Data.Entity;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.RunProgram();

            //User newUser = new User("Frank", "Sinatra", "1111");
            //database.SaveBankUser(newUser);
            //Console.WriteLine("User added");
            //Functions.LogIn();
        }
    }
}