# Team Squid - Bank project 02/2023

## Introduction 
This console application was created by *Max Guclu*, *Arvid Ljungberg* and *Mostafa Shubber*, also known as **Team Squid**. The project was conducted as part of a *C#* and *Agile Methods* course in a *Fullstack .NET* program at **Chas Academy**.   

During the project we applied **Agile- and Scrum** methodology and tracked- and version-managed our project with **Trello** and **Git / Github**. 
-> *Trello link*: https://trello.com/b/JsX3HvSb/banken . See *Admininstrative items* column for OOAD.
-> *Github link*: https://github.com/akaMaxg/ProjektBanken_Squid

## Application structure
This console application was developed in **C#** and utilizes a **PostgreSQL** database along with **Dapper**. The purpose of this application is to demonstrate basic *CRUD* (Create, Read, Update, Delete) operations on a database.  The key for the database connection is located in a git-ignored app file.

The program revolves around an object *User* with a list of properties derived from the database. The application also contains a number of classes that interacts with *User* in various ways in accordance with user stories and requirements set on the application. 
### List of classes their methods & Short descriptions
-> *Menu*
This class contains various methods with menus that are called depending on what user logs in. Furthermore the options in the menu allows the user to access different functions performable by the bank. For example: *AdminMenu* and *ClientAdminMenu*. They are operated by switch-cases that allows the user to navigate to desired choice and select with enter. 

-> *Database* 
This class contains most methods that interacts with the database and some general method. It hosts the connection string that allows access to the database. Some methods are *Check Login* that prompts the user to input a name and a pincode and runs a SQL query and returns a list with [1] item if there is a match in the database. The list is used throughout the application to access the user's properties such as role and id. Another example of the method is *Load bank accounts* which saves a list of the specific user's accounts, the method *See Accounts and Balance* prints them for the user. There are more methods that operate in similar fashion that interacts with the list of User objects and accesses its properties - but most relevant one is transfer which promps the user to select to- and from account then balance. The method interacts with the database and removes- and adds the funds to the respective accounts, assuming there are sufficient funds.

-> *Transaction*
This is an object that allows transaction entries with properties to be utilized throughout the application.
 
-> *Rates*
This is a class that hosts objects related to the various currencies that the application supports. In addition it also connects to the database and updates exchange rates.

-> *User*
This is a class that hosts the User object with properties derived from the *bank_user* table.

-> *Ascii*
This is a class that hosts objects and methods from the Spectre framework/extension. It allows for cosmetic and practical improvements to the code. Such as loading screens, menu selections, tables.

-> *Account*
This class hosts the object Accounts along with the properties that are deried from the accounts database table. 
### Additional functionality
-> Hidden pincodes  
-> Exception handlnig  
-> Tables 
-> Loading bars  
-> Locking out users

## To run program
1.  Download or clone the project files from the Git repository.
2.  Open the solution or project in an C# IDE.
3.  Build the project by pressing F6 or navigating to Build > Build Solution.
Ensure that the key for the connection string is added to the app.config

## Usage
The following users with various roles can be used to try the program
Abbe - 4763
Max - 7777
????
Then follow the in-application prompts.

## Contributing
 If you would like to contribute to this project, please feel free to submit a pull request on the Git repository.

