using System.ComponentModel.DataAnnotations;

namespace SensitiveWordsClean.Web.Models;

public class CreateSensitiveWordViewModel
{
    [Required]
    [StringLength(255, ErrorMessage = "Word cannot exceed 255 characters.")]
    public string Word { get; set; } = string.Empty;
}

public class UpdateSensitiveWordViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(255, ErrorMessage = "Word cannot exceed 255 characters.")]
    public string Word { get; set; } = string.Empty;
}

public class ChatViewModel
{
    public string UserMessage { get; set; } = string.Empty;
    public List<ChatMessage> Messages { get; set; } = new();
}

public class ChatMessage
{
    public bool IsUser { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
