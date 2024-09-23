using AutoMapper;
using BloodBank.Core.Entites.Identity;
using BloodBank.Core.Services;
using BloodBankSystem.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RestSharp;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net.Mail;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;

namespace BloodBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _authService;
        private readonly IConfiguration _configuration;

        //private readonly IMapper _mapper;

        public AccountController(IEmailVerificationService emailVerificationService,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService authService, IConfiguration configuration)
        {
            _emailVerificationService = emailVerificationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _configuration = configuration;
            //_mapper = mapper;
        }
        private async Task<ActionResult<bool>> IsEmailValidAsync(string email)
        {
            var Client = new RestClient("https://verify-email.org");
            var request = new RestRequest("api/v1/email", Method.Get);
            request.AddParameter("email", email);
            var response = await Client.ExecuteAsync<EmailResponse>(request);
            if (response.Data != null)
            {
                return response.Data.IsValid;

            }
            else
            {
                return false;
            }
           
        }
        public class EmailResponse
        {
            public bool IsValid { get; set; }
        }


        [HttpPost("login")] // Post : /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest("Invalid payload");

            if(!user.EmailConfirmed)
            {
                return BadRequest("Email needs to be confirmed");

            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded is false)
                return Unauthorized();

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpPost("register")]  // Post : /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(ModelState.IsValid)
            {
                var IsvalidEmail=await _emailVerificationService.IsValidEmailAsync(model.Email);
                if (IsvalidEmail)
                {
                    if (CheckEmailExitst(model.Email).Result.Value)
                        return BadRequest("This Email is Exting befor");

                    var user = new AppUser()
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email, // ahmed.hegazy@gmail.com
                        UserName = model.Email.Split("@")[0],  // ahmed.hegazy
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender,
                        City = model.City,
                        Country = model.Country,
                        BloodType = model.BloodType,
                        BirthDate = model.BirthDate




                    };




                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded is false)
                        return Ok("Please request an email verification link");
                    else
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var Email_body = "Please Confirm Your Email Address <a href=\"#URL#\">Click here <a/>";
                        var callback_Url = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", controller: "Account",
                            values: new { userId = user.Id, code = code });
                        var body = Email_body.Replace(oldValue: "#URL#",
                            newValue: callback_Url);
                        // SEND EMAIL
                        var result02 = SendEmail(body, user.Email);
                        if (result02)
                            return Ok("Please verify your email, through rhe verification email ");

                        


                    }
                     

                    //return Ok(new UserDto()
                    //{
                    //    DisplayName = user.DisplayName,
                    //    Email = user.Email,
                    //    Token = await _authService.CreateTokenAsync(user, _userManager)
                    //});


                }
            }
            else
            {
                ModelState.AddModelError("Email", "The Provided EmailAddress is not Valid.");
            }
            return Ok(model);
 
            
            //if (CheckEmailExitst(model.Email).Result.Value)
            //    return BadRequest("This Email is Exting befor");


            //if (IsEmailValidAsync(model.Email).Result.Value is true)
            //{


            //    var user = new AppUser()
            //    {
            //        DisplayName = model.DisplayName,
            //        Email = model.Email, // ahmed.hegazy@gmail.com
            //        UserName = model.Email.Split("@")[0],  // ahmed.hegazy
            //        PhoneNumber = model.PhoneNumber,
            //        Gender = model.Gender,
            //        City = model.City,
            //        Country = model.Country,
            //        BloodType = model.BloodType,
            //        BirthDate = model.BirthDate




            //    };
            //    var result = await _userManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded is false)
            //        return BadRequest("There is Error ");

            //    return Ok(new UserDto()
            //    {
            //        DisplayName = user.DisplayName,
            //        Email = user.Email,
            //        Token = await _authService.CreateTokenAsync(user, _userManager)
            //    });







            //}
            //else
            //{
            //    return BadRequest("There is Eroor With Email");
            //}






            //var user = new AppUser()
            //{
            //    DisplayName = model.DisplayName,
            //    Email = model.Email, // ahmed.hegazy@gmail.com
            //    UserName = model.Email.Split("@")[0],  // ahmed.hegazy
            //    PhoneNumber = model.PhoneNumber,
            //    Gender = model.Gender,
            //    City = model.City,
            //    Country = model.Country,
            //    BloodType = model.BloodType,
            //    BirthDate = model.BirthDate




            //};




            //var result = await _userManager.CreateAsync(user, model.Password);

            //if (result.Succeeded is false)
            //    return BadRequest("There is Error");
            //New
            //string verificationToken = GenerateVerificationToken();
            //await SendVerificationEmail(model.Email, verificationToken);
            //





            //return Ok(new UserDto()
            //{
            //    DisplayName = user.DisplayName,
            //    Email = user.Email,
            //    Token = await _authService.CreateTokenAsync(user, _userManager)
            //});



        }
        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult>ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest("Invalid email confirmation Url");
               
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("Invalid email parameter");
            }
            //code = Encoding.UTF8.GetString(bytes:Convert.FromBase64String(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status = result.Succeeded ? "Thank You for confirming mail" : "Your email is not confirmed, please try again later";
            return Ok(status);
            
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]  // GET : /api/account/GetCurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                City = user.City,
                BloodType= user.BloodType,
                PhoneNumber=user.PhoneNumber
                
            });
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>>CheckEmailExitst(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return false;
            else
                return true;
        }

        //[HttpPost("resetpassword-token")]  // Post : /api/account/resetpassword-token
        //public async Task<IActionResult> ResetPasswordToken(ResetPasswordDto resetPassword)
        //{
        //    var user = await _userManager.FindByNameAsync(resetPassword.UserName);
        //    if (user == null) 
        //    {
        //        return NotFound();

        //    } 
        //    var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    return Ok(new {Token = Token});
        //}




        [HttpPost("resetpassword")]  // Post : /api/account/resetpassword
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByNameAsync(resetPassword.UserName);
            if (user == null)
            {
                return NotFound();

            }
            if (string.IsNullOrEmpty(resetPassword.Token))
            {
                return NotFound();
            }
            var result = await _userManager.ResetPasswordAsync(user,resetPassword.Token,resetPassword.NewPassword);
            if(!result.Succeeded)
            {
                var errors=new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return StatusCode(StatusCodes.Status500InternalServerError );
            }
            return Ok(result);



        }
        private bool SendEmail(string body, string email)
        {
            //Create Client
            var options = new RestClientOptions(baseUrl: "https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator(username: "api", password: _configuration.GetSection(key: "EmailConfig:API_KEY").Value)
            };
            var client = new RestClient(options);
            var request = new RestRequest(resource:"", Method.Post);
           
            request.AddParameter(name: "domain", value: "sandbox11275ed1ae0e45b8b0ee3306e822833d.mailgun.org");
            request.Resource = "{domain}/messages";
            request.AddParameter(name: "from", value: "Mailgun <postmaster@sandbox11275ed1ae0e45b8b0ee3306e822833d.mailgun.org");
            request.AddParameter(name: "to", value: "email");
            request.AddParameter(name: "sublect", value: "Email Verification");
            request.AddParameter(name: "text", value: "body");
            request.Method = Method.Post;

            var response = client.Execute(request);

            return response.IsSuccessful;


        }

        //[HttpPost("send verification email")]
        //public async Task<IActionResult> SendVerificationEmail(string email, string verificationToken)
        //{
        //    var client = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("your-emaik@gmail.com", "your-password"),
        //        EnableSsl = true
        //    };
        //    var verificationUrl = Url.Action("VerifyEmail", "Account", new { verificationToken }, Request.Scheme);
        //    var MailMessage = new MailMessage
        //    {
        //        From = new MailAddress("your-email@gmail.com"),
        //        Subject = "Verify Your email",
        //        Body = $"Please click this link to verify your email:{verificationUrl}/{verificationToken}",
        //        IsBodyHtml = false
        //    };
        //    MailMessage.To.Add(email);
        //    await client.SendMailAsync(MailMessage);

        //    return Ok();

        //}
        //private string GenerateVerificationToken()
        //{
        //    var TokenHandler = new JwtSecurityTokenHandler();
        //    var Key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name,"verification")
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = TokenHandler.CreateToken(tokenDescriptor);
        //    return TokenHandler.WriteToken(token);


        //}

        //[HttpGet("verify email/{verificationToken}")]
        //public async Task<IActionResult> VerifyEmail(string verificationToken)
        //{
        //    var  User = await _userManager.Users.FirstOrDefaultAsync(U=>U. == verificationToken);
        //}

        //private void CreateHttpOnlyCookies(HttpResponse response,string token)
        //{
        //    var cookiesOptions = new CookiesOptions()
        //    {
        //        HttpOnly = true,
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict
        //    };
        //    response.Cookies.Append("jwt",token,cookiesOptions);
        //}
        [HttpPut("editprofile")]
        public async Task<ActionResult<AppUser>> EditProfile(AppUser user)
        {
            var existinfProfile = await _userManager.Users.FirstOrDefaultAsync(U=> U.Email == user.Email);
            if (existinfProfile == null)
            {
                return NotFound();
            }
            existinfProfile.DisplayName = user.DisplayName;
            existinfProfile.City = user.City;
            existinfProfile.Country = user.Country;
            existinfProfile.BloodType = user.BloodType;
            existinfProfile.Gender = user.Gender;
            existinfProfile.PhoneNumber = user.PhoneNumber;
            
            return Ok(user);
        }


    }
}
