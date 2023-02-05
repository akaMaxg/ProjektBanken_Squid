using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProjektBankenSquid2
{
    public class Account
        {
        public int account_number { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public decimal interest_rate { get; set; }
        public int user_id { get; set; }
        public int currency_id { get; set; }
        public decimal balance { get; set; }
    }
}