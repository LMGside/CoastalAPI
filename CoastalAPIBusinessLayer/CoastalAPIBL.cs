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
        private readonly WalletFactory walletFactory;
        private readonly string dbConnectionString;
        private readonly ICoastalAPISettings coastalAPISettings;
        public CoastalAPIBL(ICoastalAPISettings coastalAPISettings)
        {
            this.coastalAPISettings = coastalAPISettings;
            this.dbConnectionString = coastalAPISettings.GetCoastalAPIDBConnectionString();
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
            this.walletFactory = new WalletFactory(this.dbConnectionString);
        }

        public CoastalAPIBL(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
            this.customerFactory = new CustomerFactory(this.dbConnectionString);
            this.errorLogFactory = new ErrorLogFactory(this.dbConnectionString);
            this.walletFactory = new WalletFactory(this.dbConnectionString);
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
                fcr.Error.ExceptionMessage = e.InnerException.Message;
                fcr.Error.StackTrace = e.StackTrace;
                fcr.Error.CrashedMethod = "FreezeCustomer in  BL";

                fcr.Status = CoastalAPIModels.ResponseStatus.Fail;

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
                ufcr.Error.ExceptionMessage = e.InnerException.Message;
                ufcr.Error.StackTrace = e.StackTrace;
                ufcr.Error.CrashedMethod = "UnfreezeCustomer in  BL";

                ufcr.Status = CoastalAPIModels.ResponseStatus.Fail;

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
                dcr.Error.ExceptionMessage = e.InnerException.Message;
                dcr.Error.StackTrace = e.StackTrace;
                dcr.Error.CrashedMethod = "DeregisterCustomer in  BL";

                dcr.Status = CoastalAPIModels.ResponseStatus.Fail;

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
                dfr.Error.ExceptionMessage = e.InnerException.Message;
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
                decimal balance = wallet.Get(newCus.ID).Balance;
                if(amount > balance)
                {
                    wr.Error.ErrorMessage = "Customer not Found";
                    wr.Error.CrashedMethod = "DepositFunds";
                    wr.Status = CoastalAPIModels.ResponseStatus.Fail;

                    return wr;
                }
            }
            catch(Exception e)
            {
                BuildAndInsertErrorLog(e, "Error Withdrawing Funds", "WithdrawFunds in BL");

                wr.Error.ErrorMessage = "Error Unfreezing Customer";
                wr.Error.ExceptionMessage = e.InnerException.Message;
                wr.Error.StackTrace = e.StackTrace;
                wr.Error.CrashedMethod = "UnfreezeCustomer in  BL";

                wr.Status = CoastalAPIModels.ResponseStatus.Error;

                return wr;
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
    }
}
