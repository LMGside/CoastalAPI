using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contour.BaseClient;
using CoastalAPIModels.Models;
using Newtonsoft.Json;

namespace CoastalAPIClient
{
    public class CoastalClient : BaseClient
    {
        public CoastalClient(string username, string password, string clienturl) : base(username, password, clienturl) { }

        public RegisterResponse InsertCustomer(RegisterRequest rr)
        {
            string methodname = "api/coastalAPI/InsertCustomer";

            try
            {
                string response = PerformPostOperation(methodname, rr);

                return JsonConvert.DeserializeObject<RegisterResponse>(response);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public FreezeCustomerResponse FreezeCustomer(FreezeCustomerRequest rr)
        {
            string methodname = "api/coastalAPI/FreezeCustomer";

            try
            {
                string response = PerformPostOperation(methodname, rr);

                return JsonConvert.DeserializeObject<FreezeCustomerResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UnfreezeCustomerResponse UnfreezeCustomer(UnfreezeCustomerRequest ucr)
        {
            string methodname = "api/coastalAPI/UnfreezeCustomer";

            try
            {
                string response = PerformPostOperation(methodname, ucr);

                return JsonConvert.DeserializeObject<UnfreezeCustomerResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string DeregisterCustomer(DeregisterCustomerRequest dcr)
        {
            string methodname = "api/coastalAPI/Deregister";

            try
            {
                string response = PerformPostOperation(methodname, dcr);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DepositFundsResponse DepositFunds(DepositFundsRequest dfr)
        {
            string methodname = "api/coastalAPI/Deposit";

            try
            {
                string response = PerformPostOperation(methodname, dfr);

                return JsonConvert.DeserializeObject<DepositFundsResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public WithdrawResponse WithdrawFunds(WithdrawRequest wr)
        {
            string methodname = "api/coastalAPI/Withdraw";

            try
            {
                string response = PerformPostOperation(methodname, wr);

                return JsonConvert.DeserializeObject<WithdrawResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public BuyAssetResponse BuyAssets(BuyAssetRequest bar)
        {
            string methodname = "api/coastalAPI/BuyAsset";

            try
            {
                string response = PerformPostOperation(methodname, bar);

                return JsonConvert.DeserializeObject<BuyAssetResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ReviewTransactionResponse ReviewTransaction(ReviewTransactionRequest rtr)
        {
            string methodname = "api/coastalAPI/Review";

            try
            {
                string response = PerformPostOperation(methodname, rtr);

                return JsonConvert.DeserializeObject<ReviewTransactionResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DayTransactionResponse DayTransactions(DayTransactionRequest dtr)
        {
            string methodname = "api/coastalAPI/TodayTransactions";

            try
            {
                string response = PerformPostOperation(methodname, dtr);

                return JsonConvert.DeserializeObject<DayTransactionResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UserTransactionResponse UserTransactions(UserTransactionRequest utr)
        {
            string methodname = "api/coastalAPI/UserTransactions";

            try
            {
                string response = PerformPostOperation(methodname, utr);

                return JsonConvert.DeserializeObject<UserTransactionResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DateRangeTransactionsResponse DateRangeTransactions(DateRangeTransactionsRequest drtr)
        {
            string methodname = "api/coastalAPI/DateRangeTransactions";

            try
            {
                string response = PerformPostOperation(methodname, drtr);

                return JsonConvert.DeserializeObject<DateRangeTransactionsResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UserTransactionResponse SuccessfulTransactions(UserTransactionRequest user)
        {
            string methodname = "api/coastalAPI/SuccessfulTransactions";

            try
            {
                string response = PerformPostOperation(methodname, user);

                return JsonConvert.DeserializeObject<UserTransactionResponse>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
