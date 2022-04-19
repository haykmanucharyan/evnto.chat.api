CREATE TABLE [dbo].[Chat]
(
	[ChatId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [InitiatorUserId] INT NOT NULL, 
    [RecipientUserId] INT NOT NULL, 
    [State] TINYINT NOT NULL, 
    [Created] DATETIMEOFFSET(3) NOT NULL, 
    CONSTRAINT [FK_Chat_User_Initiator] FOREIGN KEY ([InitiatorUserId]) REFERENCES [User]([UserId]),
    CONSTRAINT [FK_Chat_User_Recipient] FOREIGN KEY ([RecipientUserId]) REFERENCES [User]([UserId])
)

GO

CREATE INDEX [IX_Chat_InitiatorUserId] ON [dbo].[Chat] ([InitiatorUserId])

GO

CREATE INDEX [IX_Chat_RecipientUserId] ON [dbo].[Chat] ([RecipientUserId])
