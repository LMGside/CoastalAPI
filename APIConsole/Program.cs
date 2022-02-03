using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIClient;
using CoastalAPIDataLayer;
using CoastalAPIModels.Models;

namespace APIConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            CoastalClient cc = new CoastalClient("Mfundo", "Hunger", "https://localhost:44308/");
            bool options = true;

            Console.WriteLine("Welcome to the Coastal Finance API Demo");
            Console.WriteLine("Press Enter to Proceed");

            while (options)
            {
                Console.WriteLine("1. Register a new Customer");
                Console.WriteLine("2. Freeze Customer's Account");
                Console.WriteLine("3. Unfreeze Customer's Account");
                Console.WriteLine("4. Deregister a Customer");
                Console.WriteLine("5. Deposit to a Customer's Wallet");
                Console.WriteLine("6. Withdraw from a Customer's Wallet");
                Console.WriteLine("7. View Transactions made Today");
                Console.WriteLine("8. View Transactions made by User");
                Console.WriteLine("9. View Transactions made between 2 Dates");
                Console.WriteLine("10. Buy an Asset");
                Console.WriteLine("11. Approve an Asset");
                //Add Assets if theres time

                int selected = Convert.ToInt32(Console.ReadLine());

                switch (selected)
                {
                    case 1:
                        Console.WriteLine("Type the Customer's Details");
                        Console.Write("Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Surname: ");
                        string surname = Console.ReadLine();

                        Console.Write("Date of Birth => yyyy-mm-dd: ");
                        DateTime dob = Convert.ToDateTime(Console.ReadLine());

                        Console.Write("Address: ");
                        string address = Console.ReadLine();

                        Console.Write("ID Number: ");
                        string id = Console.ReadLine();

                        Console.Write("Contact: ");
                        string tele = Console.ReadLine();

                        RegisterRequest rr = new RegisterRequest();
                        rr.Name = name;
                        rr.Surname = surname;
                        rr.DOB = dob;
                        rr.Address = address;
                        rr.Identity_No = id;
                        rr.Contact = tele;

                        cc.InsertCustomer(rr);
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    case 5:
                        break;

                    case 6:
                        break;

                    case 7:
                        break;

                    case 8:
                        break;

                    case 9:
                        break;

                    case 10:
                        break;

                    case 11:
                        break;
                }

                Console.WriteLine("1. Continue");
                Console.WriteLine("0. Exit");
                int endOption = Convert.ToInt32(Console.ReadLine());

                if(endOption == 0)
                {
                    options = false;
                }

            }
        }
    }
}
