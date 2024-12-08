using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VolcanoAPI;
using VolcanoAPI.Data;

namespace VolcanoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolcanoController : ControllerBase
    {
        private readonly VolcanoContext _context;

        public VolcanoController(VolcanoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllVolcanoes()
        {
            var volcanoes = _context.Volcanoes.ToList();
            return Ok(volcanoes);
        }

        [HttpGet("{id}")]
        public IActionResult GetVolcanoById(int id)
        {
            var volcano = _context.Volcanoes.FirstOrDefault(v => v.id == id);
            if (volcano == null)
            {
                return NotFound();
            }
            return Ok(volcano);
        }
    }
}
