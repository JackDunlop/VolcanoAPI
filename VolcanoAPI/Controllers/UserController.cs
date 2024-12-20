using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Session;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
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

            private async Task<IActionResult> RegisterNewUserToDatabase(UserData newUser)
            {
                try
                {
                    _context.Users.Add(new UserData { name = newUser.name, password = newUser.password, email = newUser.email, dateOfBirth = newUser.dateOfBirth });
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "A database error occurred: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred: " + ex.Message);
                }
            }

            [HttpPost("/user/register")]
            public async Task<IActionResult> RegisterUser(string name, string password, string email, string dateOfBirth)
            {
                    if(_context.Users.Any(u => u.name == name))
                    {
                        return BadRequest("Username already in use");
                    }
           
                    if (_context.Users.Any(u => u.email == email))
                    {
                        return BadRequest("Email already in use");
                    }
                    if (!DateTime.TryParse(dateOfBirth, out DateTime dob))
                    {
                        return BadRequest("Invalid date of birth");
                    }

                    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password!,salt: salt,prf: KeyDerivationPrf.HMACSHA256,iterationCount: 100000, numBytesRequested: 256 / 8));
                    var newUser = new UserData { name = name, password = password, email = email, dateOfBirth = dob};
                    var result = await RegisterNewUserToDatabase(newUser);
                    if (result is not OkResult)
                    {
                        return result;
                    }
                    return Ok("Registered");
            }   
        
            [HttpPost("/user/login")]
            public async Task<IActionResult> LoginUser(string name, string password)
            {

                return Ok("Login");
            }

        
    }
}
