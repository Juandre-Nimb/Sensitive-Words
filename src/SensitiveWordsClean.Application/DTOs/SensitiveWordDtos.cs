namespace SensitiveWordsClean.Application.DTOs;

public class SensitiveWordDto
{
    public int Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateSensitiveWordDto
{
    public string Word { get; set; } = string.Empty;
}

public class UpdateSensitiveWordDto
{
    public string Word { get; set; } = string.Empty;
}

public class TextSanitizationRequestDto
{
    public string Text { get; set; } = string.Empty;
    public string ReplacementCharacter { get; set; } = "*";
}

public class TextSanitizationResponseDto
{
    public string OriginalText { get; set; } = string.Empty;
    public string SanitizedText { get; set; } = string.Empty;
    public List<string> DetectedWords { get; set; } = new();
}
