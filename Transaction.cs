using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBankenSquid2
{
    public class Transaction
    {
        public int id { get; set; }
        public string name { get; set; }
        public int from_account_id { get; set; }
        public int to_account_id { get; set; }
        public decimal amount { get; set; }
        public DateTime transaction_time { get; set; }
    }
}
