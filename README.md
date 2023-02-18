#Förklaring av strukturen

##RunProgram
>Check what your role is. Sends you to the menu you have access to

##CheckLogin
>Menu for login function. The program ask the user to enter username and pincode. If the you enter correct username and pincode, he get to the menu. You have only three attempts to enter correct username och pincode. After three attempt you will be locked.

##SaveBankUser
>Create a new user to the bank. You have to select firstname, lastname and pincode. Save in the database and you get a message

##LoadBankUsers
>Return a list of all users from database

##LoadBankAccounts
>Return a list of all account from database

##UserAccount
>Show list with all account for the user who is logged in

##SeeAccountsAndBalance
>See your accounts and balances

##Transfer
>Transfer money betwenn own accounts

##ExternalTransfer
>Transfer money to another user

##Withdraw
>The user chooses which account he wants to withdraw money from. Updated in database and you receives a massage

##Deposit
>The user chooses which account he wants to deposit money to. Updated in database and you receives a massage

##CreateAccount
>The user get a list with different types of account. You get 2 precent in interest rate if you choose savings account

##CreateUser
>Create a new user. You have to select firstname, lastname and pincode and choose a type of role. Saving in database and you receives a massage 

##UnlockUser
>Unlock the user who is locked after three attempts

##GetTransactionByUser
>List for transaction history for transfer, deposit and witdraw
