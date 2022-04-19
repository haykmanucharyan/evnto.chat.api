CREATE TABLE [dbo].[Message]
(
	[MessageId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ChatId] INT NOT NULL, 
    [AuthorUserId] INT NOT NULL, 
    [Created] DATETIMEOFFSET(3) NOT NULL, 
    [Text] NVARCHAR(4000) NOT NULL, 
    CONSTRAINT [FK_Message_Chat] FOREIGN KEY ([ChatId]) REFERENCES [Chat]([ChatId]), 
    CONSTRAINT [FK_Message_User] FOREIGN KEY ([AuthorUserId]) REFERENCES [User]([UserId])
)

GO

CREATE INDEX [IX_Message_ChatId] ON [dbo].[Message] ([ChatId])
