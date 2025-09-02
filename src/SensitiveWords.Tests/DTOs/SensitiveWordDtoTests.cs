using SensitiveWordsClean.Application.DTOs;
using Xunit;

namespace SensitiveWords.Tests.DTOs;

public class SensitiveWordDtoTests
{
    [Fact]
    public void SensitiveWordDto_DefaultConstructor_InitializesCorrectly()
    {
        // Act
        var dto = new SensitiveWordDto();

        // Assert
        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.Word);
        Assert.Equal(default(DateTime), dto.CreatedAt);
        Assert.Null(dto.UpdatedAt);
    }

    [Fact]
    public void SensitiveWordDto_PropertiesCanBeSet()
    {
        // Arrange
        var testDate = DateTime.UtcNow;

        // Act
        var dto = new SensitiveWordDto
        {
            Id = 1,
            Word = "TESTWORD",
            CreatedAt = testDate,
            UpdatedAt = testDate
        };

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("TESTWORD", dto.Word);
        Assert.Equal(testDate, dto.CreatedAt);
        Assert.Equal(testDate, dto.UpdatedAt);
    }

    [Fact]
    public void CreateSensitiveWordDto_DefaultConstructor_InitializesCorrectly()
    {
        // Act
        var dto = new CreateSensitiveWordDto();

        // Assert
        Assert.Equal(string.Empty, dto.Word);
    }

    [Fact]
    public void CreateSensitiveWordDto_WordProperty_CanBeSet()
    {
        // Act
        var dto = new CreateSensitiveWordDto { Word = "TESTWORD" };

        // Assert
        Assert.Equal("TESTWORD", dto.Word);
    }

    [Fact]
    public void UpdateSensitiveWordDto_DefaultConstructor_InitializesCorrectly()
    {
        // Act
        var dto = new UpdateSensitiveWordDto();

        // Assert
        Assert.Equal(string.Empty, dto.Word);
    }

    [Fact]
    public void UpdateSensitiveWordDto_WordProperty_CanBeSet()
    {
        // Act
        var dto = new UpdateSensitiveWordDto { Word = "UPDATEDWORD" };

        // Assert
        Assert.Equal("UPDATEDWORD", dto.Word);
    }

    [Fact]
    public void TextSanitizationRequestDto_DefaultConstructor_InitializesCorrectly()
    {
        // Act
        var dto = new TextSanitizationRequestDto();

        // Assert
        Assert.Equal(string.Empty, dto.Text);
        Assert.Equal("*", dto.ReplacementCharacter);
    }

    [Fact]
    public void TextSanitizationRequestDto_PropertiesCanBeSet()
    {
        // Act
        var dto = new TextSanitizationRequestDto
        {
            Text = "This is test text with badword",
            ReplacementCharacter = "#"
        };

        // Assert
        Assert.Equal("This is test text with badword", dto.Text);
        Assert.Equal("#", dto.ReplacementCharacter);
    }

    [Fact]
    public void TextSanitizationResponseDto_DefaultConstructor_InitializesCorrectly()
    {
        // Act
        var dto = new TextSanitizationResponseDto();

        // Assert
        Assert.Equal(string.Empty, dto.OriginalText);
        Assert.Equal(string.Empty, dto.SanitizedText);
        Assert.NotNull(dto.DetectedWords);
        Assert.Empty(dto.DetectedWords);
    }

    [Fact]
    public void TextSanitizationResponseDto_PropertiesCanBeSet()
    {
        // Arrange
        var detectedWords = new List<string> { "badword", "offensive" };

        // Act
        var dto = new TextSanitizationResponseDto
        {
            OriginalText = "Original text with badword and offensive content",
            SanitizedText = "Original text with ******* and ********* content",
            DetectedWords = detectedWords
        };

        // Assert
        Assert.Equal("Original text with badword and offensive content", dto.OriginalText);
        Assert.Equal("Original text with ******* and ********* content", dto.SanitizedText);
        Assert.Equal(2, dto.DetectedWords.Count);
        Assert.Contains("badword", dto.DetectedWords);
        Assert.Contains("offensive", dto.DetectedWords);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("#")]
    [InlineData("@")]
    [InlineData("X")]
    public void TextSanitizationRequestDto_ReplacementCharacter_AcceptsValidCharacters(string replacementChar)
    {
        // Act
        var dto = new TextSanitizationRequestDto
        {
            ReplacementCharacter = replacementChar
        };

        // Assert
        Assert.Equal(replacementChar, dto.ReplacementCharacter);
    }
}
