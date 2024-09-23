using BloodBank.Core.Entites.Identity;
using BloodBank.Core.Services;
using BloodBank.Repository.Identity;
using BloodBank.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BloodBankSystem.Extenstion
{
    public static class IdentityServicesExtenstion
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddAuthentication();
            


            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();/*(options =>*/
            //{
            //    //options.Password.RequiredUniqueChars = 2;
            //    //options.Password.RequireNonAlphanumeric = true;
            //    //options.Password.RequireUppercase = true;
            //    //options.Password.RequireLowercase = true;
            //}).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {


                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"])),
                        //claims: authClaims,
                       

                    };
                });
                //.AddJwtBearer("Bearer02", options =>
                //{

                //});

            return services;
        }
    }
}
