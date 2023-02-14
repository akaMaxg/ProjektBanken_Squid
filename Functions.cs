namespace ProjektBankenSquid2
{
    public class Functions
    {

        public static void Menu(List<User> activeUser)
        {
            int selectedOption = 0;
            while (true)
            {
                List<Account> activeAccount = Database.UserAccount(activeUser);
                Console.Clear();
                Console.WriteLine("Bank Menu");

                for (int i = 0; i < 7; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.WriteLine("→ " + GetOptionString(i));
                    }
                    else
                    {
                        Console.WriteLine("  " + GetOptionString(i));
                    }
                }

                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("\nUse arrow keys to navigate and 'Enter' to select");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedOption > 0)
                        {
                            selectedOption--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedOption < 6)
                        {
                            selectedOption++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedOption)
                        {
                            case 0:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("See accounts and balance selected");
                                Database.ListUserAccounts(activeAccount);
                                Console.ReadLine();
                                break;
                            case 1:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Transfer between own accounts selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.Transfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 2:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Transfer to other account selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.ExternalTransfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 3:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Deposit Money selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.Deposit(activeAccount);
                                Console.ReadLine();
                                break;
                            case 4:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Withdraw Money selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.Withdraw(activeAccount);
                                Console.ReadLine();
                                break;
                            case 5:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Create account selected");
                                Database.CreateAccount(activeUser);
                                Console.ReadLine();
                                break;
                            case 6:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Log out the bank selected");
                                Console.WriteLine("---------------------------------------------");
                                Console.Clear();
                                Database.RunProgram();
                                break;
                        }
                        break;
                }
            }
        }
        static string GetOptionString(int option)
        {
            switch (option)
            {
                case 0:
                    return "See accounts and balance";
                case 1:
                    return "Transfer between own accounts";
                case 2:
                    return "Transfer to other account";
                case 3:
                    return "Deposit money";
                case 4:
                    return "Withdraw money";
                case 5:
                    return "Create account";
                case 6:
                    return "Log out the bank";
                default:
                    return "";
            }
        }
        public static void AdminMenu(List<User> activeUser)
        {
            int selectedOption = 0;
            while (true)
            {
                List<Account> activeAccount = Database.UserAccount(activeUser);
                Console.Clear();
                Console.WriteLine("Admin Bank Menu");

                for (int i = 0; i < 4; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.WriteLine("→ " + GetOptionStringAdmin(i));
                    }
                    else
                    {
                        Console.WriteLine("  " + GetOptionStringAdmin(i));
                    }
                }

                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("\nUse arrow keys to navigate and 'Enter' to select");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedOption > 0)
                        {
                            selectedOption--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedOption < 6)
                        {
                            selectedOption++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedOption)
                        {
                           
                            case 0:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Create user selected");
                                Database.CreateUser(activeUser);
                                Console.ReadLine();
                                
                                break;
                            case 1:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("unlock user selected");
                                Database.UnlockUser(activeUser);
                                Console.ReadLine();
                                break;
                            case 3:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Log out the bank selected");
                                Console.WriteLine("---------------------------------------------");
                                Console.Clear();
                                Database.RunProgram();
                                break;
                        }
                        break;
                }
            }
        }
        static string GetOptionStringAdmin(int option)
        {
            switch (option)
            {
                case 0:
                    return "Create user";
                case 1:
                    return "Unlock user";
                case 2:
                    return "Log out the bank";
                default:
                    return "";
            }
        }
        public static void ClientAdminMenu(List<User> activeUser)
        {
            int selectedOption = 0;
            while (true)
            {
                List<Account> activeAccount = Database.UserAccount(activeUser);
                Console.Clear();
                Console.WriteLine("ClientAdmin Bank Menu");

                for (int i = 0; i < 9; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.WriteLine("→ " + GetOptionStringClientAdmin(i));
                    }
                    else
                    {
                        Console.WriteLine("  " + GetOptionStringClientAdmin(i));
                    }
                }

                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("\nUse arrow keys to navigate and 'Enter' to select");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedOption > 0)
                        {
                            selectedOption--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedOption < 8)
                        {
                            selectedOption++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedOption)
                        {
                            case 0:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("See accounts and balance selected");
                                Console.Clear();
                                Database.SeeAccountsAndBalance(activeAccount);
                                Console.ReadLine();
                                break;
                            case 1:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Transfer between own accounts selected");
                                Console.Clear();
                                Database.ListUserAccounts(activeAccount);
                                Database.Transfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 2:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Transfer to other account selected");
                                Console.Clear();
                                Database.ListUserAccounts(activeAccount);
                                Database.ExternalTransfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 3:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Deposit Money selected");
                                Console.Clear();
                                Database.ListUserAccounts(activeAccount);
                                Database.Deposit(activeAccount);
                                Console.ReadLine();
                                break;
                            case 4:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Withdraw Money selected");
                                Console.Clear();
                                Database.ListUserAccounts(activeAccount);
                                Database.Withdraw(activeAccount);
                                Console.ReadLine();
                                break;
                            case 5:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Create account selected");
                                Console.Clear();
                                Database.CreateAccount(activeUser);
                                Console.ReadLine();
                                break;
                            case 6:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Create user selected");
                                Console.Clear();
                                Database.CreateUser(activeUser);
                                Console.ReadLine();
                                break;
                            case 7:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("unlock user selected");
                                Console.Clear();
                                Database.UnlockUser(activeUser);
                                Console.ReadLine();
                                break;
                            case 8:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Log out the bank selected");
                                Console.WriteLine("---------------------------------------------");
                                Console.Clear();
                                Database.RunProgram();
                                break;
                        }
                        break;
                }
            }
        }
        static string GetOptionStringClientAdmin(int option)
        {
            switch (option)
            {
                case 0:
                    return "See accounts and balance";
                case 1:
                    return "Transfer between own accounts";
                case 2:
                    return "Transfer to other account";
                case 3:
                    return "Deposit money";
                case 4:
                    return "Withdraw money";
                case 5:
                    return "Create account";
                case 6:
                    return "Create user";
                case 7:
                    return "Unlock user";
                case 8:
                    return "Log out the bank";
                default:
                    return "";
            }
        }
    }
}
