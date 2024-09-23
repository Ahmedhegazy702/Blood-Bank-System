using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Entites.Identity
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        //public string GenderName { get; set; }
        public string BloodType { get; set; }
        public string PhoneNumber {  get; set; } //New
        public string Gender { get; set; } /*= Gender.Male*/       
       
        public DateTime BirthDate { get; set; }
        //New -- Not Add to Db Yet
        //public int weightInKgs { get; set; }
        //public bool TattoInLast12Month { get; set; }
        //public bool HivePositive { get; set; }
       


    }
  
}
