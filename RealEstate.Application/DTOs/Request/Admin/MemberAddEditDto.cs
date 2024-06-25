using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Request.Admin
{
    public class MemberAddEditDto
    {
        public string Id { get; set; }
        [RegularExpression(@"^[A-Za-z0-9!#%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?$", ErrorMessage = "Invalid Email Address")]
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[-'a-zA-Z]+$", ErrorMessage = "First Name cannot contain numbers.")]

        [StringLength(15, MinimumLength = 3, ErrorMessage = "First name must be atleast {2},and maximum {1} characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be atleast {2},and maximum {1} characters")]
        [RegularExpression(@"^[-'a-zA-Z]+$", ErrorMessage = "Last Name cannot contain numbers.")]
        public string LastName { get; set; }
        public string Password { get; set; }
        [Required]
        //eg:"Admin,Player,Manager"
        public string Roles { get; set; }
    }
}
