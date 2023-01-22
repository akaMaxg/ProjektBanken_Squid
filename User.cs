namespace ProjektBankenSquid2
{
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
}