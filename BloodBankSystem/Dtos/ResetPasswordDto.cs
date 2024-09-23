using System.ComponentModel.DataAnnotations;

namespace BloodBankSystem.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage ="Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "CurrentPassword is Required")]

        public string NewPassword { get; set; }
        //[Required(ErrorMessage = "ConfirmPassword is Required")]
        [Required,Compare("NewPassword")]
 
        public string ConfirmPassword { get; set; }

        public string Token { get; set; } 


    }
}
