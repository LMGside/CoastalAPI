using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contour.BaseClient;

namespace CoastalAPIClient
{
    public class CoastalClient : BaseClient
    {
        public CoastalClient(string username, string password, string clienturl) : base(username, password, clienturl) { }


    }
}
