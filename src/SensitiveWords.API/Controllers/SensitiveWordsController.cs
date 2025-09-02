using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Application.Interfaces;
using System.Net;

namespace SensitiveWordsClean.API.Controllers;

/// <summary>
/// Controller for managing sensitive words in the system
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SensitiveWordsController : ControllerBase
{
    private readonly ISensitiveWordService _sensitiveWordService;
    private readonly ILogger<SensitiveWordsController> _logger;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Initializes a new instance of the SensitiveWordsController
    /// </summary>
    /// <param name="sensitiveWordService">Service for sensitive word operations</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="cache">Memory cache instance</param>
    public SensitiveWordsController(ISensitiveWordService sensitiveWordService, ILogger<SensitiveWordsController> logger, IMemoryCache cache)
    {
        _sensitiveWordService = sensitiveWordService ?? throw new ArgumentNullException(nameof(_sensitiveWordService));
        _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(_cache));
    }

    /// <summary>
    /// Retrieves all sensitive words from the system
    /// </summary>
    /// <returns>A list of all sensitive words</returns>
    /// <response code="200">Returns the list of sensitive words</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SensitiveWordDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IEnumerable<SensitiveWordDto>>> GetAll()
    {
        const string cacheKey = "sensitive-words-all";
        
        if (_cache.TryGetValue(cacheKey, out IEnumerable<SensitiveWordDto>? cachedWords))
        {
            return Ok(cachedWords);
        }

        try
        {
            var words = await _sensitiveWordService.GetAllAsync();
            _cache.Set(cacheKey, words, _cacheExpiration);
            return Ok(words);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all sensitive words");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves a specific sensitive word by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word</param>
    /// <returns>The sensitive word with the specified ID</returns>
    /// <response code="200">Returns the sensitive word</response>
    /// <response code="404">If the sensitive word is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SensitiveWordDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<SensitiveWordDto>> GetById(int id)
    {
        string cacheKey = $"sensitive-word-{id}";
        
        if (_cache.TryGetValue(cacheKey, out SensitiveWordDto? cachedWord))
        {
            return Ok(cachedWord);
        }

        try
        {
            var word = await _sensitiveWordService.GetByIdAsync(id);
            if (word == null)
                return NotFound($"Sensitive word with ID {id} not found");

            _cache.Set(cacheKey, word, _cacheExpiration);
            return Ok(word);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sensitive word with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Creates a new sensitive word in the system
    /// </summary>
    /// <param name="createDto">The sensitive word data to create</param>
    /// <returns>The created sensitive word</returns>
    /// <response code="201">Returns the newly created sensitive word</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="409">If the sensitive word already exists</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(SensitiveWordDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<SensitiveWordDto>> Create([FromBody] CreateSensitiveWordDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdWord = await _sensitiveWordService.CreateAsync(createDto);
            
            ClearCache();
            
            return CreatedAtAction(nameof(GetById), new { id = createdWord.Id }, createdWord);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Attempt to create duplicate sensitive word: {Word}", createDto.Word);
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sensitive word");
            return StatusCode(500, new { message = "An error occurred while creating the sensitive word" });
        }
    }

    /// <summary>
    /// Updates an existing sensitive word
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to update</param>
    /// <param name="updateDto">The updated sensitive word data</param>
    /// <returns>The updated sensitive word</returns>
    /// <response code="200">Returns the updated sensitive word</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the sensitive word is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SensitiveWordDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<SensitiveWordDto>> Update(int id, [FromBody] UpdateSensitiveWordDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedWord = await _sensitiveWordService.UpdateAsync(id, updateDto);
            
            ClearCache();
            
            return Ok(updatedWord);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Attempt to update non-existent sensitive word with ID {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sensitive word with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the sensitive word" });
        }
    }

    /// <summary>
    /// Deletes a sensitive word from the system
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the sensitive word was successfully deleted</response>
    /// <response code="404">If the sensitive word is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _sensitiveWordService.DeleteAsync(id);
            
            ClearCache();
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Attempt to delete non-existent sensitive word with ID {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sensitive word with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the sensitive word" });
        }
    }

    private void ClearCache()
    {
        _cache.Remove("sensitive-words-all");
        _cache.Remove("sensitive-words-active");
    }
}
