using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerApp.DTO;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private UserManager<User> _userManager;


        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO model)
        {
                    var user = new User{
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Gender = model.Gender
                    };

                    var result = await _userManager.CreateAsync(user,model.Password);

                    if(result.Succeeded)
                    {
                        return StatusCode(201);
                    }
                    
                return BadRequest(result.Errors);
        }
    }
}