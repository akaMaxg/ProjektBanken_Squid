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
}