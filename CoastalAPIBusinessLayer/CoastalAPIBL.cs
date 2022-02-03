using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIDataLayer.Models;
using CoastalAPIDataLayer;
using CoastalAPIDataLayer.Factories;
using CoastalAPIModels.Models;
using Contour.Localisation;
using System.Diagnostics;

namespace CoastalAPIBusinessLayer
{
    public class CoastalAPIBL
    {
        private readonly CustomerFactory customerFactory;
        private readonly ErrorLogFactory errorLogFactory;
        private readonly WalletFactory walletFactory;
        private readonly TransactionFactory transactionFactory;
        private readonly ArtFactory artFactory;
        private readonly PropertyFactory propertyFactory;
        private readonly CarFactory carFactory;
        private readonly AssetFactory assetFactory;
        private readonly CommissionLogFactory commissionLogFactory;
        private readonly string dbConnectionString;
        private readonly ICoastalAPISettings coastalAPISettings;
        public CoastalAPIBL(ICoastalAPISettings coastalAPISettings)
        {
            this.coastalAPISettings = coastalAPISettings;
            this.dbConnectionString = coastalAPISettings.GetCoastalAPIDBConnectionString();
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
            this.walletFactory = new WalletFactory(this.dbConnectionString);
            this.transactionFactory = new TransactionFactory(this.dbConnectionString);
            this.artFactory = new ArtFactory(this.dbConnectionString);
            this.propertyFactory = new PropertyFactory(this.dbConnectionString);
            this.carFactory = new CarFactory(this.dbConnectionString);
            this.assetFactory = new AssetFactory(this.dbConnectionString);
            this.commissionLogFactory = new CommissionLogFactory(this.dbConnectionString);
        }

        public CoastalAPIBL(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
            this.walletFactory = new WalletFactory(this.dbConnectionString);
            this.transactionFactory = new TransactionFactory(this.dbConnectionString);
            this.artFactory = new ArtFactory(this.dbConnectionString);
            this.propertyFactory = new PropertyFactory(this.dbConnectionString);
            this.carFactory = new CarFactory(this.dbConnectionString);
            this.assetFactory = new AssetFactory(this.dbConnectionString);
            this.commissionLogFactory = new CommissionLogFactory(this.dbConnectionString);
        }

        public RegisterResponse InsertCustomer(string name, string surname, DateTime dob, string address, string id, string tele)
        {
            RegisterResponse elc = new RegisterResponse();
            try
            {
                Customer customer = this.customerFactory.Create(e =>
                {
                    e.Name = name;
                    e.Surname = surname;
                    e.DOB = dob;
                    e.Address = address;
                    e.Identity_No = id;
                    e.Contact = tele;
                    e.Blocked = false;
                    e.Sales_Made = 0;
                    e.Rating = 0;
                });

                if (this.customerFactory.GetCustomer(id))
                {
                    elc.Error.ErrorMessage = "Customer already exists";
                    elc.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return elc;
                }
                else
                {
                    customer.Insert();

                    elc.Status = CoastalAPIModels.ResponseStatus.Success;
                    return elc;
                }
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Inserting Customer", "InsertCustomer in BL");

                elc.Error.ErrorMessage = "Error Inserting new Customer";
                elc.Error.StackTrace = e.StackTrace;
                elc.Error.CrashedMethod = "InsertCustomer in BL";

                elc.Status = CoastalAPIModels.ResponseStatus.Error;

                return elc;
            }
        }

        public FreezeCustomerResponse FreezeCustomer(string id)
        {
            FreezeCustomerResponse fcr = new FreezeCustomerResponse();
            try
            {

                if (this.customerFactory.FreezeCustomer(id))
                {
                    fcr.Status = CoastalAPIModels.ResponseStatus.Success;
                    return fcr;
                }
                else
                {
                    fcr.Error.ErrorMessage = "Customer not found";
                    fcr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return fcr;
                }
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Inserting Customer", "FreezeCustomer in BL");

                fcr.Error.ErrorMessage = "Error Inserting new Customer";
                fcr.Error.StackTrace = e.StackTrace;
                fcr.Error.CrashedMethod = "FreezeCustomer in  BL";

                fcr.Status = CoastalAPIModels.ResponseStatus.Error;

                return fcr;
            }
        }

