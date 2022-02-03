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
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Error. Couldn't Insert Customer");
            }

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
            if (this.cBL.FreezeCustomer(rr.Identity_No).Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }

        }

        [Authorize]
        [HttpPost, Route("UnfreezeCustomer")]
        public HttpResponseMessage UnfreezeCustomer([FromBody] UnfreezeCustomerRequest rr)
        {
            if (this.cBL.UnfreezeCustomer(rr.Identity_No).Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }
        }

        [Authorize]
        [HttpPost, Route("Deregister")]
        public HttpResponseMessage DeregisterCustomer([FromBody] DeregisterCustomerRequest rr)
        {
            if (this.cBL.DeregisterCustomer(rr.Identity_No).Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }

        }

        [Authorize]
        [HttpPost, Route("Deposit")]
        public HttpResponseMessage DepositFunds([FromBody] DepositFundsRequest dfr)
        {
            DepositFundsResponse response = this.cBL.DepositFunds(dfr.Id_No, dfr.FundAmount);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }

        }

        [Authorize]
        [HttpPost, Route("Withdraw")]
        public HttpResponseMessage WithdrawFunds([FromBody] WithdrawRequest wr)
        {
            WithdrawResponse response = this.cBL.WithdrawFunds(wr.Id_No, wr.FundAmount);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }
        }

        [Authorize]
        [HttpPost, Route("BuyAsset")]
        public HttpResponseMessage BuyAsset([FromBody] BuyAssetRequest bar)
        {
            BuyAssetResponse response = this.cBL.BuyAsset(bar.Asset_ID, bar.Buyer_ID, bar.Purchase_Price);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }
        }

        [Authorize]
        [HttpPost, Route("Review")]
        public HttpResponseMessage BuyAsset([FromBody] ReviewTransactionRequest rtr)
        {
            ReviewTransactionResponse response = this.cBL.AppproveTransaction(rtr.TransactionID, rtr.Decision);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
            }
        }

        [Authorize]
        [HttpPost, Route("TodayTransactions")]
        public HttpResponseMessage TodayTransactions([FromBody] DayTransactionRequest dtr)
        {
            DayTransactionResponse response = this.cBL.ViewDayTransactions(dtr.Day);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Error");
            }

        }

        [Authorize]
        [HttpPost, Route("UserTransactions")]
        public HttpResponseMessage UserTransactions([FromBody] UserTransactionRequest utr)
        {
            UserTransactionResponse response = this.cBL.ViewUsersTransactions(utr.ID_No);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Error");
            }
        }

        [Authorize]
        [HttpPost, Route("DateRangeTransactions")]
        public HttpResponseMessage DateRangeTransactions([FromBody] DateRangeTransactionsRequest drtr)
        {
            DateRangeTransactionsResponse response = this.cBL.ViewDateRangeTransactions(drtr.StartDate, drtr.EndDate);
            if (response.Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Error");
            }

        }
    }
}
