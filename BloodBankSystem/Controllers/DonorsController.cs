using BloodBank.Core.Entites;
using BloodBank.Core.Entites.Identity;
using BloodBank.Repository.Data;
using BloodBank.Repository.Identity;
using BloodBankSystem.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace BloodBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DonorsController : BaseApiController
    {
       
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly BloodBanKDbContext _dbContext;
      
      





        public DonorsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, BloodBanKDbContext dbContext)
        {
           
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
         
            

        }
        [HttpGet("search")] // api/Donors/Search
        public async Task<ActionResult<IEnumerable<AppUser>>> SearchWithBloodType(string bloodType)
        {

            var users = await _userManager.Users.Where(u => u.BloodType == bloodType).Select(u => new UserDto
            {
                DisplayName = u.DisplayName,
                Email = u.Email,
                BloodType = u.BloodType,
                PhoneNumber = u.PhoneNumber,
                City = u.City,

            }).ToListAsync();

            if (!users.Any())
            {
                return NotFound("BloodType Not Found");
            }

            return Ok(users);

        }

        [HttpPost("requests")]
        public ActionResult CreateRequest([FromBody] RequestModel request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(/*ModelState*/ "Enter Request Correct!");
            }
            var NewRequest = new RequestModel
            {
                City = request.City,
                Hospital = request.Hospital,
                BloodType = request.BloodType,
                Mobile = request.Mobile,
                Note = request.Note
            };
            _dbContext.Requests.Add(NewRequest);
            _dbContext.SaveChanges();
            return Ok(NewRequest);
            
        }
      

      
      


        

    }
}
