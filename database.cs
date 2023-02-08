using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Policy;
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

        public static void RunProgram()
        {
            Console.WriteLine("Welcome to Squid-bank");
            List<User> users = Database.LoadBankUsers();
            foreach (User user in users)
            {
                Console.WriteLine($"Hello {user.first_name} your last name is {user.pin_code}");
            }
            List<User> activeUser = Database.CheckLogin();
            Functions.Menu(activeUser);
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
                        return output; //returns the list with user
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

        //Lists all user accounts based of id
        public static List<Account> UserAccount(List<User> user)
        {

            //List<Account> activeAccounts = Database.UserAccount(activeUser[0].id);

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Account>($"SELECT * FROM bank_account WHERE user_id = '{user[0].id}'", new DynamicParameters()).ToList();
                return output;
            }
        }
        //Returns the information of someone with a specific account number. 

        public static void ListUserAccounts(List<Account> accounts)
        {
            int counter = 1;
            foreach (var item in accounts) //Lists accounts and balances with numbers
            {
                Console.WriteLine($"{counter}. {item.name}, {item.balance}");
                counter++;
            }
        }

        //Transfers money between own accounts
        public static void Transfer(List<Account> activeAccounts)
        {

            Console.Write("Type the account you want to transfer from: "); //From
            int choiceFrom = int.Parse(Console.ReadLine());
            int idOne = 0;
            int idTwo = 0;

            switch (choiceFrom)
            {
                case 1:
                    idOne = activeAccounts[0].id;
                    break;
                case 2:
                    idOne = activeAccounts[1].id;
                    break;
                case 3:
                    idOne = activeAccounts[2].id;
                    break;
                default:
                    break;
            }
            Console.Write("Type the account you want to transfer to: "); //To
            int choiceTo = int.Parse(Console.ReadLine());
            switch (choiceTo)
            {
                case 1:
                    idTwo = activeAccounts[0].id;
                    break;
                case 2:
                    idTwo = activeAccounts[1].id;
                    break;
                case 3:
                    idOne = activeAccounts[2].id;
                    break;
                default:
                    break;
            }


            Console.WriteLine("How much money do you want to transfer? ");
            int amount = int.Parse(Console.ReadLine());
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{amount}' WHERE bank_account.id = '{idOne}'; UPDATE bank_account SET balance = balance + '{amount}' WHERE bank_account.id = '{idTwo}'", new DynamicParameters());
                Console.WriteLine("Successful transfer");
            }
        }

        //Transfer funds, but to an account that is not logged in 
        public static void ExternalTransfer(List<Account> account)
        {

            Console.Write("Enter another users account number, eg. 101: "); //Test to transfer to external user with known account_number - uses the same account as previous
            int reciever = int.Parse(Console.ReadLine());


            Console.Write("Enter amount"); //Test to transfer to external user with known account_number - uses the same account as previous
            int amount = int.Parse(Console.ReadLine());


            List<Account> accountX = Database.ForeignAccount(reciever); //Finds accounts with specific account number
            int accountReciever = accountX[0].account_number; //Sets reciever to the new account
            Console.WriteLine($"Account {accountX[0].account_number}, {accountX[0].name}");

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{amount}' WHERE bank_account.id = '{account[0].id}'; UPDATE bank_account SET balance = balance + '{amount}' WHERE bank_account.account_number = '{accountReciever}'", new DynamicParameters());
                Console.WriteLine($"Successfully transfered {amount} from {account[0].id} to {accountReciever}");

            }
        }

        public static List<Account> ForeignAccount(int accountNumber)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Account>($"SELECT * FROM bank_account WHERE account_number = '{accountNumber}'", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
        }

        //Loan functionality
        public static void Loan(List<Account> recieverAccount)
        {
            Console.WriteLine("How much money would you like to loan?");
            double loanAmount = double.Parse(Console.ReadLine());
            double interestRate = 0.0570;
            double interest = interestRate * loanAmount;
            Console.WriteLine($"The interest rate for this loan is {interest}");
            Console.WriteLine("Please enter receiver account");
            ListUserAccounts(recieverAccount);
            int receiverAccount = int.Parse(Console.ReadLine());
            ////calls the list accounts function on row 29 above to list accounts and
            ////let user pic one, this will be receiverAccount sent to Loan function

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance + '{loanAmount}' " +
                    $"WHERE bank_account.id = '{receiverAccount}'", new DynamicParameters());
            }
            Console.WriteLine($"You have successfully loaned {loanAmount}");
        }

        //Create a new account
        public static void CreateAccount(List<User> user)
        {
            var NewAccount = new Account();
            Console.WriteLine("Select account");
            NewAccount.name = Console.ReadLine();
            Console.WriteLine("Select interest rate");
            NewAccount.interest_rate = decimal.Parse(Console.ReadLine());
            NewAccount.user_id = user[0].id;
            Console.WriteLine("Select currency id");
            NewAccount.currency_id = int.Parse(Console.ReadLine());
            Console.WriteLine("Select balance");
            NewAccount.balance = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Select account number");
            NewAccount.account_number = int.Parse(Console.ReadLine());
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into bank_account (name, interest_rate, user_id, currency_id, balance, account_number) values (@name, @interest_rate, @user_id, @currency_id, @balance, @account_number)", NewAccount);
            }
        }
    }
}