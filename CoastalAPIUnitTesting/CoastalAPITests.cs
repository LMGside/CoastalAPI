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
            string name = "Tando";
            string surname = "Lemar";
            DateTime dob = new DateTime(1989, 7, 12);
            string address = "98 Lambier Street";
            string id = "8907125094763";
            string contact = "0836253892";
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
            string id = "1";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestUnfreezingCustomer()
        {
            string id = "1";
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
            string ID_no = "1";
            decimal amount = (decimal)5000.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Surpassed the number of Customer Withdrawals");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawInsufficientFunds()
        {
            string ID_no = "1";
            decimal amount = (decimal)499.23;
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
                e.Owner = 1;
            });

            Assert.IsTrue(prop.Insert());
        }

        [TestMethod]
        public void TestInsertProperty2()
        {
            Property prop = coastalBL.PropertyFactory.Create(e =>
            {
                e.Address = "220 Main Road";
                e.SQ = 138;
                e.Property_Type = CoastalAPIDataLayer.Models.Property.PropertyType.Commercial_Building;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Property;
                e.Auto_Sale = false;
                e.Auto_Valuation = 6000000;
                e.Normal_Valuation = 3000000;
                e.Owner = 1;
            });

            Assert.IsTrue(prop.Insert());
        }

        [TestMethod]
        public void TestInsertArt()
        {
            Art art = coastalBL.ArtFactory.Create(e =>
            {
                e.Artist = "Leonard Di Vinci";
                e.ArtTitle = "Peru Se Qua";
                e.Year_Completed = 1992;
                e.Type = CoastalAPIDataLayer.Models.Asset.AssetType.Art;
                e.Auto_Sale = false;
                e.Auto_Valuation = 500000;
                e.Normal_Valuation = 300000;
                e.Owner = 1;
            });

            Assert.IsTrue(art.Insert());
        }

        [TestMethod]
        public void TestBuyAssetInsufficientBalance()
        {
            BuyAssetResponse bar = coastalBL.BuyAsset(2, "8907125094763", 40000000);

            Assert.AreEqual(bar.Error.ErrorMessage, "Insufficient Balance to make the purchase");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestBuyOwnAsset()
        {
            string owner_id_no = "1";
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
            decimal purchase = 300000;

            BuyAssetResponse bar = coastalBL.BuyAsset(asset_id, owner_id_no, purchase);

            Assert.AreEqual(bar.Error.ErrorMessage, "Normal Valuation not met");
            Assert.AreEqual(bar.Status, CoastalAPIModels.ResponseStatus.Fail);
        }
    }
}
