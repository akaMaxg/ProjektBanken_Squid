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
        public static List<User> CheckLogin()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                Console.Write("Enter name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter pincode: ");
                string pinCode = Console.ReadLine();


                var output = cnn.Query<User>($"SELECT * FROM bank_user WHERE first_name = '{firstName}' AND pin_code = '{pinCode}'", new DynamicParameters()).ToList();
                var outputTwo = cnn.Query<User>($"SELECT * FROM bank_user WHERE first_name = '{firstName}'", new DynamicParameters()).ToList(); //Sets user for wrong code increment
                if (outputTwo[0].login_attempt <= 3) //If a user with matchning name exists, check so attempts <=3
                {
                    if (output.Any()) //if output returns anything, a match on both name and pin - continue
                    {
                        Console.WriteLine($"Log in successfull, welcome {output[0].first_name} {output[0].last_name}");
                        cnn.Query<User>($"UPDATE bank_user SET login_attempt = '0' WHERE first_name = '{firstName}' AND pin_code = '{pinCode}'", new DynamicParameters()); //resets log in counter
                        return output;
                    }
                    else
                    {
                        cnn.Query<User>($"UPDATE bank_user SET login_attempt = login_attempt + '1' WHERE first_name = '{firstName}'", new DynamicParameters()); //increments login counter for user
                        Console.WriteLine($"Login failed, incorrect name or pincode. Try again. \r\nFailed attempt(s): {outputTwo[0].login_attempt + 1}, account will be locked after 3 failed attempts."); //Information
                        CheckLogin(); //Prompts the user to log in again
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Your account is locked due to too many failed login attempts. Only system administrator can unlock it"); //If user has failed too many times, will be locked until login_attempts maunally resets to 0
                    return null;
                }
            }

            // Kopplar upp mot DB:n
            // läser ut en användare med specifikt namn och pinkod
            // Returnerar en lista med en träff
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


        //Loan functionality
        public static void Loan(int receiverAccount, decimal loanAmount)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance + '{loanAmount}' " +
                    $"WHERE bank_account.id = '{receiverAccount}'", new DynamicParameters());
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