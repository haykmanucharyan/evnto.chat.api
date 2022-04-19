CREATE PROCEDURE [dbo].[SPSessionUpdateApiKey]
	@Token CHAR(64),	
	@ApiKey VARCHAR(256)
WITH  
        NATIVE_COMPILATION,  
        SCHEMABINDING
AS
BEGIN ATOMIC
	WITH  
        (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english');

	UPDATE dbo.UserSession
		SET ApiKey = @ApiKey
	WHERE Token = @Token;
END
