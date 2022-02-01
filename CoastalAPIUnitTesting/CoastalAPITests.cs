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
            DateTime dob = new DateTime(1998, 4, 24);
            Assert.AreEqual(coastalBL.InsertCustomer("Nandos", "Sauce", dob, "32 Cornfield Road", "9804245340084", "0735243762").Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestInsertExistingCustomer()
        {
            DateTime dob = new DateTime(1998, 4, 24);
            Assert.AreEqual(coastalBL.InsertCustomer("Nandos", "Sauce", dob, "32 Cornfield Road", "9804245340084", "0735243762").Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestFreezingCustomer()
        {
            Assert.AreEqual(coastalBL.FreezeCustomer("9804245340084").Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestUnfreezingCustomer()
        {
            Assert.AreEqual(coastalBL.UnfreezeCustomer("9804245340084").Status, CoastalAPIModels.ResponseStatus.Success);
        }

        [TestMethod]
        public void TestFreezingEmptyCustomer()
        {
            Assert.AreEqual(coastalBL.FreezeCustomer("98042453400765434").Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestUnfreezingEmptyCustomer()
        {
            Assert.AreEqual(coastalBL.UnfreezeCustomer("98042453400848543").Status, CoastalAPIModels.ResponseStatus.Fail);
        }

        [TestMethod]
        public void TestDeregisterCustomer()
        {
            Assert.AreEqual(coastalBL.DeregisterCustomer("98042453400848543").Status, CoastalAPIModels.ResponseStatus.Success);
        }


    }
}
