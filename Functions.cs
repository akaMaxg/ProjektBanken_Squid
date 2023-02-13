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

                for (int i = 0; i < 6; i++)
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
                        if (selectedOption < 5)
                        {
                            selectedOption++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedOption)
                        {
                            case 0:
                                Console.WriteLine("See accounts and balance selected");
                                Database.ListUserAccounts(activeAccount);
                                Console.ReadLine();
                                break;
                            case 1:
                                Console.WriteLine("Transfer between own accounts selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.Transfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 2:
                                Console.WriteLine("Transfer to other account selected");
                                Database.ListUserAccounts(activeAccount);
                                Database.ExternalTransfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 3:
                                Console.WriteLine("Loan money selected");
                                //Database.Loan(activeAccount);
                                Console.ReadLine();

                                break;
                            case 4:
                                Console.WriteLine("Create account selected");
                                Database.CreateAccount(activeUser);
                                break;
                            case 5:
                                Console.WriteLine("Log out the bank selected");
                                return;
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
                    return "Loan money";
                case 4:
                    return "Create account";
                case 5:
                    return "Log out the bank";
                default:
                    return "";
            }
        }
    }
}
