using System.ComponentModel.DataAnnotations;

namespace SensitiveWordsClean.Domain.Entities;

public class SensitiveWord
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Word { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
