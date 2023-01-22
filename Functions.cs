namespace ProjektBankenSquid2
{
    public class Functions
    {
        //Funktion för att skapa användare
        public static User CreateUser()
        {
            Console.Write("Enter name of new user: ");
            string inputName = Console.ReadLine();
            Console.Write("Enter pincode for new user: ");
            int inputPin = int.Parse(Console.ReadLine());
            User newUser = new User(inputName, inputPin);
            return newUser;
        }

        //Funktion för inlogg
        //Funktion för att säkerställa att rätt typ av input ges int/string**

        public static bool LogIn() // Vill vi hämta in alla användare som en lista? Vill vi injecta användarens input i SQL query och hämta därifrån? Vill vi göra en variant? Vill vi ha kodsnipp så en användare inte kan skriva egen SQL
        {
            bool check = false;
            Console.WriteLine("Enter username: ");
            string inputUserName = Console.ReadLine();
            List<string> users = database.GetUsers();
            foreach (var item in users)
            {
                Console.WriteLine(item);
                if (inputUserName == item) // Add second check for pincode if username = item
                {
                    Console.WriteLine("Välkommen");
                    check = true;
                }
            }
            if (check == false) // Add second check for pincode if username = item
            {
                Console.WriteLine("Fel inloggningsuppgifter");
            }
            return false;
        }
        public static void MockLogIn() //
        {
            //User newUser = Functions.CreateUser();
            User mockUser1 = new User("Frank", 1111);
            User mockUser2 = new User("Berit", 2222);
            Console.WriteLine("Current users: " + mockUser1.name + " and " + mockUser2.name);
            Console.WriteLine("Enter username: ");
            string inputUserName = Console.ReadLine();
            Console.WriteLine("Enter pincode: ");
            int inputPinCode = int.Parse(Console.ReadLine());

            if (inputUserName == mockUser1.name && inputPinCode == mockUser1.pincode || inputUserName == mockUser2.name && inputPinCode == mockUser2.pincode)
            {
                Console.WriteLine("Välkommen!");
            }
            else
            {
                Console.WriteLine("Fel");
            }
        }
    }
}



