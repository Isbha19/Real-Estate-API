

using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs.Request.Account
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression(@"^[-'a-zA-Z]+$", ErrorMessage = "First Name cannot contain numbers.")]

        [StringLength(15,MinimumLength =3,ErrorMessage ="First name must be atleast {2},and maximum {1} characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Last name must be atleast {2},and maximum {1} characters")]
        [RegularExpression(@"^[-'a-zA-Z]+$", ErrorMessage = "Last Name cannot contain numbers.")]

        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9!#%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be atleast {2},and maximum {1} characters")]

        public string Password { get; set; }
    }
}
