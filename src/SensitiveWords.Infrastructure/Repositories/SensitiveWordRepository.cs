using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SensitiveWordsClean.Domain.Entities;
using SensitiveWordsClean.Domain.Interfaces;

namespace SensitiveWordsClean.Infrastructure.Repositories;

public class SensitiveWordRepository : ISensitiveWordRepository
{
    private readonly string _connectionString;

    public SensitiveWordRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<SensitiveWord>> GetAllAsync()
    {
        var words = new List<SensitiveWord>();
        
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_GetAllSensitiveWords", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            words.Add(MapFromReader(reader));
        }
        
        return words;
    }

    public async Task<SensitiveWord?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_GetSensitiveWordById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    public async Task<SensitiveWord> CreateAsync(SensitiveWord entity)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_CreateSensitiveWord", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Word", entity.Word);
        
        var idParameter = new SqlParameter("@NewId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(idParameter);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        
        entity.Id = (int)idParameter.Value;
        entity.CreatedAt = DateTime.UtcNow;
        
        return entity;
    }

    public async Task<SensitiveWord> UpdateAsync(SensitiveWord entity)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_UpdateSensitiveWord", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@Word", entity.Word);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new ArgumentException($"Sensitive word with ID {entity.Id} not found.");
        }

        entity.UpdatedAt = DateTime.UtcNow;
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_DeleteSensitiveWord", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new ArgumentException($"Sensitive word with ID {id} not found.");
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_CheckWordExistsById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Id", id);
        
        var existsParameter = new SqlParameter("@Exists", SqlDbType.Bit)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(existsParameter);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        
        return (bool)existsParameter.Value;
    }

    public async Task<SensitiveWord?> GetByWordAsync(string word)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_GetSensitiveWordByWord", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        command.Parameters.AddWithValue("@Word", word);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    private static SensitiveWord MapFromReader(SqlDataReader reader)
    {
        return new SensitiveWord
        {
            Id = reader.GetInt32("Id"),
            Word = reader.GetString("Word"),
            CreatedAt = reader.GetDateTime("CreatedAt"),
            UpdatedAt = reader.IsDBNull("UpdatedAt") ? null : reader.GetDateTime("UpdatedAt")
        };
    }
}
