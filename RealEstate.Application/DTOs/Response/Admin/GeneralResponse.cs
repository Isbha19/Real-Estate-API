using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public record GeneralResponse(bool Success, string Message=null);
    
}
