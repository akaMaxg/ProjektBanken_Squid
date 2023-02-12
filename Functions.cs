namespace ProjektBankenSquid2
{
    public class Functions
    {

        public static void Menu(List<User> activeUser)
        {
            List<Account> activeAccount = Database.UserAccount(activeUser);
            int selectedOption = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Bank Menu");

                for (int i = 0; i < 5; i++)
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

                Console.WriteLine("\b\nSelect an option:");
                Console.WriteLine("\b\nUse arrow keys to navigate and 'Enter' to select");

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
                        if (selectedOption < 4)
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
                                Database.ExternalTransfer(activeAccount);
                                Console.ReadLine();

                                break;
                            case 3:
                                Console.WriteLine("Loan money selected");
                                //Database.Loan(activeAccount);
                                Console.ReadLine();

                                break;
                            case 4:
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
                    return "Log out the bank";
                default:
                    return "";
            }
        }
        public static void MockLogIn() //
        {
            //User newUser = Functions.CreateUser();
            //User mockUser1 = new User("Frank", "1111");
            //User mockUser2 = new User("Berit", "2222");
            //Console.WriteLine("Current users: " + mockUser1.first_name + " and " + mockUser2.first_name);
            //Console.WriteLine("Enter username: ");
            //string inputUserName = Console.ReadLine();
            //Console.WriteLine("Enter pincode: ");
            //int inputPinCode = int.Parse(Console.ReadLine());

            //if (inputUserName == mockUser1.first_name && inputPinCode == mockUser1.pin_code || inputUserName == mockUser2.first_name && inputPinCode == mockUser2.pin_code)
            //{
            //    Console.WriteLine("Välkommen!");
            //}
            //else
            //{
            //    Console.WriteLine("Fel");
            //}
        }
        //public static User CreateUser()

        //Funktion för att skapa användare


        //Funktion för inlogg
        //Funktion för att säkerställa att rätt typ av input ges int/string**
        //{
        //    Console.Write("Enter name of new user: ");
        //    string inputName = Console.ReadLine();
        //    Console.Write("Enter pincode for new user: ");
        //    string inputPin = Console.ReadLine();
        //    User newUser = new User(inputName, inputPin);
        //    return newUser;
        //}
    }
}
