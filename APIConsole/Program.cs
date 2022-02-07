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
    public class Program
    {

        public bool ID_Valid(string id, DateTime dob)
        {
            bool valid = false;
            string year = dob.ToString("yy");
            string month = dob.ToString("MM");
            string day = dob.ToString("dd");
            string startSix = year+month+day;

            string idSix = id.Substring(0, 6);

            if(idSix.CompareTo(startSix) == 0 && id.Length == 13)
            {
                valid = true;
            }

            return valid;
        }
        public static void Main(string[] args)
        {
            CoastalClient cc = new CoastalClient("Mfundo", "Hunger", "https://localhost:44308/");
            bool options = true;

            Console.WriteLine("Welcome to the Coastal Finance API Demo");
            Console.WriteLine("");

            while (options)
            {
                Console.WriteLine("1. Register a new Customer");
                Console.WriteLine("2. Freeze Customer's Account");
                Console.WriteLine("3. Unfreeze Customer's Account");
                Console.WriteLine("4. Deregister a Customer");
                Console.WriteLine("5. Deposit to a Customer's Wallet");
                Console.WriteLine("6. Withdraw from a Customer's Wallet");
                Console.WriteLine("7. View Transactions made on a Date");
                Console.WriteLine("8. View Transactions made by a User");
                Console.WriteLine("9. View Transactions made between two Dates");
                Console.WriteLine("10. Buy an Asset");
                Console.WriteLine("11. Approve/Reject an Asset");
                Console.WriteLine("12. View Successful Transactions");
                Console.WriteLine("13. View Unsuccessful Transactions");
                Console.WriteLine("14. Add Art Asset");
                Console.WriteLine("15. Add Car Asset");
                Console.WriteLine("16. Add Property Asset");
                Console.WriteLine("17. Deregister Asset");
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

                        if(new Program().ID_Valid(id, dob))
                        {
                            Console.WriteLine("");
                            Console.WriteLine(cc.InsertCustomer(rr).Message);
                        }
                        else
                        {
                            Console.WriteLine("ID Number Invalid");
                        }

                        break;

                    case 2:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum = Console.ReadLine();

                        FreezeCustomerRequest fcr = new FreezeCustomerRequest();
                        fcr.Identity_No = idNum;

                        Console.WriteLine("");
                        Console.WriteLine(cc.FreezeCustomer(fcr).Message);
                        break;

                    case 3:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum2 = Console.ReadLine();

                        UnfreezeCustomerRequest ufcr = new UnfreezeCustomerRequest();
                        ufcr.Identity_No = idNum2;

                        Console.WriteLine("");
                        Console.WriteLine(cc.UnfreezeCustomer(ufcr).Message);
                        break;

                    case 4:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum3 = Console.ReadLine();

                        DeregisterCustomerRequest dcr = new DeregisterCustomerRequest();
                        dcr.Identity_No = idNum3;

                        Console.WriteLine("");
                        Console.WriteLine(cc.DeregisterCustomer(dcr).Message);
                        break;

                    case 5:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum5 = Console.ReadLine();

                        Console.Write("Deposit Amount: ");
                        decimal amount = Convert.ToDecimal(Console.ReadLine());

                        DepositFundsRequest dfr = new DepositFundsRequest();
                        dfr.FundAmount = amount;
                        dfr.Id_No = idNum5;

                        Console.WriteLine("");
                        Console.WriteLine(cc.DepositFunds(dfr).Message);
                        break;

                    case 6:
                        Console.WriteLine("Enter Customer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum6 = Console.ReadLine();

                        Console.Write("Amount Withdrawing: ");
                        decimal amount2 = Convert.ToDecimal(Console.ReadLine());

                        WithdrawRequest wr = new WithdrawRequest();
                        wr.FundAmount = amount2;
                        wr.Id_No = idNum6;

                        Console.WriteLine("");
                        Console.WriteLine(cc.WithdrawFunds(wr).Message);
                        break;

                    case 7:
                        Console.Write("Enter Date being searched for yyyy-mm-dd: ");
                        DateTime day = Convert.ToDateTime(Console.ReadLine());

                        DayTransactionRequest dtr = new DayTransactionRequest();
                        dtr.Day = day;

                        Console.WriteLine("");
                        DayTransactionResponse dtr2 = cc.DayTransactions(dtr);

                        if (dtr2.Status != CoastalAPIModels.ResponseStatus.Error)
                        {
                            List<Transaction> list1 = dtr2.Transactions;

                            if (list1.Count > 0)
                            {
                                foreach (Transaction t in list1)
                                {
                                    Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Name1 + " "+ t.Surname1 + ", Seller: " + t.Name2 + " " + t.Surname2 + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved + ", Approved By: " + t.Who_Approved);
                                }
                            }
                            else
                            {
                                Console.WriteLine("None");
                            }
                        }
                        else
                        {
                            Console.WriteLine(dtr2.Error.ExceptionMessage);
                        }

                        break;

                    case 8:
                        Console.Write("Enter User's ID being searched for: ");
                        Console.Write("ID Number: ");
                        string idNum4 = Console.ReadLine();

                        UserTransactionRequest utr = new UserTransactionRequest();
                        utr.ID_No = idNum4;

                        Console.WriteLine("");
                        UserTransactionResponse utResponse = cc.UserTransactions(utr);

                        if(utResponse.Status != CoastalAPIModels.ResponseStatus.Error)
                        {
                            List<Transaction> list2 = utResponse.Transactions;

                            if (list2.Count > 0)
                            {
                                foreach (Transaction t in list2)
                                {
                                    Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Name1 + " " + t.Surname1 + ", Seller: " + t.Name2 + " " + t.Surname2 + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved + ", Approved By: " + t.Who_Approved);
                                }
                            }
                            else
                            {
                                Console.WriteLine("None");
                            }
                        }
                        else
                        {
                            Console.WriteLine(utResponse.Error.ExceptionMessage);
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
                        DateRangeTransactionsResponse dateRangeTransactionsResponse = cc.DateRangeTransactions(drtr);

                        if(dateRangeTransactionsResponse.Status != CoastalAPIModels.ResponseStatus.Error)
                        {
                            List<Transaction> list3 = dateRangeTransactionsResponse.Transactions;

                            if (list3.Count > 0)
                            {
                                foreach (Transaction t in list3)
                                {
                                    Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Name1 + " " + t.Surname1 + ", Seller: " + t.Name2 + " " + t.Surname2 + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved + ", Approved By: " + t.Who_Approved);
                                }
                            }
                            else
                            {
                                Console.WriteLine("None");
                            }
                        }
                        else
                        {
                            Console.WriteLine(dateRangeTransactionsResponse.Error.ExceptionMessage);
                        }

                        break;

                    case 10:
                        Console.WriteLine("Enter Buyer's ID Number");
                        Console.Write("ID Number: ");
                        string idNum7 = Console.ReadLine();

                        Console.Write("Enter Asset ID: ");
                        int assetID = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Purchase Amount: ");
                        decimal amount3 = Convert.ToDecimal(Console.ReadLine());

                        BuyAssetRequest buyer = new BuyAssetRequest();
                        buyer.Buyer_ID = idNum7;
                        buyer.Purchase_Price = amount3;
                        buyer.Asset_ID = assetID;

                        Console.WriteLine("");
                        BuyAssetResponse bars = cc.BuyAssets(buyer);
                        Console.WriteLine(bars.Message);
                        break;

                    case 11:
                        Console.Write("Enter Transaction ID:");
                        int transID = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("1. Approve");
                        Console.WriteLine("2. Reject");
                        int dec = Convert.ToInt32(Console.ReadLine());
                        ReviewTransactionRequest.TransactionStatus status;

                        if (dec == 1)
                        {
                            status = ReviewTransactionRequest.TransactionStatus.Approved;
                        }
                        else
                        {
                            status = ReviewTransactionRequest.TransactionStatus.Rejected;
                        }

                        ReviewTransactionRequest review = new ReviewTransactionRequest();
                        review.Decision = status;
                        review.TransactionID = transID;

                        Console.WriteLine("");
                        Console.WriteLine(cc.ReviewTransaction(review).Message);
                        break;

                    case 12:
                        Console.WriteLine("Successful Transactions");
                        UserTransactionRequest utr2 = new UserTransactionRequest();

                        Console.WriteLine("Loading...");
                        List<Transaction> listS = cc.SuccessfulTransactions(utr2).Transactions;

                        if (listS.Count > 0)
                        {
                            foreach (Transaction t in listS)
                            {
                                Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Name1 + " " + t.Surname1 + ", Seller: " + t.Name2 + " " + t.Surname2 + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved + ", Approved By: " + t.Who_Approved);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }

                        break;

                    case 13:
                        Console.WriteLine("Unsuccessful Transactions");
                        UserTransactionRequest utr3 = new UserTransactionRequest();

                        Console.WriteLine("Loading...");
                        List<Transaction> listU = cc.UnsuccessfulTransactions(utr3).Transactions;

                        if (listU.Count > 0)
                        {
                            foreach (Transaction t in listU)
                            {
                                Console.WriteLine("ID: " + t.ID + ", Buyer: " + t.Name1 + " " + t.Surname1 + ", Seller: " + t.Name2 + " " + t.Surname2 + ", Asset ID: " + t.Asset + ", Status: " + t.Status.ToString() + ", Date Requested: " + t.Date_Transaction_Requested + ", Date Approved: " + t.Date_Transaction_Approved + ", Approved By: " + t.Who_Approved);
                            }
                        }
                        else
                        {
                            Console.WriteLine("None");
                        }
                        break;
                    case 14:
                        Console.WriteLine("Type the new Art Details");
                        bool art = true;
                        int? autoVal = null;

                        Console.Write("Artist: ");
                        string artist = Console.ReadLine();

                        Console.Write("Art Title: ");
                        string title = Console.ReadLine();

                        Console.Write("Year Completed: ");
                        int artYear = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Auto Sale: 1. True  2.False");
                        int autoSale = Convert.ToInt32(Console.ReadLine());

                        if(autoSale == 1)
                        {
                            art = false;
                            Console.Write("Auto Valuation Amount: ");
                            autoVal = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.Write("Normal Valuation: ");
                        int normal = Convert.ToInt32(Console.ReadLine());

                        AddArtRequest addArt = new AddArtRequest();
                        addArt.Artist = artist;
                        addArt.Art_Title = title;
                        addArt.Art_Year = artYear;
                        addArt.Type = Asset.AssetType.Art;
                        addArt.Auto_Sale = art;
                        addArt.Auto_Valuation = autoVal;
                        addArt.Normal_Valuation = normal;
                        addArt.Owner = 1;

                        Console.WriteLine("");
                        Console.WriteLine(cc.AddArt(addArt).Message);
                      
                        break;
                    case 15:
                        Console.WriteLine("Type the new Car Details");
                        bool car = true;
                        int? autoVal2 = null;

                        Console.Write("Car Licence Number: ");
                        string licence = Console.ReadLine();

                        Console.Write("Manufacturer: ");
                        string manu = Console.ReadLine();

                        Console.Write("Model: ");
                        string model = Console.ReadLine();

                        Console.Write("Year: ");
                        int year = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Auto Sale: 1. True  2.False");
                        int autoSale2 = Convert.ToInt32(Console.ReadLine());

                        if (autoSale2 == 1)
                        {
                            car = false;
                            Console.Write("Auto Valuation Amount: ");
                            autoVal2 = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.Write("Normal Valuation: ");
                        int normal2 = Convert.ToInt32(Console.ReadLine());

                        AddCarRequest addCar = new AddCarRequest();
                        addCar.Licence = licence;
                        addCar.Manufacturer = manu;
                        addCar.Model = model;
                        addCar.Year = year;
                        addCar.Type = Asset.AssetType.Car;
                        addCar.Auto_Sale = car;
                        addCar.Auto_Valuation = autoVal2;
                        addCar.Normal_Valuation = normal2;
                        addCar.Owner = 1;

                        Console.WriteLine("");
                        Console.WriteLine(cc.AddCar(addCar).Message);
                        break;
                    case 16:
                        Console.WriteLine("Type the new Property Details");
                        bool prop = true;
                        int? autoVal3 = null;

                        Console.Write("Property Address: ");
                        string address2 = Console.ReadLine();

                        Console.Write("Square Meters: ");
                        int sq = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Property Type: 1. House 2. Apartment 3. Commercial Building");
                        int propType = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("Auto Sale: 1. True  2.False");
                        int autoSale3 = Convert.ToInt32(Console.ReadLine());

                        if (autoSale3 == 1)
                        {
                            prop = false;
                            Console.Write("Auto Valuation Amount: ");
                            autoVal3 = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.Write("Normal Valuation: ");
                        int normal3 = Convert.ToInt32(Console.ReadLine());

                        AddPropertyRequest addProp = new AddPropertyRequest();
                        addProp.Address = address2;
                        addProp.SQ = sq;
                        addProp.Type = Asset.AssetType.Property;
                        if(propType == 1)
                        {
                            addProp.Property_Type = Property.PropertyType.House;
                        }else if(propType == 2)
                        {
                            addProp.Property_Type = Property.PropertyType.Apartment;
                        }else if(propType == 3)
                        {
                            addProp.Property_Type = Property.PropertyType.Commercial_Building;
                        }
                        addProp.Auto_Sale = prop;
                        addProp.Auto_Valuation = autoVal3;
                        addProp.Normal_Valuation = normal3;
                        addProp.Owner = 1;

                        Console.WriteLine("");
                        Console.WriteLine(cc.AddProperty(addProp).Message);
                        break;
                    case 17:
                        Console.Write("Enter Asset ID: ");
                        int assetID2 = Convert.ToInt32(Console.ReadLine());

                        DeregisterAssetRequest deAsset = new DeregisterAssetRequest();
                        deAsset.Asset_ID = assetID2;

                        Console.WriteLine("");
                        Console.WriteLine(cc.RemoveAsset(deAsset).Message);
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
