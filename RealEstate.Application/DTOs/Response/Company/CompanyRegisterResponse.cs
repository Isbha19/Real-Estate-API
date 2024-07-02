using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Company
{
   
        public record CompanyRegisterResponse(bool Success,string Message, int id = 0);

    
}
