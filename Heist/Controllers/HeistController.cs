using Heist.Core.DTO;
using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;


namespace Heist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeistController : ControllerBase
    {
        private readonly IHeistService _heistService;
        private readonly IHeistRepository _heistRepository;
        private readonly ILogger<MemberController> _logger;

        public HeistController(IHeistRepository heistRepository, ILogger<MemberController> logger, IHeistService heistService)
        {
            _heistRepository = heistRepository;
            _logger = logger;
            _heistService = heistService;
        }

        // POST /heist
        [HttpPost]
        public async Task<IActionResult> AddHeist([FromBody] AddHeistDto addHeistDto)
        {
            _logger.LogInformation("Creating new heist with data: {HeistDto}", addHeistDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request if the model is invalid
            }
            try
            {
                var result = await _heistService.CreateHeistAsync(addHeistDto);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Errors); // 400 Bad Request if creation fails due to validation
                }

                return CreatedAtAction(nameof(AddHeist), new { id = result.HeistId }, result); // 201 Created on success
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding the heist.");

                // Return a 400 Bad Request in case of an exception
                return BadRequest(new { message = "An error occurred while processing the request." });
            }

        }
        [HttpPatch("{heistId}/skills")]
        public async Task<IActionResult> UpdateHeistSkills(int heistId, [FromBody] UpdateHeistSkillsDto updateSkillsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _heistService.UpdateHeistSkillsAsync(heistId, updateSkillsDto);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return result.Error switch
            {
                "Heist not found" => NotFound(result.Error),
                "The heist has already started" => StatusCode(405, result.Error),
                "Multiple skills with the same name and level were provided." => BadRequest(result.Error),
                "Each skill must have a valid name." => BadRequest(result.Error),
                _ => StatusCode(500, "An error occurred while updating the heist skills.")
            };
        }
    }
}



