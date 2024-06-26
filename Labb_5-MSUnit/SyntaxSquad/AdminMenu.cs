﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    //Noah SUT23
    public class AdminMenu : Menu
    {
        ExchangeRateManager manager = new ExchangeRateManager();

        public override void ShowMenu(User user)
        {

            ACIIART Art = new ACIIART();
            Console.ForegroundColor = ConsoleColor.Yellow;
            bool validChoice = true;
            do
            {
                Console.Clear();
                Art.PrintArt();
                Console.WriteLine("\t---|| Admin Menu ||---");
                Console.WriteLine("\t1: Add User \n\t2: Currency Value \n\t3: Show Users \n\t4: Remove User \n\t5: Transfer history \n\t6: Loan history \n\t7: Print new accounts" +
                    "\n\t8: Logout");


                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        AdminFunctions.AddUser(user);
                        break;
                    case "2":
                        manager.ChangeExchangeRates();
                        break;
                    case "3":
                        AdminFunctions.ShowCurrentUsers();
                        break;
                    case "4":
                        AdminFunctions.RemoveUser();
                        break;
                    case "5":
                        Transfer.PrintTransferHistoryAdmin();
                        break;
                    case "6":
                        //Loan.SeeAllLoans();
                        break;
                    case "7":
                        CreateAccount.AdminPrintNewAccounts();
                        break;
                    case "8":
                        user.IsLoggedIn = false;
                        validChoice = false;
                        break;
                    default:
                        break;

                }

            } while (validChoice);


        }
    }
}
