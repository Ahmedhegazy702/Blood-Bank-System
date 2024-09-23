
using BloodBank.Core.Entites;
using BloodBank.Core.Entites.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Repository.Data.Config
{
    public class GenderConfigurations : IEntityTypeConfiguration<AppUser>
    {
        //public void Configure(EntityTypeBuilder<AppUser> builder)
        //{
        //   builder.Property(G=>G.GenderName)
        //        .HasConversion(GName=>GName.ToString()
        //        ,GName=>(Gender)Enum.Parse(typeof(Gender),GName));


        //    builder.Property(B=>B.BloodTypeName)
        //        .HasConversion(BName=>BName.ToString()
        //        ,BName=>(BloodType)Enum.Parse(typeof (BloodType),BName));
        //}
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            throw new NotImplementedException();
        }
    }
}
