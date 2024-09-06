using Heist.Core.DTO;
using Heist.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

    // POST /member
    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto)
    {
        _logger.LogInformation("Creating member with data: {MemberDto}", memberDto);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400 Bad Request if the model is invalid
        }

        var result = await _memberService.CreateMemberAsync(memberDto);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors); // 400 Bad Request if creation fails due to validation
        }

        return CreatedAtAction(nameof(CreateMember), new { id = result.MemberId }, result); // 201 Created on success
    }
}
