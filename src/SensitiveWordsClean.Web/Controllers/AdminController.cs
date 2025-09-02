using Microsoft.AspNetCore.Mvc;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Web.Models;
using System.Text;
using System.Text.Json;

namespace SensitiveWordsClean.Web.Controllers;

public class AdminController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IHttpClientFactory httpClientFactory, ILogger<AdminController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("SensitiveWordsAPI");
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var response = await _httpClient.GetAsync("sensitivewords");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var words = JsonSerializer.Deserialize<List<SensitiveWordDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(words ?? new List<SensitiveWordDto>());
            }
            else
            {
                TempData["Error"] = $"API returned error: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sensitive words from API");
            TempData["Error"] = "Unable to connect to API service. Please ensure the API is running.";
        }

        return View(new List<SensitiveWordDto>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateSensitiveWordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSensitiveWordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide a valid word.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var dto = new CreateSensitiveWordDto
            {
                Word = model.Word
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("sensitivewords", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Sensitive word created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = $"Failed to create sensitive word: {response.StatusCode}";
            }
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to connect to API service.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateSensitiveWordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide a valid word.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var dto = new UpdateSensitiveWordDto
            {
                Word = model.Word
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"sensitivewords/{model.Id}", content);
            
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Word updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = $"Failed to update word: {response.StatusCode}";
            }
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to connect to API service.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"sensitivewords/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Sensitive word deleted successfully!";
            }
            else
            {
                TempData["Error"] = $"Failed to delete sensitive word: {response.StatusCode}";
            }
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to connect to API service.";
        }

        return RedirectToAction(nameof(Index));
    }
}
