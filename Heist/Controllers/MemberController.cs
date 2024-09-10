using Heist.Core.DTO;
using Heist.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ILogger<MemberController> _logger;

    public MemberController(IMemberService memberService, ILogger<MemberController> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto)
    {
        _logger.LogInformation("Creating member with data: {MemberDto}", memberDto);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 Bad Request if the model is invalid
        }
        try
        {
            var result = await _memberService.CreateMemberAsync(memberDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors); // 400 Bad Request if creation fails due to validation
            }

            return CreatedAtAction(nameof(CreateMember), new { id = result.MemberId }, null); // 201 Created on success
        }
        catch { return BadRequest(); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemberById(int id)
    {
        var member = await _memberService.GetMemberByIdAsync(id);
        if (member == null)
        {
            return NotFound(new { message = "Member not found." });
        }

        return Ok(member);
    }

    [HttpPut("{id}/skills")]
    public async Task<IActionResult> UpdateMemberSkills(int id, [FromBody] UpdateMemberSkillDto updateMemberSkillDto)
    {
        _logger.LogInformation("Updating member {MemberId} skills", id);

        var result = await _memberService.UpdateMemberSkillsAsync(id, updateMemberSkillDto);


        if (!result.IsSuccess)
        {
            if (result.Error.Equals("Member not found"))
            {
                return NotFound();
            }
            return BadRequest(result.Error); // 400 Bad Request if update fails
        }

        return NoContent(); // 204 No Content on success
    }

    [HttpDelete("{memberId}/skills/{skillName}")]
    public async Task<IActionResult> DeleteSkill(int memberId, string skillName)
    {
        var result = await _memberService.RemoveSkillAsync(memberId, skillName);

        if (result)
        {
            return NoContent();  // 204 No Content
        }

        return NotFound();  // 404 Not Found
    }
}
