using AutoMapper;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Application.Interfaces;
using SensitiveWordsClean.Domain.Entities;
using SensitiveWordsClean.Domain.Interfaces;

namespace SensitiveWordsClean.Application.Services;

public class SensitiveWordService : ISensitiveWordService
{
    private readonly ISensitiveWordRepository _repository;
    private readonly ITextSanitizationService _sanitizationService;
    private readonly IMapper _mapper;

    public SensitiveWordService(ISensitiveWordRepository repository, ITextSanitizationService sanitizationService, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _sanitizationService = sanitizationService ?? throw new ArgumentNullException(nameof(sanitizationService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<SensitiveWordDto>> GetAllAsync()
    {
        var words = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SensitiveWordDto>>(words);
    }

    public async Task<SensitiveWordDto?> GetByIdAsync(int id)
    {
        var word = await _repository.GetByIdAsync(id);
        return word != null ? _mapper.Map<SensitiveWordDto>(word) : null;
    }

    public async Task<SensitiveWordDto> CreateAsync(CreateSensitiveWordDto createDto)
    {
        var entity = _mapper.Map<SensitiveWord>(createDto);
        entity.Word = createDto.Word.Trim().ToUpperInvariant();

        try
        {
            var createdEntity = await _repository.CreateAsync(entity);
            return _mapper.Map<SensitiveWordDto>(createdEntity);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Sensitive word with name {entity.Word} already exists.");
        }
    }

    public async Task<SensitiveWordDto> UpdateAsync(int id, UpdateSensitiveWordDto updateDto)
    {
        var entity = _mapper.Map<SensitiveWord>(updateDto);
        entity.Id = id;
        entity.Word = updateDto.Word.Trim().ToUpperInvariant();

        try
        {
            var updatedEntity = await _repository.UpdateAsync(entity);
            return _mapper.Map<SensitiveWordDto>(updatedEntity);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Sensitive word with ID {id} not found.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Sensitive word with ID {id} not found.");
        }
    }

    public async Task<TextSanitizationResponseDto> SanitizeTextAsync(TextSanitizationRequestDto request)
    {
        var detectedWords = await _sanitizationService.DetectSensitiveWordsAsync(request.Text);
        var sanitizedText = await _sanitizationService.SanitizeTextAsync(request.Text, request.ReplacementCharacter);

        return new TextSanitizationResponseDto
        {
            OriginalText = request.Text,
            SanitizedText = sanitizedText,
            DetectedWords = detectedWords
        };
    }
}
