using Microsoft.AspNetCore.Mvc;
using Mysqlx.Session;
using System.Linq;
using VolcanoAPI.Contexts;
using VolcanoAPI.Data;
using static System.Net.WebRequestMethods;
namespace VolcanoAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

            private readonly UserContext _context;

            public UserController(UserContext context)
            {
                _context = context;
            }

            [HttpGet("/user/register")]
            public IActionResult PostRegisterUser()
            {

                return Ok("Register");
            }   
        
            [HttpGet("/user/login")]
            public IActionResult PostLoginUser()
            {

                return Ok("Login");
            }

        
    }
}
