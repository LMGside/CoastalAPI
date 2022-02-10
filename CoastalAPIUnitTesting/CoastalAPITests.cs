using System;
using System.Collections.Generic;
using CoastalAPIBusinessLayer;
using CoastalAPIModels.Models;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoastalAPIDataLayer.Models;

namespace CoastalAPIUnitTesting
{
    [TestClass]
    public class CoastalAPITests
    {
        private CoastalAPIBL coastalBL;

        public CoastalAPITests()
        {
            this.coastalBL = new CoastalAPIBL(ConfigurationManager.ConnectionStrings["CoastalFinanceDB"].ConnectionString);
        }

        [TestMethod]
        public void TestInsert()
        {
            string name = "Jam";
            string surname = "Mama";
            DateTime dob = new DateTime(1995, 8, 22);
            string address = "98 Lambier Street";
            string id = "9508225094700";
            string contact = "0836253485";
            RegisterResponse rr = coastalBL.InsertCustomer(name, surname, dob, address, id, contact);

            Assert.AreEqual(rr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestInsertExistingCustomer()
        {
            string name = "Coastal Finances";
            string surname = "Company";
            DateTime dob = new DateTime(1998, 4, 24);
            string address = "32 Cornfield Road";
            string id = "1";
            string contact = "0312097978";
            RegisterResponse rr = coastalBL.InsertCustomer(name, surname, dob, address, id, contact);

            Assert.AreEqual(rr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestFreezingCustomer()
        {
            string id = "8907149839483";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestUnfreezingCustomer()
        {
            string id = "8907149839483";
            UnfreezeCustomerResponse ufcr = coastalBL.UnfreezeCustomer(id);

            Assert.AreEqual(ufcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestFreezingEmptyCustomer()
        {
            string id = "9804245340084948363";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestUnfreezingEmptyCustomer()
        {
            string id = "9804245340084938743";
            UnfreezeCustomerResponse ufcr = coastalBL.UnfreezeCustomer(id);

            Assert.AreEqual(ufcr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestDeregisterCustomer()
        {
            string id = "1";
            DeregisterCustomerResponse drcr = coastalBL.DeregisterCustomer(id);

            Assert.AreEqual(drcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestDepositFunds()
        {
            string ID_no = "8907125094763";
            decimal amount = (decimal)5000000;

            Assert.AreEqual(coastalBL.DepositFunds(ID_no, amount).Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestDepositFundsNonExistingCustomer()
        {
            string ID_no = "980424534008494763";
            decimal amount = (decimal)500.23;

            DepositFundsResponse dfr = coastalBL.DepositFunds(ID_no, amount);

            Assert.AreEqual(dfr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawsFunds()
        {
            string ID_no = "1";
            decimal amount = (decimal)29.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestWithdrawFundsNonExistingCustomer()
        {
            string ID_no = "980424534008494763";
            decimal amount = (decimal)50.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Customer not Found");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestMaxWithdrawFunds()
        {
            string ID_no = "991011948503";
            decimal amount = (decimal)5000.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Surpassed the number of Customer Withdrawals");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawInsufficientFunds()
        {
            string ID_no = "991011948503";
            decimal amount = (decimal) 499.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Insufficient Balance to Withdraw funds");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawBlockedAccount()
        {
            string id = "1";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Success);

            decimal amount = (decimal) 29.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(id, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Can't Access Blocked Account");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestUserTransactionList()
        {
            string id = "8907125094763";
            UserTransactionResponse utr = coastalBL.ViewUsersTransactions(id);

            Assert.AreEqual(utr.Status, CoastalAPIModels.ResponseStatus.Success);

            decimal amount = (decimal)29.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(id, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Can't Access Blocked Account");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestInsertProperty()
        {
            Property prop =  coastalBL.PropertyFactory.Create(e =>
            {
                e.Address = "228 Main Road";
                e.SQ = 98;
                e.Property_Type = CoastalAPIDataLayer.Models.Property.PropertyType.House;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Property;
                e.Auto_Sale = false;
                e.Auto_Valuation = 3000000;
                e.Normal_Valuation = 1500000;
                e.Owner = 0;
            });

            Assert.IsTrue(prop.Insert());
        }

        [TestMethod]
        public void TestInsertProperty2()
        {
            Property prop = coastalBL.PropertyFactory.Create(e =>
            {
                e.Address = "204 Main Road";
                e.SQ = 100;
                e.Property_Type = CoastalAPIDataLayer.Models.Property.PropertyType.House;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Property;
                e.Auto_Sale = true;
                e.Auto_Valuation = 7000000;
                e.Normal_Valuation = 3900000;
                e.Owner = 0;
            });

            Assert.IsTrue(prop.Insert());
        }

        [TestMethod]
        public void TestInsertArt()
        {
            Art art = coastalBL.ArtFactory.Create(e =>
            {
                e.Artist = "Tommy Lee";
                e.ArtTitle = "The Jungle";
                e.Year_Completed = 2004;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Art;
                e.Auto_Sale = false;
                e.Auto_Valuation = 200000;
                e.Normal_Valuation = 100000;
                e.Owner = 0;
            });

            Assert.IsTrue(art.Insert());
        }

        [TestMethod]
        public void TestInsertCar()
        {
            Car car = coastalBL.CarFactory.Create(e =>
            {
                e.Licence = "DF09957634FT";
                e.Manufacturer = "Toyota";
                e.Model = "Bishop";
                e.Year = 2021;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Car;
                e.Auto_Sale = false;
                e.Auto_Valuation = 900000;
                e.Normal_Valuation = 800000;
                e.Owner = 0;
            });

            Assert.IsTrue(car.Insert());
        }

        [TestMethod]
        public void TestBuyAssetInsufficientBalance()
        {
            int assetId = 2;
            string buyerID = "8907125094763";
            decimal amount = (decimal) 40000000;
            BuyAssetResponse bar = coastalBL.BuyAsset(assetId, buyerID, amount);

            Assert.AreEqual(bar.Error.ErrorMessage, "Insufficient Balance to make the purchase");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestBuyAssetNormalValuation()
        {
            int assetId = 4;
            string buyerID = "991011948503";
            decimal amount = (decimal)4;
            BuyAssetResponse bar = coastalBL.BuyAsset(assetId, buyerID, amount);

            Assert.AreEqual(bar.Error.ErrorMessage, "Normal Valuation not met");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestBuyAssetNotForSale()
        {
            int assetId = 3;
            string buyerID = "8907149839483";
            decimal amount = (decimal)500000;
            BuyAssetResponse bar = coastalBL.BuyAsset(assetId, buyerID, amount);

            Assert.AreEqual(bar.Error.ErrorMessage, "Asset Not for Sale by Customer");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestBuyOwnAsset()
        {
            string owner_id_no = "0";
            int asset_id = 1;
            decimal purchase = 4;

            BuyAssetResponse bar = coastalBL.BuyAsset(asset_id, owner_id_no, purchase);

            Assert.AreEqual(bar.Error.ErrorMessage, "Asset Already Owned By Nandos");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestBuyAsset()
        {
            string owner_id_no = "8907125094763";
            int asset_id = 3;
            decimal purchase = 500000;

            BuyAssetResponse bar = coastalBL.BuyAsset(asset_id, owner_id_no, purchase);

            Assert.AreEqual(bar.Message, "Item is waiting for Approval");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Success);
        }


        [TestMethod]
        public void TestRejectedAsset()
        {
            int transaction_id = 7;
            ReviewTransactionRequest.TransactionStatus decision =  ReviewTransactionRequest.TransactionStatus.Rejected;

            ReviewTransactionResponse rtr = coastalBL.AppproveTransaction(transaction_id, decision);

            Assert.AreEqual(rtr.Message, "Item has been Rejected");
            Assert.AreEqual(rtr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestAcceptedAsset()
        {
            int transaction_id = 5;
            ReviewTransactionRequest.TransactionStatus decision = ReviewTransactionRequest.TransactionStatus.Approved;

            ReviewTransactionResponse rtr = coastalBL.AppproveTransaction(transaction_id, decision);

            Assert.AreEqual(rtr.Message, "Item has been Approved");
            Assert.AreEqual(rtr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestAlreadyReviewedAsset()
        {
            int transaction_id = 5;
            ReviewTransactionRequest.TransactionStatus decision = ReviewTransactionRequest.TransactionStatus.Approved;

            ReviewTransactionResponse rtr = coastalBL.AppproveTransaction(transaction_id, decision);

            Assert.AreEqual(rtr.Message, "Transaction has been reviewed");
            Assert.AreEqual(rtr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestViewTransactionsUserInvalid()
        {
            string user = "49865325094763";

            var list = coastalBL.ViewUsersTransactions(user).Message;

            Assert.AreEqual(list, "Customer not Found");
        }

        [TestMethod]
        public void TestViewTransactionsDate()
        {
            DateTime date = new DateTime(2022, 02, 03);

            var list = coastalBL.ViewDayTransactions(date).Transactions; 
            
            foreach (Transaction x in list)
            {
                Assert.AreEqual(date, x.Date_Transaction_Requested);
            }
        }

        [TestMethod]
        public void TestViewTransactionsDateRange()
        {
            DateTime sDate = new DateTime(2022, 02, 02);
            DateTime eDate = new DateTime(2022, 02, 04);

            var list = coastalBL.ViewDateRangeTransactions(sDate, eDate).Transactions;

            foreach (Transaction x in list)
            {
                bool checker = x.Date_Transaction_Requested >= sDate && x.Date_Transaction_Requested <= eDate;
                Assert.IsTrue(checker);
            }
        }

        [TestMethod]
        public void TestViewTransactionsSuccess()
        {
            var list = coastalBL.ViewSuccessfulTransactions().Transactions;

            foreach (Transaction x in list)
            {
                Assert.IsTrue(x.Status == Transaction.TransactionStatus.Approved);
            }
        }

        [TestMethod]
        public void TestViewTransactionsUnsuccess()
        {
            var list = coastalBL.ViewUnsuccessfulTransactions().Transactions;

            foreach (Transaction x in list)
            {
                Assert.IsTrue(x.Status == Transaction.TransactionStatus.Rejected);
            }
        }

        [TestMethod]
        public void TestDeregisterAssetInvalid()
        {
            int id = 46;
            DeregisterAssetResponse dar = coastalBL.DeregisterAsset(id);

            Assert.AreEqual(dar.Message, "Asset Not found");
        }

        [TestMethod]
        public void TestDeregisterOwnerAsset()
        {
            int id = 9;
            DeregisterAssetResponse dar = coastalBL.DeregisterAsset(id);

            Assert.AreEqual(dar.Message, "Can't Deregister Customer's Assets");
        }


    }
}
