using Microsoft.AspNetCore.Mvc;
using Mysqlx.Session;
using System.Linq;
using VolcanoAPI.Contexts;
using VolcanoAPI.Data;
using static System.Net.WebRequestMethods;

namespace VolcanoAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly VolcanoContext _context;

        public CountriesController(VolcanoContext context)
        {
            _context = context;
        }

        
        [HttpGet("/countries")]
        public IActionResult GetCountries()
        {
            var countries = _context.Volcanoes.Select(v => v.country).Distinct().OrderBy(country => country).ToList();
            if (countries == null)
            {
                return NotFound();
            }
            return Ok(countries);
        }
    }
}
