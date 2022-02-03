using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIClient;
using CoastalAPIDataLayer;
using CoastalAPIDataLayer.Models;
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
            Console.WriteLine("");

            while (options)
            {
                Console.WriteLine("1. Register a new Customer"); //
                Console.WriteLine("2. Freeze Customer's Account"); //
                Console.WriteLine("3. Unfreeze Customer's Account"); //
                Console.WriteLine("4. Deregister a Customer");
                Console.WriteLine("5. Deposit to a Customer's Wallet");
                Console.WriteLine("6. Withdraw from a Customer's Wallet");
                Console.WriteLine("7. View Transactions made on a Date"); //
                Console.WriteLine("8. View Transactions made by User"); //
                Console.WriteLine("9. View Transactions made between 2 Dates"); //
                Console.WriteLine("10. Buy an Asset");
                Console.WriteLine("11. Approve/Reject an Asset");
                Console.WriteLine("12. View Successful Transactions");
                Console.WriteLine("13. View Unsuccessful Transactions");
                Console.WriteLine("");
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
                        Console.WriteLine("");
                        Console.WriteLine(cc.InsertCustomer(rr).Status.ToString());
                        break;

                    case 2:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum = Console.ReadLine();

                        FreezeCustomerRequest fcr = new FreezeCustomerRequest();
                        fcr.Identity_No = idNum;

                        Console.WriteLine("");
                        Console.WriteLine(cc.FreezeCustomer(fcr));
                        break;

                    case 3:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum2 = Console.ReadLine();

                        UnfreezeCustomerRequest ufcr = new UnfreezeCustomerRequest();
                        ufcr.Identity_No = idNum2;

                        Console.WriteLine("");
                        Console.WriteLine(cc.UnfreezeCustomer(ufcr));
                        break;

                    case 4:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum3 = Console.ReadLine();

                        DeregisterCustomerRequest dcr = new DeregisterCustomerRequest();
                        dcr.Identity_No = idNum3;

                        Console.WriteLine("");
                        Console.WriteLine(cc.DeregisterCustomer(dcr));
                        break;

                    case 5:
                        break;

                    case 6:
                        break;

                    case 7:
                        Console.Write("Enter Date being searched for yyyy-mm-dd: ");
                        DateTime day = Convert.ToDateTime(Console.ReadLine());

                        DayTransactionRequest dtr = new DayTransactionRequest();
                        dtr.Day = day;

                        Console.WriteLine("");
                        List<Transaction> list1 = cc.DayTransactions(dtr).Transactions;

                        if (list1.Count > 0)
                        {
                            foreach (Transaction t in list1)
                            {
                                Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Buyer + ", Seller: " + t.Seller + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }

                        break;

                    case 8:
                        Console.Write("Enter User's ID being searched for: ");
                        Console.Write("ID Number: ");
                        string idNum4 = Console.ReadLine();

                        UserTransactionRequest utr = new UserTransactionRequest();
                        utr.ID_No = idNum4;

                        Console.WriteLine("");
                        List<Transaction> list2 = cc.UserTransactions(utr).Transactions;

                        if (list2.Count > 0)
                        {
                            foreach (Transaction t in list2)
                            {
                                Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Buyer + ", Seller: " + t.Seller + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }

                        break;

                    case 9:
                        Console.Write("Enter Start Date being searched for yyyy-mm-dd: ");
                        DateTime startD = Convert.ToDateTime(Console.ReadLine());

                        Console.Write("Enter End Date being searched for yyyy-mm-dd: ");
                        DateTime endD = Convert.ToDateTime(Console.ReadLine());

                        DateRangeTransactionsRequest drtr = new DateRangeTransactionsRequest();
                        drtr.StartDate = startD;
                        drtr.EndDate = endD;

                        Console.WriteLine("");
                        List<Transaction> list3 = cc.DateRangeTransactions(drtr).Transactions;

                        if (list3.Count > 0)
                        {
                            foreach (Transaction t in list3)
                            {
                                Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Buyer + ", Seller: " + t.Seller + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }

                        break;

                    case 10:
                        break;

                    case 11:
                        break;
                }

                Console.WriteLine("");

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
