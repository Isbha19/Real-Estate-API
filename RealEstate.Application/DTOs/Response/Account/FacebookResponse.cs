using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Account
{
    public class FacebookResponse
    {
        public FacebookData Data { get; set; }
    }
    public class FacebookData
    {
        public bool Is_Valid { get; set; }
        public string User_Id { get; set; }
    }
}
