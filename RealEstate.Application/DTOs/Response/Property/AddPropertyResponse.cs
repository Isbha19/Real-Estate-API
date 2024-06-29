using RealEstate.Application.DTOs.Request.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Property
{
  
        public record AddPropertyResponse(bool Success, string Message = null);

    
}
