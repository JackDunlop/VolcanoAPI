using Microsoft.AspNetCore.Mvc;
using Mysqlx.Session;
using System.Linq;
using VolcanoAPI.Contexts;
using VolcanoAPI.Data;
using static System.Net.WebRequestMethods;

namespace VolcanoAPI.Controllers
{
    //      GET /countries
    //      GET /volcanoes
    //      GET /volcano/{id}

    [ApiController]
    public class VolcanoController : ControllerBase
    {
        private readonly VolcanoContext _context;

        public VolcanoController(VolcanoContext context)
        {
            _context = context;
        }

        [HttpGet("/volcanoes")]
        public IActionResult GetVolcanoes([FromQuery] string? country, string? populatedWithin)
        {
            if (string.IsNullOrEmpty(country))
            {
                var allVolcanoes = _context.Volcanoes.Select(v => new { v.id, v.name, v.country, v.region, v.subregion }).ToList();
                return Ok(allVolcanoes);
            }

            if (string.IsNullOrEmpty(populatedWithin))
            {
                var volcanos = _context.Volcanoes.Where(v => v.country == country).Select(v => new { v.id, v.name, v.country, v.region, v.subregion }).ToList();
                if (volcanos.Count() == 0)
                {
                    return NotFound(new { message = "Country not found", errorCode = 404 });
                }
                return Ok(new { data = volcanos });
            }


            if (populatedWithin is not ("5km" or "10km" or "30km" or "100km"))
            {
                return BadRequest($"Invalid population radius: {populatedWithin}");
            }

            var populationRadius = populatedWithin switch
            {
                "5km" => _context.Volcanoes.Where(v => v.country == country).Select(v => new VolcanoData { id = v.id, name = v.name, country = v.country, region = v.region, subregion = v.subregion, population_5km = v.population_5km }).ToList(),
                "10km" => _context.Volcanoes.Where(v => v.country == country).Select(v => new VolcanoData { id = v.id, name = v.name, country = v.country, region = v.region, subregion = v.subregion, population_5km = v.population_5km, population_10km = v.population_10km }).ToList(),
                "30km" => _context.Volcanoes.Where(v => v.country == country).Select(v => new VolcanoData { id = v.id, name = v.name, country = v.country, region = v.region, subregion = v.subregion, population_5km = v.population_5km, population_10km = v.population_10km, population_30km = v.population_30km }).ToList(),
                "100km" => _context.Volcanoes.Where(v => v.country == country).Select(v => new VolcanoData { id = v.id, name = v.name, country = v.country, region = v.region, subregion = v.subregion, population_5km = v.population_5km, population_10km = v.population_10km, population_30km = v.population_30km, population_100km = v.population_100km }).ToList(),
                _ => null
            };
          
            if (populationRadius == null)
            {
                return NotFound("Volcano not found with that inputted population radius");
            }
            return Ok(populationRadius);
        }

        [HttpGet("/volcanoes/{id}")]
        public IActionResult GetVolcanoById(int id)
        {
            var volcano = _context.Volcanoes.Where(v => v.id == id).Select(v => new { v.id, v.name, v.country, v.region, v.subregion, v.last_eruption,v.summit, v.elevation, v.latitude, v.longitude }).ToList();
            if (volcano == null)
            {
                return NotFound(new { message = "Volcano not found", errorCode = 404 });
            }
            return Ok(volcano);
        }  
       
    }
}
