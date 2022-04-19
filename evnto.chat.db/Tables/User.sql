CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] NVARCHAR(256) NOT NULL,
    [UserName] VARCHAR(50) NOT NULL, 
    [PasswordHash] CHAR(64) NOT NULL, 
    [Salt] INT NOT NULL, 
    [SaltCount] INT NOT NULL, 
    [Created] DATETIMEOFFSET(3) NOT NULL DEFAULT sysdatetimeoffset()    
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_UserName] ON [dbo].[User] ([UserName])
