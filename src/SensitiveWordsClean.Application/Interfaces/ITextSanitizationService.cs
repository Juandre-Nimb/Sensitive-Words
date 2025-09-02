namespace SensitiveWordsClean.Application.Interfaces;

public interface ITextSanitizationService
{
    Task<string> SanitizeTextAsync(string input, string replacementCharacter = "*");
    Task<List<string>> DetectSensitiveWordsAsync(string input);
}
