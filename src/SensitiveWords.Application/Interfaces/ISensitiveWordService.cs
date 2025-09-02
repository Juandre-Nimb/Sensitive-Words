using SensitiveWordsClean.Application.DTOs;

namespace SensitiveWordsClean.Application.Interfaces;

public interface ISensitiveWordService
{
    Task<IEnumerable<SensitiveWordDto>> GetAllAsync();
    Task<SensitiveWordDto?> GetByIdAsync(int id);
    Task<SensitiveWordDto> CreateAsync(CreateSensitiveWordDto createDto);
    Task<SensitiveWordDto> UpdateAsync(int id, UpdateSensitiveWordDto updateDto);
    Task DeleteAsync(int id);
    Task<TextSanitizationResponseDto> SanitizeTextAsync(TextSanitizationRequestDto request);
}
