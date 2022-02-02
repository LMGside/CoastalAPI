using System;
using System.Collections.Generic;
using CoastalAPIBusinessLayer;
using CoastalAPIModels.Models;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            string name = "Nandos";
            string surname = "Sauce";
            DateTime dob = new DateTime(1998, 4, 24);
            string address = "32 Cornfield Road";
            string id = "9804245340084";
            string contact = "0735243762";
            RegisterResponse rr = coastalBL.InsertCustomer(name, surname, dob, address, id, contact);

            Assert.AreEqual(rr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestInsertExistingCustomer()
        {
            string name = "Nandos";
            string surname = "Sauce";
            DateTime dob = new DateTime(1998, 4, 24);
            string address = "32 Cornfield Road";
            string id = "9804245340084";
            string contact = "0735243762";
            RegisterResponse rr = coastalBL.InsertCustomer(name, surname, dob, address, id, contact);

            Assert.AreEqual(rr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestFreezingCustomer()
        {
            string id = "9804245340084";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestUnfreezingCustomer()
        {
            string id = "9804245340084";
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
            string id = "9804245340084";
            DeregisterCustomerResponse drcr = coastalBL.DeregisterCustomer(id);

            Assert.AreEqual(drcr.Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestDepositFunds()
        {
            string ID_no = "9804245340084";
            decimal amount = (decimal)500.23;

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
            string ID_no = "9804245340084";
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
            string ID_no = "9804245340084";
            decimal amount = (decimal)5000.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Surpassed the number of Customer Withdrawals");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawInsufficientFunds()
        {
            string ID_no = "9804245340084";
            decimal amount = (decimal)499.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(ID_no, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Insufficient Balance to Withdraw funds");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestWithdrawBlockedAccount()
        {
            string id = "9804245340084";
            FreezeCustomerResponse fcr = coastalBL.FreezeCustomer(id);

            Assert.AreEqual(fcr.Status, CoastalAPIModels.ResponseStatus.Success);

            decimal amount = (decimal)29.23;
            WithdrawResponse wdr = coastalBL.WithdrawFunds(id, amount);

            Assert.AreEqual(wdr.Error.ErrorMessage, "Can't Access Blocked Account");
            Assert.AreEqual(wdr.Status, CoastalAPIModels.ResponseStatus.Success);
        }
    }
}
