namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Squid-bank");
            //User newUser = Functions.CreateUser();
            User mockUser1 = new User("Frank", 1111);
            User mockUser2 = new User("Berit", 2222);
            Console.WriteLine("Current users: " + mockUser1.name + " and " + mockUser2.name );
            Functions.LogIn(mockUser1.name, mockUser1.pincode);
        }
    }
    //Klass för användare
    public class User
    {
        public string name { get; set; }
        public int pincode { get; set; }

        //Konstruktor
        public User(string userName, int userPincode)
        {
            name = userName;
            pincode = userPincode;
        }
    }

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

        public static bool LogIn(string logInName, int logInPin) //
        {
            Console.WriteLine("Enter username: ");
            string inputUserName = Console.ReadLine();
            Console.WriteLine("Enter pincode: ");
            int inputPinCode = int.Parse(Console.ReadLine());
            
            if (inputUserName == logInName && inputPinCode == logInPin)
            {
                Console.WriteLine("Välkommen");
                return true;
            }
            else
            {
                Console.WriteLine("Fel inloggningsuppgifter");
                return false;
            }
        }
    }
}