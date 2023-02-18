using System.Configuration.Internal;
using System.Data.Entity;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.RunIntro();
            Database.RunProgram();
        }
    }
}