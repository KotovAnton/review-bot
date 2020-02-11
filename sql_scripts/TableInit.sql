CREATE TABLE [ChatMembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChatId] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LastReview] [datetime] NOT NULL DEFAULT(GETDATE()),
	[Team] [nvarchar](100)
 CONSTRAINT [PK_ChatMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO