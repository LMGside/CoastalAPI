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
            if (this.cBL.InsertCustomer(rr.Name, rr.Surname, rr.DOB, rr.Address, rr.Identity_No, rr.Contact).Status == CoastalAPIModels.ResponseStatus.Success)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            else
            {
                return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotModified, "Update Error");
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
    }
}
