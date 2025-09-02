using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Application.Interfaces;
using System.Net;

namespace SensitiveWordsClean.API.Controllers;

/// <summary>
/// Controller for text sanitization operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TextSanitizationController : ControllerBase
{
    private readonly ISensitiveWordService _sensitiveWordService;
    private readonly ILogger<TextSanitizationController> _logger;

    /// <summary>
    /// Initializes a new instance of the TextSanitizationController
    /// </summary>
    /// <param name="sensitiveWordService">Service for sensitive word operations</param>
    /// <param name="logger">Logger instance</param>
    public TextSanitizationController(ISensitiveWordService sensitiveWordService, ILogger<TextSanitizationController> logger)
    {
        _sensitiveWordService = sensitiveWordService ?? throw new ArgumentNullException(nameof(_sensitiveWordService));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
    }

    /// <summary>
    /// Sanitizes the provided text by detecting and replacing sensitive words
    /// </summary>
    /// <param name="request">The text sanitization request containing the text to sanitize</param>
    /// <returns>The sanitization result with original text, sanitized text, and detected words</returns>
    /// <response code="200">Returns the sanitization result</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("sanitize")]
    [ProducesResponseType(typeof(TextSanitizationResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<TextSanitizationResponseDto>> SanitizeText([FromBody] TextSanitizationRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _sensitiveWordService.SanitizeTextAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sanitizing text");
            return StatusCode(500, new { message = "An error occurred while sanitizing the text" });
        }
    }
}
