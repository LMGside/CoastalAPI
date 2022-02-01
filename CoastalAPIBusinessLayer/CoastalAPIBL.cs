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

namespace CoastalAPIBusinessLayer
{
    public class CoastalAPIBL
    {
        private readonly CustomerFactory customerFactory;
        private readonly ErrorLogFactory errorLogFactory;
        private readonly string dbConnectionString;
        private readonly ICoastalAPISettings coastalAPISettings;
        public CoastalAPIBL(ICoastalAPISettings coastalAPISettings)
        {
            this.coastalAPISettings = coastalAPISettings;
            this.dbConnectionString = coastalAPISettings.GetCoastalAPIDBConnectionString();
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
        }

        public CoastalAPIBL(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
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
                elc.Error.ExceptionMessage = e.InnerException.Message;
                elc.Error.StackTrace = e.StackTrace;

                elc.Status = CoastalAPIModels.ResponseStatus.Fail;

                return elc;
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

            //errorLog.Insert();
        }
    }
}
