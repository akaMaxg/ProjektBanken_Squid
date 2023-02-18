﻿using Npgsql.Replication.PgOutput.Messages;

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
    
}