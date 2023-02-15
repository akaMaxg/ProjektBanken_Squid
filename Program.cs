using System.Configuration.Internal;
using System.Data.Entity;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            
            bool introScreen = true;
            Functions.HentaiSquid();
            while (introScreen == true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {

                    case ConsoleKey.Enter:
                        introScreen = false;
                        Console.Clear();
                        Database.RunProgram();
                        break;
                    case ConsoleKey.Spacebar:
                        introScreen = false;
                        Console.Clear();
                        Database.RunProgram();
                        break;
                }
            }
            


        }
    }
}