        public UnfreezeCustomerResponse UnfreezeCustomer(string id)
        {
            UnfreezeCustomerResponse ufcr = new UnfreezeCustomerResponse();
            try
            {

                if (this.customerFactory.UnfreezeCustomer(id))
                {
                    ufcr.Status = CoastalAPIModels.ResponseStatus.Success;
                    return ufcr;
                }
                else
                {
                    ufcr.Error.ErrorMessage = "Customer not found";
                    ufcr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return ufcr;
                }
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Unfreezing Customer", "UnfreezeCustomer in BL");

                ufcr.Error.ErrorMessage = "Error Unfreezing Customer";
                ufcr.Error.StackTrace = e.StackTrace;
                ufcr.Error.CrashedMethod = "UnfreezeCustomer in  BL";

                ufcr.Status = CoastalAPIModels.ResponseStatus.Error;

                return ufcr;
            }
        }

        public DeregisterCustomerResponse DeregisterCustomer(string id)
        {
            DeregisterCustomerResponse dcr = new DeregisterCustomerResponse();
            try
            {
                Customer customer = new Customer(this.dbConnectionString);
                Wallet wallet = new Wallet(this.dbConnectionString);
                Customer newCus = customer.Get(id);

                if (newCus != null)
                {
                    customer.Delete(newCus.ID);
                    wallet.Delete(newCus.ID);
                    
                    dcr.Status = CoastalAPIModels.ResponseStatus.Success;
                    return dcr;
                }
                else
                {
                    dcr.Error.ErrorMessage = "Customer not found";
                    dcr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return dcr;
                }
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Deleting Customer", "DeregisterCustomer in BL");

                dcr.Error.ErrorMessage = "Error Deleting Customer";
                dcr.Error.StackTrace = e.StackTrace;
                dcr.Error.CrashedMethod = "DeregisterCustomer in  BL";

                dcr.Status = CoastalAPIModels.ResponseStatus.Error;

                return dcr;
            }
        }

        public DepositFundsResponse DepositFunds(string id, decimal amount)
        {
            DepositFundsResponse dfr = new DepositFundsResponse();
            try
            {
                Customer customer = new Customer(this.dbConnectionString);
                Wallet wallet = new Wallet(this.dbConnectionString);
                Customer newCus = customer.Get(id);

                if (newCus == null)
                {
                    dfr.Error.ErrorMessage = "Customer not Found";
                    dfr.Error.CrashedMethod = "DepositFunds";
                    dfr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return dfr;
                }
                else if (newCus.Blocked)
                {
                    dfr.Error.ErrorMessage = "Can't Access Blocked Account";
                    dfr.Error.CrashedMethod = "DepositFunds";
                    dfr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return dfr;
                }
                else
                {
                    if(this.walletFactory.AddDeposit(newCus.ID, amount))
                    {
                        dfr.Status = CoastalAPIModels.ResponseStatus.Success;
                    }

                    return dfr;
                }
            }
            catch(Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Depositing Funds", "DepositFunds in BL");

                dfr.Error.ErrorMessage = "Error Depositing Funds";
                dfr.Error.StackTrace = e.StackTrace;
                dfr.Error.CrashedMethod = "DepositFunds in  BL";

                dfr.Status = CoastalAPIModels.ResponseStatus.Error;

                return dfr;
            }
        }

        public WithdrawResponse WithdrawFunds(string id, decimal amount)
        {
            WithdrawResponse wr = new WithdrawResponse();
            try
            {
                Customer customer = new Customer(this.dbConnectionString);
                Wallet wallet = new Wallet(this.dbConnectionString);
                Customer newCus = customer.Get(id);
                Settings settings = new Settings(this.dbConnectionString);
                decimal maxWithdrawal = settings.Get().MaxWithdrawal;
                if (newCus == null)
                {
                    wr.Error.ErrorMessage = "Customer not Found";
                    wr.Error.CrashedMethod = "WithdrawFunds";
                    wr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return wr;

                }

                decimal balance = wallet.Get(newCus.ID).Balance;

                if (newCus.Blocked)
                {
                    wr.Error.ErrorMessage = "Can't Access Blocked Account";
                    wr.Error.CrashedMethod = "WithdrawFunds";
                    wr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return wr;
                }
                else if(amount > maxWithdrawal)
                {
                    wr.Error.ErrorMessage = "Surpassed the number of Customer Withdrawals";
                    wr.Error.CrashedMethod = "WithdrawFunds";
                    wr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return wr;
                }
                else if(amount > balance)
                {
                    wr.Error.ErrorMessage = "Insufficient Balance to Withdraw funds";
                    wr.Error.CrashedMethod = "WithdrawFunds";
                    wr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return wr;
                }
                else
                {
                    this.walletFactory.WithdrawDeposit(newCus.ID, amount);
                    wr.Status = CoastalAPIModels.ResponseStatus.Success;

                    return wr;
                }
            }
            catch(Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Withdrawing Funds", "WithdrawFunds in BL");

                wr.Error.ErrorMessage = "Error Withdrawing Funds";
                wr.Error.StackTrace = e.StackTrace;
                wr.Error.CrashedMethod = "WithdrawFunds in BL";

                wr.Status = CoastalAPIModels.ResponseStatus.Error;

                return wr;
            }
        }

