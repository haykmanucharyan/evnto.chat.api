﻿CREATE PROCEDURE [dbo].[SPSignOut]
	@UserId INT
WITH  
        NATIVE_COMPILATION,  
        SCHEMABINDING
AS
BEGIN ATOMIC
	WITH  
        (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english');

	DELETE FROM dbo.UserSession
	WHERE UserId = @UserId;
END