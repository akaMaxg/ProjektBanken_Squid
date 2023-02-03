using Internal;

namespace ProjektBankenSquid2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Squid-bank");
            List<User> users = Database.LoadBankUsers();

            foreach (User user in users)
            {
                Console.WriteLine($"Hello {user.first_name} your last name is {user.pin_code}");
            }

            Console.Write("Enter name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter pincode: ");
            string pinCode = Console.ReadLine();

            List<User> activeUser = Database.CheckLogin(firstName, pinCode);
            Console.WriteLine($"Log in successfull, welcome {activeUser[0].first_name} {activeUser[0].last_name}");

            List<Account> accounts = Database.UserAccount(activeUser[0].id);

            int counter = 1;
            foreach (var item in accounts) //Lists accounts and balances with numbers
            {
                Console.WriteLine($"{counter}. {item.name}, {item.balance}");
                counter++;
            }

            //Function to transfer balance internally
            Console.Write("Type the account you want to transfer from: "); //From
            int choiceFrom = int.Parse(Console.ReadLine());
            int idOne = 0;
            int idTwo = 0;

            switch (choiceFrom)
            {
                case 1:
                    idOne = accounts[0].id;
                    break;
                case 2:
                    idOne = accounts[1].id;
                    break;
                case 3:
                    idOne = accounts[2].id;
                    break;
                default:
                    break;
            }
            Console.Write("Type the account you want to transfer to: "); //To
            int choiceTo = int.Parse(Console.ReadLine());
            switch (choiceTo)
            {
                case 1:
                    idTwo = accounts[0].id;
                    break;
                case 2:
                    idTwo = accounts[1].id;
                    break;
                case 3:
                    idOne = accounts[2].id;
                    break;
                default:
                    break;
            }

            Console.WriteLine("How much money do you want to transfer? ");
            int amount = int.Parse(Console.ReadLine());
            Database.Transfer(idOne, idTwo, amount); //Runs transfer
            Console.WriteLine("Successful transfer");


            Console.Write("Enter another users account number, eg. 101"); //Test to transfer to external user with known account_number - uses the same account as previous
            int reciever = int.Parse(Console.ReadLine());


            List<Account> accountX = Database.ForeignAccount(reciever); //Finds accounts with specific account number
            idTwo = accountX[0].account_number; //Sets reciever to the new account
            Console.WriteLine($"Account {accountX[0].account_number}, {accountX[0].name}");

            Database.ExternalTransfer(idOne, idTwo, amount); //Transfers
            Console.WriteLine($"Successfully transfered {amount} from {idOne} to {idTwo}");

            //User newUser = new User("Frank", "Sinatra", "1111");
            //database.SaveBankUser(newUser);
            //Console.WriteLine("User added");
            //Functions.LogIn();
            //Functions.LogIn();

            //Loan funstionality

            Console.WriteLine("How much money would you like to loan?");
            decimal loanAmount = decimal.Parse(Console.ReadLine());
            decimal interestRate = 0,0570;   
            decimal interest = interestRate * loanAmount;
            Console.ReadLine($"The interest rate for this loan is {interest}");
            Console.WriteLine("Please enter receiver account");
            int receiverAccount = int.Parse(Console.ReadLine());
            //calls the list accounts function on row 29 above to list accounts and
            //let user pic one, this will be receiverAccount sent to Loan function
            Database.Loan(receiverAccount, loanAmount);
            Console.WriteLine($"You have successfully loaned {loanAmount}");



        }
    }
}