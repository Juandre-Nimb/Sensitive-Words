-- =============================================================================
-- Complete Sensitive Words Clean Database Setup Script
-- Creates database, drops and recreates all objects, seeds data
-- Safe to run multiple times
-- =============================================================================

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SensitiveWordsCleanDb')
BEGIN
    CREATE DATABASE SensitiveWordsCleanDb;
    PRINT 'Database SensitiveWordsCleanDb created successfully.';
END
ELSE
BEGIN
    PRINT 'Database SensitiveWordsCleanDb already exists.';
END
GO

-- Switch to the database
USE SensitiveWordsCleanDb;
GO

-- =============================================================================
-- DROP EXISTING OBJECTS (in reverse dependency order)
-- =============================================================================

USE SensitiveWordsCleanDb;
GO

-- ===========================================
-- DROP existing procs if they exist
-- ===========================================
IF OBJECT_ID('sp_GetAllSensitiveWords', 'P') IS NOT NULL DROP PROCEDURE sp_GetAllSensitiveWords;
IF OBJECT_ID('sp_GetSensitiveWordById', 'P') IS NOT NULL DROP PROCEDURE sp_GetSensitiveWordById;
IF OBJECT_ID('sp_GetActiveSensitiveWords', 'P') IS NOT NULL DROP PROCEDURE sp_GetActiveSensitiveWords;
IF OBJECT_ID('sp_GetSensitiveWordByWord', 'P') IS NOT NULL DROP PROCEDURE sp_GetSensitiveWordByWord;
IF OBJECT_ID('sp_CreateSensitiveWord', 'P') IS NOT NULL DROP PROCEDURE sp_CreateSensitiveWord;
IF OBJECT_ID('sp_UpdateSensitiveWord', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateSensitiveWord;
IF OBJECT_ID('sp_DeleteSensitiveWord', 'P') IS NOT NULL DROP PROCEDURE sp_DeleteSensitiveWord;
IF OBJECT_ID('sp_GetSensitiveWordsByPage', 'P') IS NOT NULL DROP PROCEDURE sp_GetSensitiveWordsByPage;
IF OBJECT_ID('sp_SearchSensitiveWords', 'P') IS NOT NULL DROP PROCEDURE sp_SearchSensitiveWords;
IF OBJECT_ID('sp_GetSensitiveWordsCount', 'P') IS NOT NULL DROP PROCEDURE sp_GetSensitiveWordsCount;
IF OBJECT_ID('sp_CheckWordExistsById', 'P') IS NOT NULL DROP PROCEDURE sp_CheckWordExistsById;
GO

-- ===========================================
-- RE-CREATE all procs
-- ===========================================

CREATE PROCEDURE sp_GetAllSensitiveWords
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_GetSensitiveWordById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_GetActiveSensitiveWords
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_GetSensitiveWordByWord
    @Word NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE LOWER(Word) = LOWER(@Word);
END;
GO

CREATE PROCEDURE sp_CreateSensitiveWord
    @Word NVARCHAR(255),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM SensitiveWords WHERE Word = @Word)
    BEGIN
        RAISERROR ('A sensitive word with this value already exists.', 16, 1);
        RETURN;
    END;
    
    INSERT INTO SensitiveWords (Word, CreatedAt)
    VALUES (@Word, GETUTCDATE());

    SET @NewId = SCOPE_IDENTITY();

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE Id = @NewId;
END;
GO

CREATE PROCEDURE sp_UpdateSensitiveWord
    @Id INT,
    @Word NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM SensitiveWords WHERE Id = @Id)
    BEGIN
        RAISERROR ('Sensitive word not found.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM SensitiveWords WHERE Word = @Word AND Id != @Id)
    BEGIN
        RAISERROR ('A sensitive word with this name already exists.', 16, 1);
        RETURN;
    END;

    UPDATE SensitiveWords
    SET Word = @Word,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteSensitiveWord
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM SensitiveWords WHERE Id = @Id)
    BEGIN
        RAISERROR ('Sensitive word not found.', 16, 1);
        RETURN;
    END;

    DELETE FROM SensitiveWords
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS DeletedRows;
END;
GO

CREATE PROCEDURE sp_GetSensitiveWordsByPage
    @Page INT = 1,
    @PageSize INT = 10,
    @Search NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE (@Search IS NULL OR Word LIKE '%' + @Search + '%')
    ORDER BY Word
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;
GO

