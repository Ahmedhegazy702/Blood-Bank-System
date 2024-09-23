using BloodBank.Core.Entites;

namespace BloodBankSystem.Dtos
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string BloodType { get; set; }
      
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }
}
