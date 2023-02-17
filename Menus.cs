using Spectre.Console;
using System.Runtime.Intrinsics.X86;
using System.Transactions;

namespace ProjektBankenSquid2
{

    public class Menus
    {
        public static void AdminMenu(List<User> activeUser)
        {

            while (true)
            {
                Console.Clear();
                Ascii.AsciiAdminMenu(activeUser);
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
                        Console.Clear();
                        Ascii.AsciiNewUserAccount();
                        Database.CreateUser();
                        Console.ReadLine();
                        break;

                    case "Unlock user":
                        Console.Clear();
                        Ascii.AsciiUnlockUser();
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
                    Ascii.AsciiClientAdminMenu(activeUser);
                    List<Account> activeAccount = Database.UserAccount(activeUser);
                    var selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select desired task:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                        .AddChoices(new[] {
                         "View your bank accounts", "Transfer between own accounts", "Transfer to other account",
                            "Deposit money", "Withdraw money", "Open new account", "Unlock user", "Create new user account","Transaction history", 
                            "Log out",
                        }));
                switch (selectedOption)
                    {
                        case "View your bank accounts":
                            Console.Clear();
                            Ascii.LoadingAccounts();
                            Thread.Sleep(2300);
                            Console.Clear();
                            Ascii.AsciiViewAccounts();
                            Database.SeeAccountsAndBalance(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Transfer between own accounts":
                            Console.Clear();
                            Ascii.AsciiTransfer();
                            Database.ListUserAccounts(activeAccount);
                            Database.Transfer(activeAccount);
                            Console.ReadLine();

                            break;
                        case "Transfer to other account":
                            Console.Clear();
                            Ascii.AsciiTransfer();
                            Database.ListUserAccounts(activeAccount);
                            Database.ExternalTransfer(activeAccount);
                            Console.ReadLine();

                            break;
                        case "Deposit money":
                            Console.Clear();
                            Ascii.AsciiDeposit();
                            Database.ListUserAccounts(activeAccount);
                            Database.Deposit(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Withdraw money":
                            Console.Clear();
                            Ascii.AsciiWithdraw();
                            Database.ListUserAccounts(activeAccount);
                            Database.Withdraw(activeAccount);
                            Console.ReadLine();
                            break;
                        case "Open new account":
                            Console.Clear();
                            Ascii.AsciiNewAccount();
                            Database.CreateAccount(activeUser);
                            Console.ReadLine();
                            break;
                        case "Create new user account":
                            Console.Clear();
                            Ascii.AsciiNewUserAccount();
                            Database.CreateUser();
                            Console.ReadLine();
                            break;

                        case "Unlock user":
                            Console.Clear();
                            Ascii.AsciiUnlockUser();
                            Database.UnlockUser(activeUser);
                            Console.ReadLine();
                            break;

                        case "Transaction history":
                            Console.Clear();
                            Ascii.AsciiTransactionHistory();
                            Database.GetTransactionByUser(activeAccount);
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
                Ascii.AsciiClientMenu(activeUser);
                List<Account> activeAccount = Database.UserAccount(activeUser);
                var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select desired task:")
                    .PageSize(8)
                    .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                    .AddChoices(new[] {
                         "View your bank accounts", "Transfer between own accounts", "Transfer to other account",
                         "Deposit money", "Withdraw money", "Open new account", "Transaction history", "Log out",

                    }));
                switch (selectedOption)
                {
                    case "View your bank accounts":
                        Console.Clear();
                        Ascii.LoadingAccounts();
                        Thread.Sleep(2300);
                        Console.Clear();
                        Ascii.AsciiViewAccounts();
                        Database.SeeAccountsAndBalance(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Transfer between own accounts":
                        Console.Clear();
                        Ascii.AsciiTransfer();
                        Database.ListUserAccounts(activeAccount);
                        Database.Transfer(activeAccount);
                        Console.ReadLine();

                        break;
                    case "Transfer to other account":
                        Console.Clear();
                        Ascii.AsciiTransfer();
                        Database.ListUserAccounts(activeAccount);
                        Database.ExternalTransfer(activeAccount);
                        Console.ReadLine();

                        break;
                    case "Deposit money":
                        Console.Clear();
                        Ascii.AsciiDeposit();
                        Database.ListUserAccounts(activeAccount);
                        Database.Deposit(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Withdraw money":
                        Console.Clear();
                        Ascii.AsciiWithdraw();
                        Database.ListUserAccounts(activeAccount);
                        Database.Withdraw(activeAccount);
                        Console.ReadLine();
                        break;
                    case "Open new account":
                        Console.Clear();
                        Ascii.AsciiNewAccount();
                        Database.CreateAccount(activeUser);
                        Console.ReadLine();
                        break;

                    case "Transaction history":
                        Console.Clear();
                        Ascii.AsciiTransactionHistory();
                        Database.GetTransactionByUser(activeAccount);
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
