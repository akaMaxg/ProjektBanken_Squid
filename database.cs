using Dapper;
using Newtonsoft.Json;
using Npgsql;

using Spectre.Console;

using System.ComponentModel;

using System.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Principal;

namespace ProjektBankenSquid2
{

    public class Database
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        public static void RunIntro()
        {
            bool introScreen = true;
            Ascii.HentaiSquid();
            while (introScreen == true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {

                    case ConsoleKey.Enter:
                        introScreen = false;
                        Console.Clear();
                        break;
                    case ConsoleKey.Spacebar:
                        introScreen = false;
                        Console.Clear();
                        break;
                }
            }
        }
        public static void RunProgram()
        {
            Rates.ImportRates();
          
            Ascii.AsciiSquidBank();
            List<User> users = LoadBankUsers();
            var table = new Table();
            // Sets tables border
            table.Border(TableBorder.Ascii);
            // Adds columns
            table.AddColumn("First Name");
            table.AddColumn(new TableColumn("Pincode"));
            foreach (User user in users)
            {
                table.AddRow($"{user.first_name}", $"{user.pin_code}");
            }
            // Render the table to the console
            AnsiConsole.Write(table);

            // Selection menu for Login or Exit application
            var selectedOption = AnsiConsole.Prompt(
              new SelectionPrompt<string>()
                  .Title("Select desired task:")
                  .PageSize(3)
                  .MoreChoicesText("[grey](Move up and down using arrow keys)[/]")
                  .AddChoices(new[] {
                          "Login", "Exit application",

                  }));
            switch (selectedOption)
            {   
                // Starts the login process 
                case "Login":
                    List<User> activeUser = CheckLogin();
                    while (activeUser.Count == 0)
                    {
                        activeUser = CheckLogin();
                    }
                    if (activeUser[0].role_id == 1)
                    {
                        Menus.AdminMenu(activeUser);
                    }
                    else if (activeUser[0].role_id == 2)
                    {
                        Menus.Menu(activeUser);
                    }
                    else
                    {
                        Menus.ClientAdminMenu(activeUser);
                    }
                    break;
                // Exits the application
                case "Exit application":
                    Environment.Exit(0);
                    break;
                
            }

        }
        // Function to hide what the user types in when entering pincode
        public static string PassPrompt()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter 4 number [green]pincode[/]:")
                .PromptStyle("red")
                .Secret());
        }
        public static List<User> CheckLogin()
        {
           
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                Console.Write("Enter name: ");
                string firstName = Console.ReadLine();
                string pinCode = PassPrompt();

                var output = cnn.Query<User>($"SELECT * FROM bank_user WHERE first_name = '{firstName}' AND pin_code = '{pinCode}'", new DynamicParameters()).ToList();
                var outputTwo = cnn.Query<User>($"SELECT * FROM bank_user WHERE first_name = '{firstName}'", new DynamicParameters()).ToList(); // Sets user for wrong code increment
                if (outputTwo[0].login_attempt < 3) // If a user with matchning name exists, check so attempts <=3
                {
                    if (output.Any()) // If output returns anything, a match on both name and pin - continue
                    {
                        Console.WriteLine($"Log in successfull, welcome {output[0].first_name} {output[0].last_name}");
                        cnn.Query<User>($"UPDATE bank_user SET login_attempt = '0' WHERE first_name = '{firstName}' AND pin_code = '{pinCode}'", new DynamicParameters()); // Resets log in counter
                        return output; // Returns the list with user
                    }
                    else
                    {
                        cnn.Query<User>($"UPDATE bank_user SET login_attempt = login_attempt + '1' WHERE first_name = '{firstName}'", new DynamicParameters()); // Increments login counter for user
                        Console.WriteLine($"Login failed, incorrect name or pincode. Try again. \r\nFailed attempt(s): {outputTwo[0].login_attempt + 1}, account will be locked after 3 failed attempts."); //Information
                        // Sets output to 0
                        output.Clear();
                        return output;
                    }
                }
                else if (outputTwo[0].login_attempt == 3)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Your account is locked due to too many failed login attempts. Contact an administrator to unlock it."); // If user has failed too many times, will be locked until login_attempts maunally resets to 0
                    Console.WriteLine("---------------------------------------------");
                    // Sets output to 0
                    output.Clear();
                    return output;
                }
                else
                {
                    output.Clear();
                    return output;
                }
            }

            // Connects to the DB
            // Reads a user with a specific name and pincode
            // Returns a list if it gets a hit
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

                var output = cnn.Query<User>("SELECT * FROM bank_user  ORDER BY id", new DynamicParameters());
                return output.ToList();
            }
            // Connects to the DB
            // Reads all Users
            // Returns a list of Users
        }

        public static List<User> LoadBankAccounts()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<User>("SELECT * FROM bank_account ORDER BY id", new DynamicParameters());
                return output.ToList();
            }
        }

        // Lists all user accounts based of id
        public static List<Account> UserAccount(List<User> user)
        {

            // List<Account> activeAccounts = Database.UserAccount(activeUser[0].id);

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Account>($"SELECT * FROM bank_account WHERE user_id = '{user[0].id}' ORDER BY id", new DynamicParameters()).ToList();
                return output;
            }
        }
        // Returns the information of someone with a specific account number. 

        public static void SeeAccountsAndBalance(List<Account> accounts)
        {
            int counter = 1;
            string currency = "";
            var table = new Table();
            // Sets tables border
            table.Border(TableBorder.Ascii);
            // Adds columns
            table.AddColumn("|").Centered();
            table.AddColumn(new TableColumn("Account Name").Centered());
            table.AddColumn(new TableColumn("Account Balance").Centered());
            table.AddColumn(new TableColumn("Account Number").Centered());
            foreach (var item in accounts) // Lists accounts and balances with numbers
            {
                switch (item.currency_id)
                {
                    case 1:
                        currency = "SEK";
                        break;
                    case 2:
                        currency = "USD";
                        break;
                    case 3:
                        currency = "EUR";
                        break;
                    case 4:
                        currency = "GBP";
                        break;
                    default:
                        break;
                }
                // Adds one row to table per loop iteration
                table.AddRow($"{counter}", $"{item.name}", $"{item.balance} {currency}", $"{item.account_number}");
                counter++;
            }
            // Prints out the full table
            AnsiConsole.Write(table);
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }
        public static void ListUserAccounts(List<Account> accounts)
        {
            int counter = 1;
            string currency = "";
            var table = new Table();
            // sets tables border
            table.Border(TableBorder.Ascii);
            // Adds columns
            table.AddColumn("|").Centered();
            table.AddColumn(new TableColumn("Account Name").Centered());
            table.AddColumn(new TableColumn("Account Balance").Centered());
            table.AddColumn(new TableColumn("Account Number").Centered());
            foreach (var item in accounts) // Lists accounts and balances with numbers
            {
                switch (item.currency_id)
                {
                    case 1:
                        currency = "SEK";
                        break;
                    case 2:
                        currency = "USD";
                        break;
                    case 3:
                        currency = "EUR";
                        break;
                    case 4:
                        currency = "GBP";
                        break;
                    default:
                        break;
                }
                // Adds one row to table per loop iteration
                table.AddRow($"{counter}", $"{item.name}", $"{item.balance} {currency}", $"{item.account_number}");
                counter++;
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }

        // Transfers money between own accounts
        public static void Transfer(List<Account> activeAccounts)
        {
            Console.Write("Type the account you want to transfer from: "); //From
            int choiceFrom = int.Parse(Console.ReadLine()) - 1;
            // Checks if user input is a valid account
            if (choiceFrom + 1 <= 0)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Transfer(activeAccounts);
            }
            else if (choiceFrom + 1 > activeAccounts.Count)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Transfer(activeAccounts);
            }
            int idFrom = activeAccounts[choiceFrom].id;
            int idFromCurrency = activeAccounts[choiceFrom].currency_id;

            // Switch to put right currency in the API string 
            string from = "";
            switch (idFromCurrency)
            {
                case 1:
                    from = "SEK";
                    break;
                case 2:
                    from = "USD";
                    break;
                case 3:
                    from = "EUR";
                    break;
                case 4:
                    from = "GBP";
                    break;
                default:
                    break;
            }

            Console.WriteLine("---------------------------------------------");
            Console.Write("Type the account you want to transfer to: "); //To
            int choiceTo = int.Parse(Console.ReadLine()) - 1;

            // Checks if user input is a valid account
            if (choiceTo + 1 < 0)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error! Invalid account number");
                Transfer(activeAccounts);
            }
            else if (choiceTo + 1 > activeAccounts.Count)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error! Invalid account number");
                Transfer(activeAccounts);
            }
            int idTo = activeAccounts[choiceTo].id;
            int idToCurrency = activeAccounts[choiceTo].currency_id;

            // Switch to put right currency in the API string 
            string to = "";
            switch (idToCurrency)
            {
                case 1:
                    to = "SEK";
                    break;
                case 2:
                    to = "USD";
                    break;
                case 3:
                    to = "EUR";
                    break;
                case 4:
                    to = "GBP";
                    break;
                default:
                    break;
            }

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("How much money do you want to transfer? ");
            Console.Write($"{from}: ");
            string input = Console.ReadLine();
            // Checks if user input is a number or a letter, runs rest of code if its able to be parsed to decimal otherwise it gives an error
            if (decimal.TryParse(input, out decimal amount))
            {
                // Checks if the chosen account has enough money on it, if not gives error and starts the function from the start
                if (amount > activeAccounts[choiceFrom].balance)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Insufficient funds.");
                    Transfer(activeAccounts);
                }
                // Checks if user input is a negative number, if true gives error and starts the function from the start
                else if (amount <= 0)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Can't transfer negative amount.");
                    Transfer(activeAccounts);
                }
                else
                {
                    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString())) // db connection string
                    {

                        String URLString = $"https://v6.exchangerate-api.com/v6/32b26456dd41b6e1bc2befd1/pair/{from}/{to}/{amount}"; // API string to calculate value of one currency to another 
                        using (var webClient = new System.Net.WebClient())
                        {
                            var json = webClient.DownloadString(URLString);
                            API_Obj_Convert rate = JsonConvert.DeserializeObject<API_Obj_Convert>(json);
                            decimal transfer = Convert.ToDecimal($"{rate.conversion_result}"); // Converting string recieved from API to double since it gives asnwer with a comma when we need a dot.
                            var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{amount}' WHERE bank_account.id = '{idFrom}'; UPDATE bank_account SET balance = balance + {transfer} WHERE bank_account.id = '{idTo}'; INSERT INTO bank_transaction (name, user_id, from_account_id, to_account_id, amount) VALUES ('Transfer between own accounts', '{activeAccounts[0].user_id}', '{idFrom}', '{idTo}', '{amount}' )", new DynamicParameters());
                            Console.WriteLine();
                            Ascii.LoadingTransfer();


                            
                        }

                        if (activeAccounts[choiceTo].name == "Savings")
                        {
                            decimal interestRate = 0.02m;
                            decimal initialBalance = activeAccounts[choiceTo].balance;
                            //decimal currentbalance = amount;

                            decimal interest = 0m;
                            decimal yearOne = Math.Truncate(initialBalance + amount + ((initialBalance + amount) * interestRate));
                            decimal yearTwo = Math.Truncate(yearOne + yearOne * interestRate);
                            decimal yearThree = Math.Truncate(yearTwo + yearTwo * interestRate);


                            var table = new Table();
                            table.AddColumn("Balance").Centered();
                            table.AddColumn(new TableColumn("Interest").Centered());
                            table.AddColumn(new TableColumn("Year").Centered());

                            table.AddRow($"{yearOne}", "2%", "1");
                            table.AddRow($"{yearTwo}", "2%", "2");
                            table.AddRow($"{yearThree}", "2%", "3");

                            AnsiConsole.Write(table);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error: Input must be a number.");
                Transfer(activeAccounts);
            }
            Thread.Sleep(2500);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");

        }

        // Transfer funds, but to an account that is not logged in 
        public static void ExternalTransfer(List<Account> activeAccounts)
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write("Type the account you want to transfer from: "); // From
            int choiceFrom = int.Parse(Console.ReadLine()) - 1;

            // Checks if user input is a valid account
            if (choiceFrom + 1 <= 0)
            {
                Console.WriteLine("Invalid account number");
                RunProgram();
            }
            else if (choiceFrom + 1 > activeAccounts.Count)
            {
                Console.WriteLine("Invalid account number");
                RunProgram();
            }


            int idFrom = activeAccounts[choiceFrom].id;
            int idFromCurrency = activeAccounts[choiceFrom].currency_id;


            // Switch to put right currency in the API string 
            string from = "";
            switch (idFromCurrency)
            {
                case 1:
                    from = "SEK";
                    break;
                case 2:
                    from = "USD";
                    break;
                case 3:
                    from = "EUR";
                    break;
                case 4:
                    from = "GBP";
                    break;
                default:
                    break;
            }

            Console.WriteLine("---------------------------------------------");
            Console.Write("Enter another users account number, eg. 101: "); // Test to transfer to external user with known account_number - uses the same account as previous
            int reciever = int.Parse(Console.ReadLine());
            bool accountNumberCheck = false;
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<User>($"SELECT * FROM bank_account WHERE account_number = '{reciever}'", new DynamicParameters()).ToList();

                if (output.Count >= 1)
                {
                    Console.Write($"Enter amount: {from} "); // Test to transfer to external user with known account_number - uses the same account as previous
                    string input = Console.ReadLine();
                    // Checks if user input is a number or a letter, runs rest of code if its able to be parsed to decimal otherwise it gives an error
                    if (decimal.TryParse(input, out decimal amount))
                    {
                        // Checks if the chosen account has enough money on it, if not gives error and starts the function from the start
                        if (amount > activeAccounts[choiceFrom].balance)
                        {
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Error: Insufficient funds.");
                            ExternalTransfer(activeAccounts);
                        }
                        // Checks if user input is a negative number, if true gives error and starts the function from the start
                        else if (amount <= 0)
                        {
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("Error: Can't transfer negative amount.");
                            ExternalTransfer(activeAccounts);
                        }

                        List<Account> accountX = Database.ForeignAccount(reciever); // Finds accounts with specific account number
                        int idTo = accountX[0].account_number; // Sets reciever to the new account
                        Console.WriteLine($"Account {accountX[0].account_number}, {accountX[0].name}");
                        int idToCurrency = accountX[0].currency_id;


                        // Switch to put right currency in the API string 
                        string to = "";
                        switch (idToCurrency)
                        {
                            case 1:
                                to = "SEK";
                                break;
                            case 2:
                                to = "USD";
                                break;
                            case 3:
                                to = "EUR";
                                break;
                            case 4:
                                to = "GBP";
                                break;
                            default:
                                break;
                        }


                        String URLString = $"https://v6.exchangerate-api.com/v6/32b26456dd41b6e1bc2befd1/pair/{from}/{to}/{amount}"; // API string to calculate value of one currency to another 
                        using (var webClient = new System.Net.WebClient())
                        {
                            var json = webClient.DownloadString(URLString);
                            API_Obj_Convert rate = JsonConvert.DeserializeObject<API_Obj_Convert>(json);
                            decimal transfer = Convert.ToDecimal($"{rate.conversion_result}"); // Converting string recieved from API to double since it gives asnwer with a comma when we need a dot.
                            var output2 = cnn.Query<User>($"UPDATE bank_account SET balance = balance - '{amount}' WHERE bank_account.id = '{idFrom}'; UPDATE bank_account SET balance = balance + '{amount}' WHERE bank_account.account_number = '{idTo}'; INSERT INTO bank_transaction (name, user_id, from_account_id, to_account_id, amount) VALUES ('External transfer', '{activeAccounts[0].user_id}', '{idFrom}', '{accountX[0].id}', {amount})", new DynamicParameters());
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine($"Successfully transfered {amount} from {activeAccounts[choiceFrom].account_number} to {idTo}");
                        }

                    }
                    else
                    {
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Error: Input must be a number.");
                        ExternalTransfer(activeAccounts);
                    }
                }
                else
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Invalid account number.");
                    ExternalTransfer(activeAccounts);
                }

            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }

        public static void Withdraw(List<Account> activeAccounts)
        {
            Console.Write("Type the account you want to withdraw money from: ");
            int userChoice = int.Parse(Console.ReadLine()) - 1;

            // Checks if user input is a valid account
            if (userChoice + 1 <= 0)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Withdraw(activeAccounts);
            }
            else if (userChoice + 1 > activeAccounts.Count)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Withdraw(activeAccounts);
            }
            int id = activeAccounts[userChoice].id;
            int idCurrency = activeAccounts[userChoice].currency_id;

            // Switch to put right currency in the API string 
            string currency = "";
            switch (idCurrency)
            {
                case 1:
                    currency = "SEK";
                    break;
                case 2:
                    currency = "USD";
                    break;
                case 3:
                    currency = "EUR";
                    break;
                case 4:
                    currency = "GBP";
                    break;
                default:
                    break;
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("How much money do you want to withdraw?");
            Console.Write($"{currency}: ");
            string input = Console.ReadLine();
            // Checks if user input is a number or a letter, runs rest of code if its able to be parsed to decimal otherwise it gives an error
            if (decimal.TryParse(input, out decimal amount))
            {
                // Checks if the chosen account has enough money on it, if not gives error and starts the function from the start
                if (amount > activeAccounts[userChoice].balance)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Insufficient funds.");
                    Withdraw(activeAccounts);
                }
                // Checks if user input is a negative number, if true gives error and starts the function from the start
                else if (amount <= 0)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Can't transfer negative amount.");
                    Withdraw(activeAccounts);
                }
                else
                {
                    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString())) // db connection string
                    {
                        var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance - {amount} WHERE bank_account.id = '{id}'; INSERT INTO bank_transaction (name, user_id, from_account_id, amount) VALUES ('Withdrawal from {activeAccounts[userChoice].name}', '{activeAccounts[0].user_id}', '{id}', '{amount}')", new DynamicParameters());
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Money successfully withdrawn.");
                    }
                }
            }
            else
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error: Input must be a number.");
                Deposit(activeAccounts);
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }

        public static void Deposit(List<Account> activeAccounts)
        {
            Console.Write("Type the account you want to deposit money into: ");
            int userChoice = int.Parse(Console.ReadLine()) - 1;
            // Checks if user input is a valid account
            if (userChoice + 1 <= 0)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Deposit(activeAccounts);
            }
            else if (userChoice + 1 > activeAccounts.Count)
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Invalid account number");
                Deposit(activeAccounts);
            }
            int id = activeAccounts[userChoice].id;
            int idCurrency = activeAccounts[userChoice].currency_id;
            // Switch to put right currency in the API string 
            string currency = "";
            switch (idCurrency)
            {
                case 1:
                    currency = "SEK";
                    break;
                case 2:
                    currency = "USD";
                    break;
                case 3:
                    currency = "EUR";
                    break;
                case 4:
                    currency = "GBP";
                    break;
                default:
                    break;
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("How much money do you want to deposit?");
            Console.Write($"{currency}: ");
            string input = Console.ReadLine();
            // Checks if user input is a number or a letter, runs rest of code if its able to be parsed to decimal otherwise it gives an error
            if (decimal.TryParse(input, out decimal amount))
            {
                // Checks if user input is a negative number, if true gives error and starts the function from the start
                if (amount <= 0)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Can't transfer negative amount.");
                    Deposit(activeAccounts);
                }
                else
                {
                    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString())) // db connection string
                    {




                        if (activeAccounts[userChoice].name == "Savings")
                        {
                            decimal interestRate = 0.02m;
                            decimal initialBalance = activeAccounts[userChoice].balance;
                            //decimal currentbalance = amount;

                            decimal interest = 0m;
                            decimal yearOne = Math.Truncate(initialBalance + amount + ((initialBalance + amount) * interestRate));
                            decimal yearTwo = Math.Truncate(yearOne + yearOne * interestRate);
                            decimal yearThree = Math.Truncate(yearTwo + yearTwo * interestRate);


                            var table = new Table();
                            table.AddColumn("Balance").Centered();
                            table.AddColumn(new TableColumn("Interest").Centered());
                            table.AddColumn(new TableColumn("Year").Centered());

                            table.AddRow($"{yearOne}", "2%", "1");
                            table.AddRow($"{yearTwo}", "2%", "2");
                            table.AddRow($"{yearThree}", "2%", "3");

                            AnsiConsole.Write(table);
                        }
                        var output = cnn.Query<User>($"UPDATE bank_account SET balance = balance + {amount} WHERE bank_account.id = '{id}'; INSERT INTO bank_transaction (name, user_id, to_account_id, amount) VALUES ('Deposit to {activeAccounts[userChoice].name}', '{activeAccounts[0].user_id}', '{id}', '{amount}')", new DynamicParameters());
                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine("Money successfully deposited.");
                    }
                }
            }
            else
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error: Input must be a number.");
                Deposit(activeAccounts);
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
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

        //Checks if user is allowed to take a loan based on total savings in bank and returns a bool

        //public static bool SetLoanPermission(List<Account> activeAccount, decimal loanAmount)
        //{

        //    bool loanPermission;
        //    loanAmount *= 5;

        //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //    {
        //        var output = Convert.ToDecimal(cnn.Query<Account>($"SELECT SUM(balance) FROM bank_account", new DynamicParameters()));

        //        if (output < loanAmount + 1)
        //        {
        //            loanPermission = false;
        //        }
        //        else
        //        {
        //            loanPermission = true;
        //        }
        //        return loanPermission;
        //    }

        //}

        //Makes the actual loan transfer of requested amount to requested account

        //public static void Loan(List<Account> activeAccount)
        //{
        //    Console.WriteLine("---------------------------------------------");
        //    Console.WriteLine();
        //    Console.WriteLine("The allowed amount for a loan is five times the amount of your total savings in this bank");
        //    Console.WriteLine();
        //    Console.WriteLine("How much money would you like to loan?");
        //    decimal loanAmount = decimal.Parse(Console.ReadLine());
        //    bool loanPermission = SetLoanPermission(activeAccount, loanAmount);
        //    if (loanPermission)
        //    {
        //        decimal interest = 0.0570 * loanAmount;
        //        Console.WriteLine($"The interest rate for the requested loan is {interest}");
        //        ListUserAccounts(activeAccount);  
        //        Console.WriteLine("Please enter preferred receiver account");
        //        int receiverAccount = int.Parse(Console.ReadLine());   
        //        using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //        {
        //                var output = cnn.Query<Account>($"UPDATE bank_account SET balance = balance + '{loanAmount}' " +
        //                    $"WHERE bank_account.id = '{receiverAccount}'", new DynamicParameters());
        //        }
        //        Console.WriteLine($"You have successfully loaned {loanAmount}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unfortunately you are not eligible for a loan in this bank, since the total amount of " +
        //            "your savings fail to reach the required threshold");
        //    }
        //}


        //Create account
        public static void CreateAccount(List<User> users)
        {
            var account = new Account();
            var accountOption = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                .Title("What type of account do you want to open?")
                .PageSize(5)
                .MoreChoicesText("[grey](Use arrow keys to move up and down)[/]")
                .AddChoices(new[] {
                    "Checking Account", "Salary", "Certificate of Deposit Account",
                    "Money Market Account", "Savings Account",
                }));
            switch (accountOption)
            {
                case "Checking Account":
                    account.name = "Checking Account";
                    account.interest_rate = 0;
                    break;
                case "Salary":
                    account.name = "Salary";
                    account.interest_rate = 0;
                    break;
                case "Certificate of Deposit Account":
                    account.name = "CD";
                    account.interest_rate = 0;
                    break;
                case "Money Market Account":
                    account.name = "MMA";
                    account.interest_rate = 0;
                    break;
                case "Savings Account":
                    account.name = "Savings";
                    account.interest_rate = 2.5m;
                    break;
                default:
                    Console.WriteLine("Error");
                    CreateAccount(users);
                    break;

            }
            account.user_id = users[0].id;
            
            var currencyOption = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                .Title("What currency do you want the account to be in?")
                .PageSize(4)
                .MoreChoicesText("[grey](Use arrow keys to move up and down)[/]")
                .AddChoices(new[] {
                    "SEK", "USD", "EUR",
                    "GBP",
                }));
            switch (currencyOption)
            {
                case "SEK":
                    account.currency_id = 1;
                    break;
                case "USD":
                    account.currency_id = 2;
                    break;
                case "EUR":
                    account.currency_id = 3;
                    break;
                case "GBP":
                    account.currency_id = 4;
                    break;
                default:
                    Console.WriteLine("Error");
                    CreateAccount(users);
                    break;

            }
           
            account.balance = 0;
            Random rnd = new Random();
            int account_number = rnd.Next(10000);
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Account>("SELECT account_number FROM bank_account", new DynamicParameters()).ToList();
                
                foreach (var item in output)
                {
                    if (item.account_number == account_number)
                    {
                        account_number += 1;
                    }
                }
            }
            
            
            account.account_number = account_number;


            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into bank_account (name, interest_rate, user_id, currency_id, balance, account_number) values (@name, @interest_rate, @user_id, @currency_id, @balance, @account_number)", account);
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Account successfully created.");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }
        //Create user function
        public static void CreateUser()
        {
            Console.WriteLine("---------------------------------------------");
            //creating a new user object
            var user = new User();

            //sets user input to the new users first name
            Console.WriteLine("What is the first name of the account holder?");
            user.first_name = Console.ReadLine();


            //sets user input to the new users last name
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("What is the last name of the account holder?");
            user.last_name = Console.ReadLine();


            //sets user input to new users pincode
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Enter desired 4 number pincode: ");
            user.pin_code = Console.ReadLine();
            //Checks if the entered pin is 4 chars long.
            if (int.TryParse(user.pin_code, out int output))
            {
                if (user.pin_code.Length > 4 | user.pin_code.Length < 4)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Error: Must be 4 number pincode");
                    CreateUser();
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Error: Can not contain letters, re-enter the creation");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine();
                CreateUser();
            }
            var accountTypeOption = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                .Title("What type of account is it?")
                .PageSize(3)
                .MoreChoicesText("[grey](Use arrow keys to move up and down)[/]")
                .AddChoices(new[] {
                    "Administrator", "ClientAdmin", "Client",
                }));
            switch (accountTypeOption)
            {
                case "Administrator":
                    user.role_id = 1;
                    break;
                case "ClientAdmin":
                    user.role_id = 3;
                    break;
                case "Client":
                    user.role_id = 2;
                    break;
                default:
                    Console.WriteLine("Error");
                    CreateUser();
                    break;

            }
           
            user.login_attempt = 0;
            // Adds new user object in to database 
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into bank_user (first_name, last_name, pin_code, role_id, login_attempt) values (@first_name, @last_name, @pin_code, @role_id, @login_attempt)", user);
            }


            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("User Account successfully created.");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }
        public static void UnlockUser(List<User> users)
        {
            List<User> bankUsers = Database.LoadBankUsers(); // Creates list of all users so I can print them all out

            // Create a table
            var table = new Table();
            // Sets table border
            table.Border(TableBorder.Ascii);
            // Add some columns
            table.AddColumn("First Name");
            table.AddColumn(new TableColumn("Last Name"));
            foreach (User user in bankUsers)
            {
                // Adds one row to table per loop iteration 
                table.AddRow($"{user.first_name}", $"{user.last_name}");
            }
            // Prints table 
            AnsiConsole.Write(table);
            
            Console.WriteLine("What user do you want to unlock? Enter first name: ");
            string userChoice = Console.ReadLine();

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Query<User>($"UPDATE bank_user SET login_attempt = '0' WHERE first_name = '{userChoice}'", new DynamicParameters()); // Resets login counter of selected user
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"User {userChoice}'s account has been unlocked.");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("→ Press enter to return to main menu...");
        }

       
        public static List<Transaction> GetTransactionByUser(List<Account> accounts)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<Transaction>($"SELECT * FROM bank_transaction where user_id='{accounts[0].user_id}'", new DynamicParameters());
                
                // Create a new table
                var table = new Table();

                // Add some columns
                table.AddColumn("Account Name").Centered();
                table.AddColumn(new TableColumn("Sending Account ID").Centered());
                table.AddColumn(new TableColumn("Receiving Account ID").Centered());
                table.AddColumn(new TableColumn("Amount").Centered());
                table.AddColumn(new TableColumn("Date").Centered());
                foreach (var item in output)
                {
                    // Adds one row to table per loop iteration
                    table.AddRow($"{item.name}", $"{item.from_account_id}", $"{item.to_account_id}", $"{item.amount}", $"{item.transaction_time}");
                }

                // Render the table to the console
                AnsiConsole.Write(table);

                
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("→ Press enter to return to main menu...");

                return output.ToList();

              
            }

        }

       

    }
}