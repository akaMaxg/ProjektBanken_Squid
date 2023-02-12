
using Newtonsoft.Json;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;

namespace ProjektBankenSquid2
{
   public class Rates
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        //function that updates the exchanges rates in datebase
        public static bool ImportRates()
        {
            try
            {
                String URLString = "https://v6.exchangerate-api.com/v6/32b26456dd41b6e1bc2befd1/latest/SEK";
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URLString);
                    API_Obj rates = JsonConvert.DeserializeObject<API_Obj>(json);
                    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
                    {

                        var output = cnn.Query<User>($"UPDATE bank_currency SET exchange_rate = '{rates.conversion_rates.USD}' WHERE bank_currency.id = '2'; UPDATE bank_currency SET exchange_rate = '{rates.conversion_rates.EUR}' WHERE bank_currency.id = '3'; UPDATE bank_currency SET exchange_rate = '{rates.conversion_rates.GBP}' WHERE bank_currency.id = '4';", new DynamicParameters());

                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
   
    public class API_Obj
    {
        public string result { get; set; }
        public string documentation { get; set; }
        public string terms_of_use { get; set; }
        public string time_last_update_unix { get; set; }
        public string time_last_update_utc { get; set; }
        public string time_next_update_unix { get; set; }
        public string time_next_update_utc { get; set; }
        public string base_code { get; set; }
        public ConversionRate conversion_rates { get; set; }
    }
    public class API_Obj_Convert
    {
        public string result { get; set; }
        public string documentation { get; set; }
        public string terms_of_use { get; set; }
        public string time_last_update_unix { get; set; }
        public string time_last_update_utc { get; set; }
        public string time_next_update_unix { get; set; }
        public string time_next_update_utc { get; set; }
        public string base_code { get; set; }
        public string target_code { get; set; }
        public decimal conversion_result { get; set; }
        public decimal conversion_rate { get; set; }
        
    }

    public class ConversionRate
    {
        
        public decimal EUR { get; set; }
       
        public decimal GBP { get; set; }
       
        public decimal SEK { get; set; }
      
        public decimal USD { get; set; }
      
    }
}




