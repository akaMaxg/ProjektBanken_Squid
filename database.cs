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

namespace ProjektBankenSquid2
{

    public class Database
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        public static void SaveBankUser(User user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
               
                cnn.Execute("insert into bank_user (first_name, last_name, pin_code) values (@first_name, @last_name, @pin_code)", user);

            }
        }

        public static List<User> LoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<User>("SELECT * FROM bank_user", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }

        public static List<User> LoadBankAccounts()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<User>("SELECT * FROM bank_account", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
        }
        public static List<User> CheckLogin(string firstName, string pinCode)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<User>($"SELECT * FROM bank_user WHERE first_name = '{firstName}' AND pin_code = '{pinCode}'", new DynamicParameters());
                output.ToList();
                //Console.WriteLine(output);
                return output.ToList();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }

        //Transfers money between own accounts
        public static void Transfer(int accountTransfer, int accountReciever, int transferAmount)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{transferAmount}' WHERE bank_account.id = '{accountTransfer}'; UPDATE bank_account SET balance = balance + '{transferAmount}' WHERE bank_account.id = '{accountReciever}'", new DynamicParameters());
            }
        }
        //Lists all user accounts based of id
        public static List<Account> UserAccount(int id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Account>($"SELECT * FROM bank_account WHERE user_id = '{id}'", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
        }
        //Returns the information of someone with a specific account number. 
        public static List<Account> ForeignAccount(int accountNumber)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Account>($"SELECT * FROM bank_account WHERE account_number = '{accountNumber}'", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
        }

        //Transfer funds, but to an account that is not logged in 
        public static void ExternalTransfer(int accountTransfer, int accountReciever, int transferAmount)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{transferAmount}' WHERE bank_account.id = '{accountTransfer}'; UPDATE bank_account SET balance = balance + '{transferAmount}' WHERE bank_account.account_number = '{accountReciever}'", new DynamicParameters());
            }
        }

        //Create a new account

        public static void CreateAccount(Account account)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                cnn.Execute("insert into bank_account (name, interest_rate, user_id, currency_id, balance, account_number) values (@name, @interest_rate, @user_id, @currency_id, @balance, @account_number)", account);

            }
        }
    }
}