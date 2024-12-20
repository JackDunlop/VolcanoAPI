using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mysqlx.Session;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using VolcanoAPI.Contexts;
using VolcanoAPI.Data;
using VolcanoAPI.Service;
using static System.Net.WebRequestMethods;
namespace VolcanoAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserContext _context;

        private readonly TokenService _tokenService;


        public UserController(UserContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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
            if (_context.Users.Any(u => u.name == name))
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

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new UserData { name = name, password = hashedPassword, email = email, dateOfBirth = dob };
            var result = await RegisterNewUserToDatabase(newUser);
            if (result is not OkResult)
            {
                return result;
            }
            return Ok("Registered");
        }


        [HttpPost("/user/login")]
        public IActionResult LoginUser(string name, string password)
        {
            if (!_context.Users.Any(u => u.name == name))
            {
                return NotFound("Username not in use");
            }
            var retriveHashedPassword = _context.Users.Where(p => p.name == name).Select(p => p.password).FirstOrDefault();
            if (retriveHashedPassword == null || retriveHashedPassword.Length == 0)
            {
                return NotFound("Password not found");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(password, retriveHashedPassword);
            if (!verified)
            {
                return BadRequest($"Password is not correct.");
            }
            // return jwt token
            var token = _tokenService.GenerateToken();
            return Ok(new { message = "Logged-in", token = token });
        }


    }
}
