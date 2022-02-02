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

        public string InsertCustomer(RegisterRequest rr)
        {
            string methodname = "api/coastalAPI/InsertCustomer";

            try
            {
                string response = PerformPostOperation(methodname, rr);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public string FreezeCustomer(FreezeCustomerRequest rr)
        {
            string methodname = "api/coastalAPI/FreezeCustomer";

            try
            {
                string response = PerformPostOperation(methodname, rr);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UnfreezeCustomer(UnfreezeCustomerRequest ucr)
        {
            string methodname = "api/coastalAPI/UnfreezeCustomer";

            try
            {
                string response = PerformPostOperation(methodname, ucr);

                return JsonConvert.DeserializeObject<string>(response);
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

        public string DepositFunds(DepositFundsRequest dfr)
        {
            string methodname = "api/coastalAPI/Deposit";

            try
            {
                string response = PerformPostOperation(methodname, dfr);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string WithdrawFunds(WithdrawRequest wr)
        {
            string methodname = "api/coastalAPI/Withdraw";

            try
            {
                string response = PerformPostOperation(methodname, wr);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BuyAssets(BuyAssetRequest bar)
        {
            string methodname = "api/coastalAPI/BuyAsset";

            try
            {
                string response = PerformPostOperation(methodname, bar);

                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