CREATE PROCEDURE sp_SearchSensitiveWords
    @SearchTerm NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Word, CreatedAt, UpdatedAt
    FROM SensitiveWords
    WHERE Word LIKE '%' + @SearchTerm + '%'
    ORDER BY Word;
END;
GO

CREATE PROCEDURE sp_GetSensitiveWordsCount
    @Search NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalCount
    FROM SensitiveWords
    WHERE (@Search IS NULL OR Word LIKE '%' + @Search + '%');
END;
GO

CREATE PROCEDURE sp_CheckWordExistsById
    @Id INT,
    @Exists BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM SensitiveWords WHERE Id = @Id)
        SET @Exists = 1;
    ELSE
        SET @Exists = 0;
END;
GO


PRINT 'All stored procedures created successfully.';

-- =============================================================================
-- SEED DATA - SQL Reserved Words
-- =============================================================================

PRINT 'Inserting seed data...';

Delete from SensitiveWords
-- Insert SQL reserved words (using simplified columns)
INSERT INTO SensitiveWords (Word, CreatedAt) VALUES
('ACTION', GETUTCDATE()),
('ADD', GETUTCDATE()),
('ALL', GETUTCDATE()),
('ALLOCATE', GETUTCDATE()),
('ALTER', GETUTCDATE()),
('ANY', GETUTCDATE()),
('APPLICATION', GETUTCDATE()),
('ARE', GETUTCDATE()),
('AREA', GETUTCDATE()),
('ASC', GETUTCDATE()),
('ASSERTION', GETUTCDATE()),
('ATOMIC', GETUTCDATE()),
('AUTHORIZATION', GETUTCDATE()),
('AVG', GETUTCDATE()),
('BEGIN', GETUTCDATE()),
('BY', GETUTCDATE()),
('CALL', GETUTCDATE()),
('CASCADE', GETUTCDATE()),
('CASCADED', GETUTCDATE()),
('CATALOG', GETUTCDATE()),
('CHECK', GETUTCDATE()),
('CLOSE', GETUTCDATE()),
('COLUMN', GETUTCDATE()),
('COMMIT', GETUTCDATE()),
('COMPRESS', GETUTCDATE()),
('CONNECT', GETUTCDATE()),
('CONNECTION', GETUTCDATE()),
('CONSTRAINT', GETUTCDATE()),
('CONSTRAINTS', GETUTCDATE()),
('CONTINUE', GETUTCDATE()),
('CONVERT', GETUTCDATE()),
('CORRESPONDING', GETUTCDATE()),
('CREATE', GETUTCDATE()),
('CROSS', GETUTCDATE()),
('CURRENT', GETUTCDATE()),
('CURRENT_PATH', GETUTCDATE()),
('CURRENT_SCHEMA', GETUTCDATE()),
('CURRENT_SCHEMAID', GETUTCDATE()),
('CURRENT_USER', GETUTCDATE()),
('CURRENT_USERID', GETUTCDATE()),
('CURSOR', GETUTCDATE()),
('DATA', GETUTCDATE()),
('DEALLOCATE', GETUTCDATE()),
('DECLARE', GETUTCDATE()),
('DEFAULT', GETUTCDATE()),
('DEFERRABLE', GETUTCDATE()),
('DEFERRED', GETUTCDATE()),
('DELETE', GETUTCDATE()),
('DESC', GETUTCDATE()),
('DESCRIBE', GETUTCDATE()),
('DESCRIPTOR', GETUTCDATE()),
('DETERMINISTIC', GETUTCDATE()),
('DIAGNOSTICS', GETUTCDATE()),
('DIRECTORY', GETUTCDATE()),
('DISCONNECT', GETUTCDATE()),
('DISTINCT', GETUTCDATE()),
('DO', GETUTCDATE()),
('DOMAIN', GETUTCDATE()),
('DOUBLEATTRIBUTE', GETUTCDATE()),
('DROP', GETUTCDATE()),
('EACH', GETUTCDATE()),
('EXCEPT', GETUTCDATE()),
('EXCEPTION', GETUTCDATE()),
('EXEC', GETUTCDATE()),
('EXECUTE', GETUTCDATE()),
('EXTERNAL', GETUTCDATE()),
('FETCH', GETUTCDATE()),
('FLOAT', GETUTCDATE()),
('FOREIGN', GETUTCDATE()),
('FOUND', GETUTCDATE()),
('FULL', GETUTCDATE()),
('FUNCTION', GETUTCDATE()),
('GET', GETUTCDATE()),
('GLOBAL', GETUTCDATE()),
('GO', GETUTCDATE()),
('GOTO', GETUTCDATE()),
('GRANT', GETUTCDATE()),
('GROUP', GETUTCDATE()),
('HANDLER', GETUTCDATE()),
('HAVING', GETUTCDATE()),
('IDENTITY', GETUTCDATE()),
('IMMEDIATE', GETUTCDATE()),
('INDEX', GETUTCDATE()),
('INDEXED', GETUTCDATE()),
('INDICATOR', GETUTCDATE()),
('INITIALLY', GETUTCDATE()),
('INNER', GETUTCDATE()),
('INOUT', GETUTCDATE()),
('INPUT', GETUTCDATE()),
('INSENSITIVE', GETUTCDATE()),
('INSERT', GETUTCDATE()),
('INTERSECT', GETUTCDATE()),
('INTO', GETUTCDATE()),
('ISOLATION', GETUTCDATE()),
('JOIN', GETUTCDATE()),
('KEY', GETUTCDATE()),
('LANGUAGE', GETUTCDATE()),
('LAST', GETUTCDATE()),
('LEAVE', GETUTCDATE()),
('LEVEL', GETUTCDATE()),
('LOCAL', GETUTCDATE()),
('LONGATTRIBUTE', GETUTCDATE()),
('LOOP', GETUTCDATE()),
('MODIFIES', GETUTCDATE()),
('MODULE', GETUTCDATE()),
('NAMES', GETUTCDATE()),
('NATIONAL', GETUTCDATE()),
('NATURAL', GETUTCDATE()),
('NEXT', GETUTCDATE()),
('NULLIF', GETUTCDATE()),
('ON', GETUTCDATE()),
('ONLY', GETUTCDATE()),
('OPEN', GETUTCDATE()),
('OPTION', GETUTCDATE()),
('ORDER', GETUTCDATE()),
('OUT', GETUTCDATE()),
('OUTER', GETUTCDATE()),
('OUTPUT', GETUTCDATE()),
('OVERLAPS', GETUTCDATE()),
('OWNER', GETUTCDATE()),
('PARTIAL', GETUTCDATE()),
('PATH', GETUTCDATE()),
('PRECISION', GETUTCDATE()),
('PREPARE', GETUTCDATE()),
('PRESERVE', GETUTCDATE()),
('PRIMARY', GETUTCDATE()),
('PRIOR', GETUTCDATE()),
('PRIVILEGES', GETUTCDATE()),
('PROCEDURE', GETUTCDATE()),
('PUBLIC', GETUTCDATE()),
('READ', GETUTCDATE()),
('READS', GETUTCDATE()),
('REFERENCES', GETUTCDATE()),
('RELATIVE', GETUTCDATE()),
('REPEAT', GETUTCDATE()),
('RESIGNAL', GETUTCDATE()),
('RESTRICT', GETUTCDATE()),
('RETURN', GETUTCDATE()),
('RETURNS', GETUTCDATE()),
('REVOKE', GETUTCDATE()),
('ROLLBACK', GETUTCDATE()),
('ROUTINE', GETUTCDATE()),
('ROW', GETUTCDATE()),
('ROWS', GETUTCDATE()),
('SCHEMA', GETUTCDATE()),
('SCROLL', GETUTCDATE()),
('SECTION', GETUTCDATE()),
('SELECT', GETUTCDATE()),
('SEQ', GETUTCDATE()),
('SEQUENCE', GETUTCDATE()),
('SESSION', GETUTCDATE()),
('SESSION_USER', GETUTCDATE()),
('SESSION_USERID', GETUTCDATE()),
('SET', GETUTCDATE()),
('SIGNAL', GETUTCDATE()),
('SOME', GETUTCDATE()),
('SPACE', GETUTCDATE()),
('SPECIFIC', GETUTCDATE()),
('SQL', GETUTCDATE()),
('SQLCODE', GETUTCDATE()),
('SQLERROR', GETUTCDATE()),
('SQLEXCEPTION', GETUTCDATE()),
('SQLSTATE', GETUTCDATE()),
('SQLWARNING', GETUTCDATE()),
('STATEMENT', GETUTCDATE()),
('STRINGATTRIBUTE', GETUTCDATE()),
('SUM', GETUTCDATE()),
('SYSACC', GETUTCDATE()),
('SYSHGH', GETUTCDATE()),
('SYSLNK', GETUTCDATE()),
('SYSNIX', GETUTCDATE()),
('SYSTBLDEF', GETUTCDATE()),
('SYSTBLDSC', GETUTCDATE()),
('SYSTBT', GETUTCDATE()),
('SYSTBTATT', GETUTCDATE()),
('SYSTBTDEF', GETUTCDATE()),
('SYSUSR', GETUTCDATE()),
('SYSTEM_USER', GETUTCDATE()),
('SYSVIW', GETUTCDATE()),
('SYSVIWCOL', GETUTCDATE()),
('TABLE', GETUTCDATE()),
('TABLETYPE', GETUTCDATE()),
('TEMPORARY', GETUTCDATE()),
('TRANSACTION', GETUTCDATE()),
('TRANSLATE', GETUTCDATE()),
('TRANSLATION', GETUTCDATE()),
('TRIGGER', GETUTCDATE()),
('UNDO', GETUTCDATE()),
('UNION', GETUTCDATE()),
('UNIQUE', GETUTCDATE()),
('UNTIL', GETUTCDATE()),
('UPDATE', GETUTCDATE()),
('USAGE', GETUTCDATE()),
('USER', GETUTCDATE()),
('USING', GETUTCDATE()),
('VALUE', GETUTCDATE()),
('VALUES', GETUTCDATE()),
('VIEW', GETUTCDATE()),
('WHERE', GETUTCDATE()),
('WHILE', GETUTCDATE()),
('WITH', GETUTCDATE()),
('WORK', GETUTCDATE()),
('WRITE', GETUTCDATE()),
('ALLSCHEMAS', GETUTCDATE()),
('ALLTABLES', GETUTCDATE()),
('ALLVIEWS', GETUTCDATE()),
('ALLVIEWTEXTS', GETUTCDATE()),
('ALLCOLUMNS', GETUTCDATE()),
('ALLINDEXES', GETUTCDATE()),
('ALLINDEXCOLS', GETUTCDATE()),
('ALLUSERS', GETUTCDATE()),
('ALLTBTS', GETUTCDATE()),
('TABLEPRIVILEGES', GETUTCDATE()),
('TBTPRIVILEGES', GETUTCDATE()),
('MYSCHEMAS', GETUTCDATE()),
('MYTABLES', GETUTCDATE()),
('MYTBTS', GETUTCDATE()),
('MYVIEWS', GETUTCDATE()),
('SCHEMAVIEWS', GETUTCDATE()),
('DUAL', GETUTCDATE()),
('SCHEMAPRIVILEGES', GETUTCDATE()),
('SCHEMATABLES', GETUTCDATE()),
('STATISTICS', GETUTCDATE()),
('USRTBL', GETUTCDATE()),
('STRINGTABLE', GETUTCDATE()),
('LONGTABLE', GETUTCDATE()),
('DOUBLETABLE', GETUTCDATE()),
('SELECT * FROM', GETUTCDATE());

PRINT 'Successfully inserted ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' sensitive words.';

-- =============================================================================
-- VERIFICATION & SUMMARY
-- =============================================================================

PRINT '';
PRINT '=== SETUP COMPLETED SUCCESSFULLY ===';
PRINT 'Database: SensitiveWordsCleanDb is ready for use.';
PRINT '';

-- Show summary
DECLARE @WordCount INT, @ProcCount INT;
SELECT @WordCount = COUNT(*) FROM SensitiveWords;
SELECT @ProcCount = COUNT(*) FROM sys.objects WHERE type = 'P' AND name LIKE 'sp_%';

PRINT '✓ Total Sensitive Words: ' + CAST(@WordCount AS NVARCHAR(10));
PRINT '✓ Total Stored Procedures: ' + CAST(@ProcCount AS NVARCHAR(10));
PRINT '✓ Connection String: Server=(localdb)\mssqllocaldb;Database=SensitiveWordsCleanDb;Trusted_Connection=true;MultipleActiveResultSets=true';
PRINT '';
PRINT 'Ready to start API and Web applications!';

GO
