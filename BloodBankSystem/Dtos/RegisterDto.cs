using BloodBank.Core.Entites;
using BloodBank.Core.Entites.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BloodBankSystem.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Phone Number is required.")]
        [RegularExpression(@"^(011|012|010|015)\d{8}$",ErrorMessage ="Phone Number Must be a Valid Egyptian Phone number starting With 011, 012, 010, 015 follwed by 9 digits.")]

        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string City { get; set; }
        public string Country { get; set; }


        public string BloodType { get; set; }



        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage = "Password Must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 Characters")]
        public string Password { get; set; }
               
    }

  
  
}
