using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoastalAPIBusinessLayer;
using CoastalAPIModels.Models;

namespace CoastalAPI.Controllers
{
    [RoutePrefix("api/coastalAPI")]
    public class CoastalAPIController : ApiController
    {
        private readonly CoastalAPIBL cBL;

        public CoastalAPIController (CoastalAPIBL cBL)
        {
            this.cBL = cBL;
        }

        /// <summary>
        /// Insert new Customer
        /// </summary>
        /// <param name="rr">Register Request</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("InsertCustomer")]
        public HttpResponseMessage InsertCustomer([FromBody] RegisterRequest rr)
        {
            RegisterResponse response =  this.cBL.InsertCustomer(rr.Name, rr.Surname, rr.DOB, rr.Address, rr.Identity_No, rr.Contact);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);


        }

        /// <summary>
        /// Freeze the customer from using their Account
        /// </summary>
        /// <param name="rr">Register Request</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("FreezeCustomer")]
        public HttpResponseMessage FreezeCustomer([FromBody] FreezeCustomerRequest rr)
        {
            FreezeCustomerResponse response = this.cBL.FreezeCustomer(rr.Identity_No);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);


        }

        /// <summary>
        /// Unblock Account
        /// </summary>
        /// <param name="rr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("UnfreezeCustomer")]
        public HttpResponseMessage UnfreezeCustomer([FromBody] UnfreezeCustomerRequest rr)
        {
            UnfreezeCustomerResponse response = this.cBL.UnfreezeCustomer(rr.Identity_No);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);

        }

        /// <summary>
        /// Remove Customer
        /// </summary>
        /// <param name="rr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Deregister")]
        public HttpResponseMessage DeregisterCustomer([FromBody] DeregisterCustomerRequest rr)
        {
            DeregisterCustomerResponse response = this.cBL.DeregisterCustomer(rr.Identity_No);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Deposit Funds to Wallet
        /// </summary>
        /// <param name="dfr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Deposit")]
        public HttpResponseMessage DepositFunds([FromBody] DepositFundsRequest dfr)
        {
            DepositFundsResponse response = this.cBL.DepositFunds(dfr.Id_No, dfr.FundAmount);
            
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);


        }

        /// <summary>
        /// Withdraw from Wallet
        /// </summary>
        /// <param name="wr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Withdraw")]
        public HttpResponseMessage WithdrawFunds([FromBody] WithdrawRequest wr)
        {
            WithdrawResponse response = this.cBL.WithdrawFunds(wr.Id_No, wr.FundAmount);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            
        }

        /// <summary>
        /// Buy an Asset
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("BuyAsset")]
        public HttpResponseMessage BuyAsset([FromBody] BuyAssetRequest bar)
        {
            BuyAssetResponse response = this.cBL.BuyAsset(bar.Asset_ID, bar.Buyer_ID, bar.Purchase_Price);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            
        }

        /// <summary>
        /// Review Transaction
        /// </summary>
        /// <param name="rtr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Review")]
        public HttpResponseMessage BuyAsset([FromBody] ReviewTransactionRequest rtr)
        {
            ReviewTransactionResponse response = this.cBL.AppproveTransaction(rtr.TransactionID, rtr.Decision);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            
        }

        /// <summary>
        /// View Transactions from Today
        /// </summary>
        /// <param name="dtr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("TodayTransactions")]
        public HttpResponseMessage TodayTransactions([FromBody] DayTransactionRequest dtr)
        {
            DayTransactionResponse response = this.cBL.ViewDayTransactions(dtr.Day);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            

        }

        /// <summary>
        /// View user's Transactions
        /// </summary>
        /// <param name="utr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("UserTransactions")]
        public HttpResponseMessage UserTransactions([FromBody] UserTransactionRequest utr)
        {
            UserTransactionResponse response = this.cBL.ViewUsersTransactions(utr.ID_No);
            
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            
        }

        /// <summary>
        /// View Transactions from Date Range
        /// </summary>
        /// <param name="drtr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("DateRangeTransactions")]
        public HttpResponseMessage DateRangeTransactions([FromBody] DateRangeTransactionsRequest drtr)
        {
            DateRangeTransactionsResponse response = this.cBL.ViewDateRangeTransactions(drtr.StartDate, drtr.EndDate);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            

        }

        /// <summary>
        /// Successful Transactions
        /// </summary>
        /// <param name="drtr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("SuccessfulTransactions")]
        public HttpResponseMessage SuccessfulTransactions([FromBody] UserTransactionRequest drtr)
        {
            UserTransactionResponse response = this.cBL.ViewSuccessfulTransactions();
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            

        }

        /// <summary>
        /// Unsuccessful Transactions
        /// </summary>
        /// <param name="drtr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("UnsuccessfulTransactions")]
        public HttpResponseMessage UnsuccessfulTransactions([FromBody] UserTransactionRequest drtr)
        {
            UserTransactionResponse response = this.cBL.ViewUnsuccessfulTransactions();
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            
        }

        /// <summary>
        /// Add Art
        /// </summary>
        /// <param name="aar"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Art")]
        public HttpResponseMessage AddArt([FromBody] AddArtRequest aar)
        {
            AddArtResponse response = this.cBL.InsertArt(aar.Artist, aar.Art_Title, aar.Art_Year, aar.Auto_Sale, aar.Auto_Valuation, aar.Normal_Valuation);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);

        }

        /// <summary>
        /// Add Car
        /// </summary>
        /// <param name="acr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Car")]
        public HttpResponseMessage AddCar([FromBody] AddCarRequest acr)
        {
            AddCarResponse response = this.cBL.InsertCar(acr.Licence, acr.Manufacturer, acr.Model, acr.Year, acr.Auto_Sale, acr.Auto_Valuation, acr.Normal_Valuation);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);

        }

        /// <summary>
        /// Add Property
        /// </summary>
        /// <param name="apr"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Property")]
        public HttpResponseMessage AddProperty([FromBody] AddPropertyRequest apr)
        {
            AddPropertyResponse response = this.cBL.InsertProperty(apr.Address, apr.SQ, apr.Property_Type, apr.Auto_Sale, apr.Auto_Valuation, apr.Normal_Valuation);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);

        }

        /// <summary>
        /// Remove Asset
        /// </summary>
        /// <param name="dar"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("RemoveAsset")]
        public HttpResponseMessage DeregisterAsset([FromBody] DeregisterAssetRequest dar)
        {
            DeregisterAssetResponse response = this.cBL.DeregisterAsset(dar.Asset_ID);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
