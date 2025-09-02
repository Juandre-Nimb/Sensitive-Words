using SensitiveWordsClean.Domain.Entities;
using Xunit;

namespace SensitiveWords.Tests.Domain;

public class SensitiveWordTests
{
    [Fact]
    public void SensitiveWord_DefaultConstructor_SetsDefaultValues()
    {
        // Act
        var sensitiveWord = new SensitiveWord();

        // Assert
        Assert.Equal(0, sensitiveWord.Id);
        Assert.Equal(string.Empty, sensitiveWord.Word);
        Assert.True(sensitiveWord.CreatedAt <= DateTime.UtcNow);
        Assert.True(sensitiveWord.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
        Assert.Null(sensitiveWord.UpdatedAt);
    }

    [Fact]
    public void SensitiveWord_SettingProperties_WorksCorrectly()
    {
        // Arrange
        var testWord = "TESTWORD";
        var testDate = DateTime.UtcNow;

        // Act
        var sensitiveWord = new SensitiveWord
        {
            Id = 1,
            Word = testWord,
            CreatedAt = testDate,
            UpdatedAt = testDate
        };

        // Assert
        Assert.Equal(1, sensitiveWord.Id);
        Assert.Equal(testWord, sensitiveWord.Word);
        Assert.Equal(testDate, sensitiveWord.CreatedAt);
        Assert.Equal(testDate, sensitiveWord.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("test")]
    [InlineData("this is a very long word that should still be valid")]
    public void SensitiveWord_Word_AcceptsValidStrings(string word)
    {
        // Act
        var sensitiveWord = new SensitiveWord { Word = word };

        // Assert
        Assert.Equal(word, sensitiveWord.Word);
    }

    [Fact]
    public void SensitiveWord_UpdatedAt_CanBeNull()
    {
        // Act
        var sensitiveWord = new SensitiveWord
        {
            UpdatedAt = null
        };

        // Assert
        Assert.Null(sensitiveWord.UpdatedAt);
    }

    [Fact]
    public void SensitiveWord_UpdatedAt_CanBeSet()
    {
        // Arrange
        var updateTime = DateTime.UtcNow;

        // Act
        var sensitiveWord = new SensitiveWord
        {
            UpdatedAt = updateTime
        };

        // Assert
        Assert.Equal(updateTime, sensitiveWord.UpdatedAt);
    }
}
