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
            Console.WriteLine("Welcome to Squid-bank");
            //Functions.LogIn();
            Functions.LogIn();

        }
    }
}