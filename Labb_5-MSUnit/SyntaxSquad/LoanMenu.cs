﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    //Noah/Max SUT23
    public class LoanMenu : UserMenu
    {
        private bool run = true;
        public override void ShowMenu(User user)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ACIIART Art = new ACIIART();
            run = true;
            do
            {
                Console.Clear();
                Art.PrintArt();
                Console.WriteLine("\t---|| Loan Menu ||---");
                Console.WriteLine("\t1: Take loan \n\t2: See Loans \n\t3: Return to Main menu");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        //Loan.TakeOutLoan(user);
                        break;
                    case "2":
                        //Loan.SeeUserLoans(user);
                        break;
                    case "3":
                        run = false;
                        break;
                    default:
                        break;
                }
            } while (run);
        }
    }
}
