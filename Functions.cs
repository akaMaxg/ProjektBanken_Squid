using Spectre.Console;

namespace ProjektBankenSquid2
{
    public class Functions
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
            new FigletText(font,"Squid Bank")
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

        public static void AdminMenu(List<User> activeUser)
        {

            while (true)
            {
                Console.Clear();
                List<Account> activeAccount = Database.UserAccount(activeUser);
                var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select desired task:")
                    .PageSize(3)
                    .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                    .AddChoices(new[] {
                          "Unlock user", "Create new user account", "Log out",

                    }));
                switch (selectedOption)
                {
                    case "Create new user account":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Create new user account selected");
                        Console.Clear();
                        Database.CreateUser(activeUser);
                        Console.ReadLine();
                        break;

                    case "Unlock user":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Unlock user selected");
                        Console.Clear();
                        Database.UnlockUser(activeUser);
                        Console.ReadLine();
                        break;
                    case "Log out":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Log out selected");
                        Console.WriteLine("---------------------------------------------");
                        Console.Clear();
                        Database.RunProgram();
                        break;
                }
            }
        }
        public static void ClientAdminMenu(List<User> activeUser)
        {
                while (true)
                {
                    Console.Clear();
                    List<Account> activeAccount = Database.UserAccount(activeUser);
                    var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select desired task:")
                        .PageSize(9)
                        .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                        .AddChoices(new[] {
                         "View your bank accounts", "Transfer between own accounts", "Transfer to other account",
                         "Deposit money", "Withdraw money", "Open new account", "Unlock user", "Create new user account", "Log out",

                        }));
                    switch (selectedOption)
                    {
                        case "View your bank accounts":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("See accounts and balance selected");
                            Console.Clear();
                            LoadingAccounts();
                            Thread.Sleep(2300);
                            Database.SeeAccountsAndBalance(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Transfer between own accounts":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Transfer between own accounts selected");
                            Database.ListUserAccounts(activeAccount);
                            Database.Transfer(activeAccount);
                            Console.ReadLine();

                            break;
                        case "Transfer to other account":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Transfer to other account selected");
                            Database.ListUserAccounts(activeAccount);
                            Database.ExternalTransfer(activeAccount);
                            Console.ReadLine();

                            break;
                        case "Deposit money":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Deposit Money selected");
                            Database.ListUserAccounts(activeAccount);
                            Database.Deposit(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Withdraw money":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Withdraw Money selected");
                            Database.ListUserAccounts(activeAccount);
                            Database.Withdraw(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Open new account":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Create account selected");
                            Database.CreateAccount(activeUser);
                            Console.ReadLine();
                            break;
                        case "Create new user account":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Create new user account selected");
                            Console.Clear();
                            Database.CreateUser(activeUser);
                            Console.ReadLine();
                            break;

                        case "Unlock user":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Unlock user selected");
                            Console.Clear();
                            Database.UnlockUser(activeUser);
                            Console.ReadLine();
                            break;
                        case "Log out":
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Log out selected");
                            Console.WriteLine("---------------------------------------------");
                            Console.Clear();
                            Database.RunProgram();
                            break;
                    }

                }
            }
       
        public static void Menu(List<User> activeUser)
        {
            while (true)
            {
                Console.Clear();
                List<Account> activeAccount = Database.UserAccount(activeUser);
                var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select desired task:")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                    .AddChoices(new[] {
                         "View your bank accounts", "Transfer between own accounts", "Transfer to other account",
                         "Deposit money", "Withdraw money", "Open new account", "Log out",

                    }));
                switch (selectedOption)
                {
                    case "View your bank accounts":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("See accounts and balance selected");
                        Console.Clear();
                        LoadingAccounts();
                        Thread.Sleep(2300);
                        Database.SeeAccountsAndBalance(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Transfer between own accounts":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Transfer between own accounts selected");
                        Database.ListUserAccounts(activeAccount);
                        Database.Transfer(activeAccount);
                        Console.ReadLine();

                        break;
                    case "Transfer to other account":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Transfer to other account selected");
                        Database.ListUserAccounts(activeAccount);
                        Database.ExternalTransfer(activeAccount);
                        Console.ReadLine();

                        break;
                    case "Deposit money":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Deposit Money selected");
                        Database.ListUserAccounts(activeAccount);
                        Database.Deposit(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Withdraw money":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Withdraw Money selected");
                        Database.ListUserAccounts(activeAccount);
                        Database.Withdraw(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Open new account":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Create account selected");
                        Database.CreateAccount(activeUser);
                        Console.ReadLine();
                        break;
                    case "Log out":
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Log out selected");
                        Console.WriteLine("---------------------------------------------");
                        Console.Clear();
                        Database.RunProgram();
                        break;
                }
                
            }
        }
    }
}
