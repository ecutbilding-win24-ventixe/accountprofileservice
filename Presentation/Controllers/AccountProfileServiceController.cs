using Business.Interfaces;
using Business.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountProfileServiceController(IProfileService profileService) : ControllerBase
{
    private readonly IProfileService _profileService = profileService;

    [HttpPost("create-profile")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateAccountProfileRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null.");

        var result = await _profileService.CreateProfileAsync(request);
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Message);
        return Ok(result);
    }
}
