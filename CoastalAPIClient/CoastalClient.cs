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

        public string FreezeCustomer(RegisterRequest rr)
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

        public string UnfreezeCustomer(RegisterRequest rr)
        {
            string methodname = "api/coastalAPI/UnfreezeCustomer";

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
    }
}
