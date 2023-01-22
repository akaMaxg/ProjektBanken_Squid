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