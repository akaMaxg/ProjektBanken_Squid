using Npgsql.Replication.PgOutput.Messages;

namespace ProjektBankenSquid2
{
    //Klass för mock-användare
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string pin_code { get; set; }
        public int role_id { get; set; }    
        public int login_attempt { get; set; }
    }
    public class MockUser
    {
        public int mockid { get; set; }
        public string mockfirst_name { get; set; }
        public string mocklast_name { get; set; }
        public string mockpin_code { get; set; }

        //Konstruktor
        public MockUser(string firstName, string lastName, string pinCode)
        {
            mockfirst_name = firstName;
            mocklast_name = lastName;
            mockpin_code = pinCode;
        }
    }
    
}