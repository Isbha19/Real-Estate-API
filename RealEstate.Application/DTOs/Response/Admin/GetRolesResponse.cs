using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public class GetRolesResponse
    {
        public IEnumerable<string> Roles { get; set; }
        public bool Success { get; set; }
    }
}
