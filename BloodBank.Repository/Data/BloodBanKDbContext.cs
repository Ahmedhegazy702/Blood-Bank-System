using BloodBank.Core.Entites;
using BloodBank.Core.Entites.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Repository.Data
{
    public class BloodBanKDbContext:DbContext
    {
        public BloodBanKDbContext(DbContextOptions<BloodBanKDbContext>options):base(options)
        {
            
        }
        public DbSet<RequestModel> Requests { get; set; }
        //public DbSet<AppUser>AppUsers { get; set; }
    }
}