        public BuyAssetResponse BuyAsset(int asset_Id, string buyer_Id, decimal purchasePrice)
        {
            BuyAssetResponse bar = new BuyAssetResponse();
            try
            {
                Customer customer = new Customer(this.dbConnectionString);
                Wallet wallet = new Wallet(this.dbConnectionString);
                Customer newCus = customer.Get(buyer_Id);
                Settings settings = new Settings(this.dbConnectionString).Get();
                Asset asset = new Asset(this.dbConnectionString).Get(asset_Id);

                Wallet buyerW = wallet.Get(newCus.ID);
                Wallet sellerW = wallet.Get(asset.Owner);

                decimal balance = buyerW.Balance;
                bool automatic = false;

                switch (newCus.Rating)
                {
                    case 0:
                        automatic = settings.Auto_0;
                        break;
                    case 1:
                        automatic = settings.Auto_1;
                        break;
                    case 2:
                        automatic = settings.Auto_2;
                        break;
                }

                if (newCus.Blocked)
                {
                    bar.Error.ErrorMessage = "Account Blocked";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if (purchasePrice > balance)
                {
                    bar.Error.ErrorMessage = "Insufficient Balance to make the purchase";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if(asset.Owner == newCus.ID)
                {
                    bar.Error.ErrorMessage = "Asset Already Owned By "+ newCus.Name;
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if(this.transactionFactory.MaxTransactionsCheck() >= settings.SalesToday)
                {
                    bar.Error.ErrorMessage = "Maximized Sales for the Day";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if(this.transactionFactory.MaxTransactionsBuyerCheck(newCus.ID) >= settings.UniqueSalesToday)
                {
                    bar.Error.ErrorMessage = "Customer has Maximized Sales for the Day";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if (!asset.Auto_Sale && asset.Owner != 1)
                {
                    bar.Error.ErrorMessage = "Asset Not for Sale by Customer";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }else if(asset.Normal_Valuation > purchasePrice)
                {
                    bar.Error.ErrorMessage = "Normal Valuation not met";
                    bar.Error.CrashedMethod = "BuyAsset";
                    bar.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return bar;
                }
                else if(purchasePrice >= asset.Auto_Valuation && automatic)
                {
                    decimal commission = 0;
                    double discount = 0;
                    double customerLoss = 0;
                    decimal endCost = 0;
                    Wallet sellerWallet = wallet.Get(asset.Owner);

                    switch (newCus.Rating)
                    {
                        case 1:
                            discount = 0.0025;
                            break;
                        case 2:
                            discount = 0.005;
                            break;
                    }

                    customerLoss = (double)purchasePrice * (1 - discount);
                    commission = (decimal)(customerLoss * 0.01);
                    endCost = (decimal)(customerLoss * 0.99);

                    Transaction newTrans = this.transactionFactory.Create(e =>
                    {
                        e.Buyer = newCus.ID;
                        e.Seller = asset.Owner;
                        e.Asset = asset.ID;
                        e.Amount = endCost;
                        e.Auto_Sale = true;
                        e.Status = Transaction.TransactionStatus.Success;
                        e.Date_Transaction_Requested = DateTime.Now;
                        e.Date_Transaction_Approved = DateTime.Now;
                        e.Who_Approved = "Auto";
                    });
                    int transID = newTrans.Insert();

                    Debug.WriteLine("Subtract R" + customerLoss + " from Customer Account");
                    this.walletFactory.WithdrawDeposit(newCus.ID, (decimal)customerLoss);

                    Debug.WriteLine("Add R" + endCost + " to Seller's Customer Account");
                    this.walletFactory.AddDeposit(asset.Owner, endCost);

                    Debug.WriteLine("Add R" + commission + " to Commision Log");
                    CommissionLog clog = this.commissionLogFactory.Create(e =>
                    {
                        e.TransactionID = transID;
                        e.TransactionDate = DateTime.Now;
                        e.Commission = commission;
                    });
                    clog.Insert();

                    Debug.WriteLine("Increment Sale_Made for Customer");
                    Debug.WriteLine("Check Rating Change");
                    newCus.Sales_Made += 1;
                    if (newCus.Sales_Made >= 5 && newCus.Sales_Made <= 10){
                        newCus.Rating = 1;
                    }
                    if (newCus.Sales_Made > 10)
                    {
                        newCus.Rating = 2;
                    }
                    newCus.Update(newCus.ID);

                    Debug.WriteLine("Change Owner of Asset");
                    asset.Owner = newCus.ID;
                    asset.Update();

                    //Update Transaction
                    newTrans.Status = Transaction.TransactionStatus.Approved;
                    newTrans.Who_Approved = "Auto";
                    newTrans.Update(newTrans.ID);

                    bar.Message = "Item has been Auto Processed";
                    bar.Status = CoastalAPIModels.ResponseStatus.Success;
                    return bar;

                } else {

                    double discount = 0;
                    double customerLoss = 0;
                    decimal endCost = 0;
                    switch (newCus.Rating)
                    {
                        case 1:
                            discount = 0.0025;
                            break;
                        case 2:
                            discount = 0.005;
                            break;
                    }

                    customerLoss = (double)purchasePrice * (1 - discount);
                    endCost = (decimal)(customerLoss * 0.99);

                    Transaction newTrans = this.transactionFactory.Create(e =>
                    {
                        e.Buyer = newCus.ID;
                        e.Seller = asset.Owner;
                        e.Asset = asset.ID;
                        e.Amount = endCost;
                        e.Auto_Sale = false;
                        e.Status = Transaction.TransactionStatus.Success;
                        e.Date_Transaction_Requested = DateTime.Now;
                    });
                    this.transactionFactory.AddWaitingApprovalTransaction(newTrans);

                    Debug.WriteLine("Subtract R" + customerLoss + " from Customer Account");
                    this.walletFactory.WithdrawDeposit(newCus.ID, (decimal)customerLoss);

                    bar.Message = "Item is waiting for Approval";
                    bar.Status = CoastalAPIModels.ResponseStatus.Success;
                    return bar;

                }
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Withdrawing Funds", "WithdrawFunds in BL");

                bar.Error.ErrorMessage = "Error Withdrawing Funds";
                bar.Error.StackTrace = e.StackTrace;
                bar.Error.CrashedMethod = "WithdrawFunds in BL";

                bar.Status = CoastalAPIModels.ResponseStatus.Error;

                return bar;
            }
        }

        public ReviewTransactionResponse AppproveTransaction(int transaction_Id, ReviewTransactionRequest.TransactionStatus decision)
        {
            ReviewTransactionResponse rtr = new ReviewTransactionResponse();
            Transaction trans = this.transactionFactory.Create(e =>
            {
                e.ID = transaction_Id;
            }).Get();
            Wallet wallet = new Wallet(this.dbConnectionString);
            Customer customer = new Customer(this.dbConnectionString);
            Customer newCus = customer.GetID(trans.Buyer);
            Asset asset = new Asset(this.dbConnectionString).Get(trans.Asset);

            decimal commission = 0;
            double discount = 0;
            double customerLoss = 0;
            decimal endCost = 0;

            // Calculate Amounts

            decimal totalPur = Decimal.Divide(trans.Amount, (decimal)0.99);
            double totalPur1 = ((double)totalPur);
            commission = (decimal)(totalPur1 * 0.01);
            customerLoss = (double)totalPur * (1 - discount);
            endCost = trans.Amount;

            if (decision == ReviewTransactionRequest.TransactionStatus.Approved)
            {

                Debug.WriteLine("Add R" + endCost + " to Seller's Customer Account");
                this.walletFactory.AddDeposit(trans.Seller, endCost);

                Debug.WriteLine("Add R" + commission + " to Commision Log");
                CommissionLog clog = this.commissionLogFactory.Create(e =>
                {
                    e.TransactionID = trans.ID;
                    e.TransactionDate = DateTime.Now;
                    e.Commission = commission;
                });
                clog.Insert();

                Debug.WriteLine("Increment Sale_Made for Customer");
                Debug.WriteLine("Check Rating Change");
                newCus.Sales_Made += 1;
                if (newCus.Sales_Made >= 5 && newCus.Sales_Made <= 10)
                {
                    newCus.Rating = 1;
                }
                if (newCus.Sales_Made > 10)
                {
                    newCus.Rating = 2;
                }
                newCus.Update(newCus.ID);

                Debug.WriteLine("Change Owner of Asset");
                asset.Owner = newCus.ID;
                asset.Update();

                //Update Transaction
                trans.Status = Transaction.TransactionStatus.Approved;
                trans.Who_Approved = "Employee";
                trans.Update(trans.ID);

                rtr.Message = "Item has been Approved";
                rtr.Status = CoastalAPIModels.ResponseStatus.Success;

                return rtr;
            }
            else
            {
                this.walletFactory.AddDeposit(trans.Buyer, (decimal)customerLoss);

                trans.Status = Transaction.TransactionStatus.Rejected;
                this.transactionFactory.UpdateRejectedTransaction(trans);

                rtr.Message = "Item has been Rejected";
                rtr.Status = CoastalAPIModels.ResponseStatus.Success;

                return rtr;
            }
        }

        public DayTransactionResponse ViewDayTransactions (DateTime day)
        {
            DayTransactionResponse dtr = new DayTransactionResponse();
            try
            {
                dtr.Transactions = this.transactionFactory.GetDayTransactions(day);

                return dtr;
            }catch(Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Getting Day Transaction", "ViewDayTransaction in BL");

                dtr.Error.ErrorMessage = "Error Getting Day Transaction";
                dtr.Error.StackTrace = e.StackTrace;
                dtr.Error.CrashedMethod = "ViewDayTransaction in BL";

                dtr.Status = CoastalAPIModels.ResponseStatus.Error;

                return dtr;
            }
        }

        public UserTransactionResponse ViewUsersTransactions(string id)
        {
            UserTransactionResponse utr = new UserTransactionResponse();
            try
            {
                Customer getCus = new Customer(this.dbConnectionString).Get(id);
                utr.Transactions = this.transactionFactory.GetUsersTransactions(getCus.ID);

                return utr;
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Getting User Transaction", "ViewUsersTransaction in BL");

                utr.Error.ErrorMessage = "Error Getting User Transaction";
                utr.Error.StackTrace = e.StackTrace;
                utr.Error.CrashedMethod = "ViewUserTransaction in BL";

                utr.Status = CoastalAPIModels.ResponseStatus.Error;

                return utr;
            }
        }

        public DateRangeTransactionsResponse ViewDateRangeTransactions(DateTime startD, DateTime endD)
        {
            DateRangeTransactionsResponse drtr = new DateRangeTransactionsResponse();
            try
            {
                drtr.Transactions = this.transactionFactory.GetDateRangeTransactions(startD, endD);

                return drtr;
            }
            catch (Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Getting Date Range Transaction", "ViewDateRangeTransaction in BL");

                drtr.Error.ErrorMessage = "Error Getting Date Range Transaction";
                drtr.Error.StackTrace = e.StackTrace;
                drtr.Error.CrashedMethod = "ViewDateRangeTransaction in BL";

                drtr.Status = CoastalAPIModels.ResponseStatus.Error;

                return drtr;
            }
        }

        public void BuildAndInsertErrorLog(Exception exception, string errorMessage, string method)
        {
            var errorLog = this.errorLogFactory.Create(e =>
            {
                e.CreatedDate = DTConfig.GetConfiguredDateTime();
                e.ErrorMessage = errorMessage;
                e.ExceptionMessage = exception.Message;
                e.CrashedMethod = method;
                e.StackTrace = exception.StackTrace;
            });

            errorLog.Insert();
        }

        public PropertyFactory PropertyFactory
        {
            get { return this.propertyFactory; }
            private set { }
        }
        public ArtFactory ArtFactory
        {
            get { return this.artFactory; }
            private set { }
        }
        public CarFactory CarFactory
        {
            get { return this.carFactory; }
            private set { }
        }
    }
}
