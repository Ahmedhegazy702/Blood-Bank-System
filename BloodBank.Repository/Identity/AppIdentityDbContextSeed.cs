using BloodBank.Core.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodBank.Core.Entites;


namespace BloodBank.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Hegazy",
                    Email = "ahmed.hegaz@gmail.com",
                    UserName = "ahmed.hegazy",
                    PhoneNumber = "01122334455",
                    City = "Tanta",
                    BloodType ="A+",
                    Country="Egypt",
                    Gender = "Male",
                    BirthDate = new DateTime(2002,10,24)
                    

                    
                };
                await _userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
