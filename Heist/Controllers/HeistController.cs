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
            const string HEIST_NOT_FOUND = "Heist not found";
            const string HEIST_ALREADY_STARTED = "The heist has already started";
            const string DUPLICATE_SKILLS = "Multiple skills with the same name and level were provided.";
            const string INVALID_SKILL_NAME = "Each skill must have a valid name.";
            const string GENERAL_ERROR = "An error occurred while updating the heist skills.";
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
                HEIST_NOT_FOUND => NotFound(result.Error),
                HEIST_ALREADY_STARTED => StatusCode(405, result.Error),
                DUPLICATE_SKILLS => BadRequest(result.Error),
                INVALID_SKILL_NAME => BadRequest(result.Error),
                _ => StatusCode(500, GENERAL_ERROR)
            };
        }
        [HttpGet("{heistId}/eligible_members")]
        public async Task<IActionResult> GetEligibleMembers(int heistId)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _heistService.GetEligibleMembersAsync(heistId);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error); // Handle errors such as "Heist not found"
            }

            return Ok(result.Value); // Return eligible members if successful
        }
    }
}



