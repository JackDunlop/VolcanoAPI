using Microsoft.AspNetCore.Mvc;
using Mysqlx.Session;
using System.Linq;
using VolcanoAPI;
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
        public IActionResult GetVolcanoes([FromQuery] string? country)
        {
            if (string.IsNullOrEmpty(country))
            {
                var allVolcanoes = _context.Volcanoes.Select(v => new { v.id, v.name, v.country, v.region, v.subregion }).ToList();
                return Ok(allVolcanoes);
            }

            var volcano = _context.Volcanoes.Where(v => v.country == country).Select(v => new { v.id, v.name, v.country, v.region, v.subregion });
            if (volcano == null)
            {
                return NotFound(new { message = "Volcano not found", errorCode = 404 });
            }
            return Ok(new { data = volcano });
        }

        [HttpGet("/volcanoes/{id}")]
        public IActionResult GetVolcanoById(int id)
        {
            var volcano = _context.Volcanoes.FirstOrDefault(v => v.id == id);
            if (volcano == null)
            {
                return NotFound(new { message = "Volcano not found", errorCode = 404 });
            }
            return Ok(new { data = volcano });
        }  
       
    }
}
