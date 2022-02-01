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
            Assert.AreEqual(coastalBL.InsertCustomer("Nandos", "Sauce", dob, "32 Cornfield Road", "9804245340084", "0735243762").Status, CoastalAPIModels.ResponseStatus.Success);
        }
    }
}
