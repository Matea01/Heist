using Heist.Core.DTO;
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Heist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeistController : ControllerBase
    {
        private readonly IHeistRepository _heistRepository;

        public HeistController(IHeistRepository heistRepository)
        {
            _heistRepository = heistRepository;
        }

        // POST /heist
        [HttpPost]
        public async Task<IActionResult> AddHeist([FromBody] AddHeistDto addHeistDto)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check for unique heist name
            if (await _heistRepository.HeistExistsAsync(addHeistDto.name))
            {
                return BadRequest(new { message = "A heist with the same name already exists." });
            }

            // Create a new Heist object from the DTO
            var heist = new HeistEntity
            {
                Name = addHeistDto.name,
                Location = addHeistDto.location,
                StartTime = addHeistDto.startTime,
                EndTime = addHeistDto.endTime,
                Skills = addHeistDto.skills.Select(skillDto => new Skill
                {
                    Name = skillDto.name,
                    Level = skillDto.level,
                }).ToList()
            };

            // Save the new heist
            await _heistRepository.AddHeistAsync(heist);

            // Return 201 Created with Location header pointing to the new heist
            return CreatedAtAction(nameof(GetHeistById), new { id = heist.Id }, null);
        }

        // Example: GET /heist/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHeistById(int id)
        {
            var heist = await _heistRepository.GetHeistByIdAsync(id);
            if (heist == null)
            {
                return NotFound();
            }

            return Ok(heist);
        }
    }
}
