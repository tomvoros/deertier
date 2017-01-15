-- tblCategories is the categories table

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [deertier].[tblCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[UrlName] [nvarchar](200) NULL,
	[ParentId] [int] NULL,
	[SectionId] [int] NULL,
	[AllowSubmission] [bit] NOT NULL,
	[Visible] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[GameTime] [bit] NOT NULL,
	[EscapeGameTime] [bit] NOT NULL,
	[RealTime] [bit] NOT NULL,
	[ShortName] [nvarchar](200) NULL,
	[Enabled] [bit] NOT NULL,
	[WikiUrl] [nvarchar](200) NULL
) ON [PRIMARY]

GO
