-- tblModerationLog is a log of moderator actions (e.g. deleting or creating records)

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [deertier].[tblModerationLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Action] [tinyint] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[RelatedId1] [int] NULL,
	[RelatedId2] [int] NULL,
	[RelatedId3] [int] NULL,
	[Date] [datetime] NOT NULL,
	[IPAddress] [nvarchar](100) NOT NULL,
	[UserAgent] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_tblModerationLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
