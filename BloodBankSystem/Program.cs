using BloodBank.Core.Entites.Identity;
using BloodBank.Core.Services;
using BloodBank.Repository.Data;
using BloodBank.Repository.Identity;
using BloodBank.Service;
using BloodBankSystem.Extenstion;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace BloodBankSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services
            //New
           


            // Add services to the Dependency Injection container.

         
            // Register Required Web APIs Services To The Dependancy injection Container



            webApplicationBuilder.Services.AddSwaggerServices();

            webApplicationBuilder.Services.AddDbContext<BloodBanKDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //webApplicationBuilder.Services.AddDbContext<AppUsersDbContext>(options =>
            //{
            //    options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("AppUsersConnection"));
            //});

            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //NN
            //webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            //{
            //    var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
            //    return ConnectionMultiplexer.Connect(connection);
            //});

            //ApplicationServicesExtension.AddApplicationServices(webApplicationBuilder.Services);
            webApplicationBuilder.Services.AddSwaggerServices();

            //New
            
            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);
            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddHttpClient<IEmailVerificationService, MailboxEmailVerification>();
            webApplicationBuilder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthSection = webApplicationBuilder.Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = webApplicationBuilder.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = webApplicationBuilder.Configuration["Authentication:Facebook:AppSecret"];
                });












            //webApplicationBuilder.Services.AddCors(corsOptions =>
            //{
            //    corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
            //    {
            //        corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins(webApplicationBuilder.Configuration["FrontBaseUrl"]);
            //    });
            //});

            #endregion

            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            //var _dbContext = services.GetRequiredService<StoreContext>();
            // Ask CLR For Creating Object From DbContext Explicitly

            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                //await _dbContext.Database.MigrateAsync(); // Update Database
                //await StoreContextSeed.SeedAsync(_dbContext); // Data Seeding

                await _identityDbContext.Database.MigrateAsync(); // Update Database

                var _userManager = services.GetRequiredService<UserManager<AppUser>>(); // Explicitly
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Has Been Occured During Apply The Migration");

            }

            #region Configure Kestrel Middlewares
            //NNNN

            //app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }
            app.UseSwaggerMiddleWares();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseStaticFiles();


            //app.UseCors("MyPolicy");
            //app.UseHttpsRedirection();
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.Run();

        }
    }
}