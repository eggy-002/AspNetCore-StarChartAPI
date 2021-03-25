using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject == null)
                return NotFound();

            if (celestialObject.Satellites == null)
            {
                celestialObject.Satellites = new List<CelestialObject>();
            }

            celestialObject.Satellites.Add(celestialObject);


            _context.SaveChanges();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (celestialObjects.Count == 0)
                return NotFound();

            foreach(var celestialObject in celestialObjects)
            {
                if(celestialObject.Satellites == null)
                {
                    celestialObject.Satellites = new List<CelestialObject>();
                }

                celestialObject.Satellites.Add(celestialObject);
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                if (celestialObject.Satellites == null)
                {
                    celestialObject.Satellites = new List<CelestialObject>();
                }

                celestialObject.Satellites.Add(celestialObject);
            }

            return Ok(celestialObjects);
        }
    }
}
