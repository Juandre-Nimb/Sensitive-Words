using AutoMapper;
using Moq;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Application.Interfaces;
using SensitiveWordsClean.Application.Services;
using SensitiveWordsClean.Domain.Entities;
using SensitiveWordsClean.Domain.Interfaces;
using Xunit;

namespace SensitiveWords.Tests.Services;

public class SensitiveWordServiceTests
{
    private readonly Mock<ISensitiveWordRepository> _mockRepository;
    private readonly Mock<ITextSanitizationService> _mockSanitizationService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SensitiveWordService _service;

    public SensitiveWordServiceTests()
    {
        _mockRepository = new Mock<ISensitiveWordRepository>();
        _mockSanitizationService = new Mock<ITextSanitizationService>();
        _mockMapper = new Mock<IMapper>();
        _service = new SensitiveWordService(_mockRepository.Object, _mockSanitizationService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSensitiveWords()
    {
        // Arrange
        var entities = new List<SensitiveWord>
        {
            new() { Id = 1, Word = "BADWORD1", CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Word = "BADWORD2", CreatedAt = DateTime.UtcNow }
        };

        var dtos = new List<SensitiveWordDto>
        {
            new() { Id = 1, Word = "BADWORD1", CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Word = "BADWORD2", CreatedAt = DateTime.UtcNow }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mockMapper.Setup(m => m.Map<IEnumerable<SensitiveWordDto>>(entities)).Returns(dtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<SensitiveWordDto>>(entities), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsSensitiveWord()
    {
        // Arrange
        var id = 1;
        var entity = new SensitiveWord { Id = id, Word = "BADWORD", CreatedAt = DateTime.UtcNow };
        var dto = new SensitiveWordDto { Id = id, Word = "BADWORD", CreatedAt = DateTime.UtcNow };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        _mockMapper.Setup(m => m.Map<SensitiveWordDto>(entity)).Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("BADWORD", result.Word);
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var id = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((SensitiveWord?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidWord_ReturnsCreatedWord()
    {
        // Arrange
        var createDto = new CreateSensitiveWordDto { Word = "  badword  " };
        var entity = new SensitiveWord { Word = "BADWORD", CreatedAt = DateTime.UtcNow };
        var createdEntity = new SensitiveWord { Id = 1, Word = "BADWORD", CreatedAt = DateTime.UtcNow };
        var resultDto = new SensitiveWordDto { Id = 1, Word = "BADWORD", CreatedAt = DateTime.UtcNow };

        _mockMapper.Setup(m => m.Map<SensitiveWord>(createDto)).Returns(entity);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<SensitiveWord>())).ReturnsAsync(createdEntity);
        _mockMapper.Setup(m => m.Map<SensitiveWordDto>(createdEntity)).Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("BADWORD", result.Word);
        _mockRepository.Verify(r => r.CreateAsync(It.Is<SensitiveWord>(w => w.Word == "BADWORD")), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DuplicateWord_ThrowsArgumentException()
    {
        // Arrange
        var createDto = new CreateSensitiveWordDto { Word = "badword" };
        var entity = new SensitiveWord { Word = "BADWORD", CreatedAt = DateTime.UtcNow };

        _mockMapper.Setup(m => m.Map<SensitiveWord>(createDto)).Returns(entity);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<SensitiveWord>())).ThrowsAsync(new ArgumentException());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        Assert.Contains("already exists", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ValidWord_ReturnsUpdatedWord()
    {
        // Arrange
        var id = 1;
        var updateDto = new UpdateSensitiveWordDto { Word = "  updatedword  " };
        var entity = new SensitiveWord { Word = "UPDATEDWORD" };
        var updatedEntity = new SensitiveWord { Id = id, Word = "UPDATEDWORD", UpdatedAt = DateTime.UtcNow };
        var resultDto = new SensitiveWordDto { Id = id, Word = "UPDATEDWORD", UpdatedAt = DateTime.UtcNow };

        _mockMapper.Setup(m => m.Map<SensitiveWord>(updateDto)).Returns(entity);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SensitiveWord>())).ReturnsAsync(updatedEntity);
        _mockMapper.Setup(m => m.Map<SensitiveWordDto>(updatedEntity)).Returns(resultDto);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("UPDATEDWORD", result.Word);
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<SensitiveWord>(w => w.Id == id && w.Word == "UPDATEDWORD")), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsArgumentException()
    {
        // Arrange
        var id = 999;
        var updateDto = new UpdateSensitiveWordDto { Word = "word" };
        var entity = new SensitiveWord { Word = "WORD" };

        _mockMapper.Setup(m => m.Map<SensitiveWord>(updateDto)).Returns(entity);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SensitiveWord>())).ThrowsAsync(new ArgumentException());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, updateDto));
        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesSuccessfully()
    {
        // Arrange
        var id = 1;
        _mockRepository.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ThrowsArgumentException()
    {
        // Arrange
        var id = 999;
        _mockRepository.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new ArgumentException());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(id));
        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public async Task SanitizeTextAsync_ValidRequest_ReturnsSanitizedResponse()
    {
        // Arrange
        var request = new TextSanitizationRequestDto
        {
            Text = "This is a badword in text",
            ReplacementCharacter = "*"
        };

        var detectedWords = new List<string> { "badword" };
        var sanitizedText = "This is a ******* in text";

        _mockSanitizationService.Setup(s => s.DetectSensitiveWordsAsync(request.Text)).ReturnsAsync(detectedWords);
        _mockSanitizationService.Setup(s => s.SanitizeTextAsync(request.Text, request.ReplacementCharacter)).ReturnsAsync(sanitizedText);

        // Act
        var result = await _service.SanitizeTextAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Text, result.OriginalText);
        Assert.Equal(sanitizedText, result.SanitizedText);
        Assert.Single(result.DetectedWords);
        Assert.Contains("badword", result.DetectedWords);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  word  ")]
    public void CreateAsync_TrimsAndUppercasesWord(string inputWord)
    {
        // Arrange
        var createDto = new CreateSensitiveWordDto { Word = inputWord };
        var entity = new SensitiveWord { Word = inputWord };
        var createdEntity = new SensitiveWord { Id = 1, Word = inputWord.Trim().ToUpperInvariant(), CreatedAt = DateTime.UtcNow };
        var resultDto = new SensitiveWordDto { Id = 1, Word = inputWord.Trim().ToUpperInvariant(), CreatedAt = DateTime.UtcNow };

        _mockMapper.Setup(m => m.Map<SensitiveWord>(createDto)).Returns(entity);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<SensitiveWord>())).ReturnsAsync(createdEntity);
        _mockMapper.Setup(m => m.Map<SensitiveWordDto>(createdEntity)).Returns(resultDto);

        // Act & Assert
        var result = _service.CreateAsync(createDto);
        
        // Verify the word was trimmed and uppercased
        _mockRepository.Verify(r => r.CreateAsync(It.Is<SensitiveWord>(w => w.Word == inputWord.Trim().ToUpperInvariant())), Times.Once);
    }

    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SensitiveWordService(null!, _mockSanitizationService.Object, _mockMapper.Object));
    }

    [Fact]
    public void Constructor_NullSanitizationService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SensitiveWordService(_mockRepository.Object, null!, _mockMapper.Object));
    }

    [Fact]
    public void Constructor_NullMapper_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SensitiveWordService(_mockRepository.Object, _mockSanitizationService.Object, null!));
    }
}
