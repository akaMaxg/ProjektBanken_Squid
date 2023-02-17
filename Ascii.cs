using Spectre.Console;

namespace ProjektBankenSquid2
{
    public class Ascii
    {
        //prints out an img of a squid
        public static void HentaiSquid()
        {
            var image = new CanvasImage("squid2.png");

            // Set the max width of the image.
            // If no max width is set, the image will take
            // up as much space as there is available.
            image.MaxWidth(64);

            // Render the image to the console
            AnsiConsole.Write(image);
        }
        //Function to print out ascii art of Login text using Spectre.Console
        public static void AsciiSquidBank()
        {
            var font = FigletFont.Load("starwars.flf");
            AnsiConsole.Write(
            new FigletText(font, "Squid Bank")
                //.LeftJustified()
                .Color(Color.SteelBlue1));

        }
        //Function to print out ascii art in the admin menu using Spectre.Console
        public static void AsciiAdminMenu(List<User> activeUser)
        {
            AnsiConsole.Write(
            new FigletText("Admin")
                //.LeftJustified()
                .Color(Color.Yellow));
            AnsiConsole.Write(
            new FigletText($"{activeUser[0].first_name}")
                .Centered()
                .Color(Color.Yellow));
        }
        //Function to print out ascii art in the adminclient menu using Spectre.Console
        public static void AsciiClientAdminMenu(List<User> activeUser)
        {
            AnsiConsole.Write(
            new FigletText($"ClientAdmin")
                //.LeftJustified()
                .Color(Color.Orange1));
            AnsiConsole.Write(
            new FigletText($"{activeUser[0].first_name}")
                .Centered()
                .Color(Color.Orange1));
        }
        //Function to print out ascii art in the client menu using Spectre.Console
        public static void AsciiClientMenu(List<User> activeUser)
        {
            AnsiConsole.Write(
            new FigletText($"{activeUser[0].first_name}")
                .Centered()
                .Color(Color.SteelBlue1));
        }
        public static void AsciiViewAccounts()
        {
            AnsiConsole.Write(
            new FigletText("Your accounts")
                .Centered()
                .Color(Color.Green));
        }
        public static void AsciiTransfer()
        {
            AnsiConsole.Write(
            new FigletText("Transfer")
                .Centered()
                .Color(Color.Green));
        }
        public static void AsciiDeposit()
        {
            AnsiConsole.Write(
            new FigletText("Deposit")
                .Centered()
                .Color(Color.Green));
        }
        public static void AsciiWithdraw()
        {
            AnsiConsole.Write(
            new FigletText("Withdraw")
                .Centered()
                .Color(Color.Green));
        }
        public static void AsciiNewAccount()
        {
            AnsiConsole.Write(
            new FigletText("Open new account")
                .Color(Color.Orange1));
        }
        public static void AsciiNewUserAccount()
        {
            AnsiConsole.Write(
            new FigletText("Create new user account")
                .Color(Color.Orange1));
        }
        public static void AsciiUnlockUser()
        {
            AnsiConsole.Write(
            new FigletText("Unlock user")
                .Centered()
                .Color(Color.Red));
        }
        public static void AsciiTransactionHistory()
        {
            AnsiConsole.Write(
            new FigletText("Transaction history")
                .Centered()
                .Color(Color.Yellow));
        }
        //Animation of a loading bar made for listing accounts
        public static void LoadingAccounts()
        {
            AnsiConsole.Progress()
               .StartAsync(async ctx =>
               {
                   // Define tasks
                   var task1 = ctx.AddTask("[green]Loading accounts[/]");


                   while (!ctx.IsFinished)
                   {
                       // Simulate some work
                       await Task.Delay(150);

                       // Increment
                       task1.Increment(7.5);

                   }
               });
        }
        //Animation of a loading bar made for transfers
        public static void LoadingTransfer()
        {
            AnsiConsole.Progress()
                   .StartAsync(async ctx =>
                   {
                       // Define tasks
                       var task1 = ctx.AddTask("[green]Transfering[/]");


                       while (!ctx.IsFinished)
                       {
                           // Simulate some work
                           await Task.Delay(150);

                           // Increment
                           task1.Increment(10.5);

                       }
                       Console.WriteLine("  Transfer successfully completed");
                   });
        }
    }
}
