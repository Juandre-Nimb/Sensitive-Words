using Microsoft.AspNetCore.Mvc;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Web.Models;
using System.Text;
using System.Text.Json;

namespace SensitiveWordsClean.Web.Controllers;

public class ChatController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IHttpClientFactory httpClientFactory, ILogger<ChatController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("SensitiveWordsAPI");
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new ChatViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(ChatViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserMessage))
        {
            return View("Index", model);
        }

        try
        {
            var request = new TextSanitizationRequestDto
            {
                Text = model.UserMessage,
                ReplacementCharacter = "*"
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("textsanitization/sanitize", content);
            if (response.IsSuccessStatusCode)
            {
                SendMessage(response, model);
            }
            else
            {
                model.Messages.Add(new ChatMessage
                {
                    IsUser = false,
                    Message = "Sorry, I'm having trouble processing your message right now.",
                    Timestamp = DateTime.Now
                });
            }
        }
        catch (Exception)
        {
            model.Messages.Add(new ChatMessage
            {
                IsUser = false,
                Message = "Sorry, I'm unable to connect to the service right now.",
                Timestamp = DateTime.Now
            });
        }

        return View("Index", model);
    }

    private static async void SendMessage(HttpResponseMessage response, ChatViewModel model)
    {
        var responseJson = await response.Content.ReadAsStringAsync();
        var sanitizationResult = JsonSerializer.Deserialize<TextSanitizationResponseDto>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (sanitizationResult != null)
        {
            model.Messages.Add(new ChatMessage
            {
                IsUser = true,
                Message = model.UserMessage,
                Timestamp = DateTime.Now
            });

            // Simulate a bot response
            var botResponse = $"I received your message: \"{sanitizationResult.SanitizedText}\"";
            if (sanitizationResult.DetectedWords.Any())
            {
                botResponse += $" (Note: {sanitizationResult.DetectedWords.Count} sensitive word(s) were filtered)";
            }

            model.Messages.Add(new ChatMessage
            {
                IsUser = false,
                Message = botResponse,
                Timestamp = DateTime.Now
            });

            model.UserMessage = string.Empty;
        }
    }
}
