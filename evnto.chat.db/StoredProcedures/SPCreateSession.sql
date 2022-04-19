CREATE PROCEDURE [dbo].[SPCreateSession]
	@Token CHAR(64),
	@UserId INT,
	@ApiKey VARCHAR(256)
WITH  
        NATIVE_COMPILATION,  
        SCHEMABINDING
AS
BEGIN ATOMIC
	WITH  
        (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english');

	-- delete old session
	DELETE FROM dbo.UserSession
	WHERE UserId = @UserId;

	-- create new session
	INSERT INTO dbo.UserSession (Token, UserId, ApiKey)
					VALUES (@Token, @UserId, @ApiKey);

END
