using System.Text.RegularExpressions;
using SensitiveWordsClean.Domain.Interfaces;
using SensitiveWordsClean.Application.Interfaces;

namespace SensitiveWordsClean.Infrastructure.Services;

public class TextSanitizationService : ITextSanitizationService
{
    private readonly ISensitiveWordRepository _sensitiveWordRepository;

    public TextSanitizationService(ISensitiveWordRepository sensitiveWordRepository)
    {
        _sensitiveWordRepository = sensitiveWordRepository ?? throw new ArgumentNullException(nameof(_sensitiveWordRepository));
    }

    public async Task<string> SanitizeTextAsync(string input, string replacementCharacter = "*")
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var sensitiveWords = await _sensitiveWordRepository.GetAllAsync();
        var result = input;

        foreach (var sensitiveWord in sensitiveWords)
        {
            var pattern = $@"\b{Regex.Escape(sensitiveWord.Word)}\b";
            var replacement = new string(replacementCharacter[0], sensitiveWord.Word.Length);
            result = Regex.Replace(result, pattern, replacement, RegexOptions.IgnoreCase);
        }

        return result;
    }

    public async Task<List<string>> DetectSensitiveWordsAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        var sensitiveWords = await _sensitiveWordRepository.GetAllAsync();
        var detectedWords = new List<string>();

        foreach (var sensitiveWord in sensitiveWords)
        {
            var pattern = $@"\b{Regex.Escape(sensitiveWord.Word)}\b";
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                detectedWords.Add(sensitiveWord.Word);
            }
        }

        return detectedWords.Distinct().ToList();
    }
}
