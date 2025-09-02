using SensitiveWordsClean.Domain.Entities;

namespace SensitiveWordsClean.Domain.Interfaces;

public interface ISensitiveWordRepository
{
    Task<SensitiveWord?> GetByWordAsync(string word);
    Task<SensitiveWord?> GetByIdAsync(int id);
    Task<IEnumerable<SensitiveWord>> GetAllAsync();
    Task<SensitiveWord> CreateAsync(SensitiveWord entity);
    Task<SensitiveWord> UpdateAsync(SensitiveWord entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
