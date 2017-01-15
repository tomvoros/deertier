-- tblRecords is the table that holds records (i.e. times/runs) for all categories

-- TODO: 
-- - replace the Player column with UserId
-- - remove RealTimeString and GameTimeString columns
-- - normalize how times are stored in general

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [deertier].[tblRecords](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Player] [nvarchar](100) NOT NULL,
	[RealTimeSeconds] [int] NOT NULL,
	[RealTimeString] [nvarchar](100) NOT NULL,
	[GameTimeSeconds] [int] NULL,
	[GameTimeString] [nvarchar](100) NULL,
	[Comment] [nvarchar](100) NULL,
	[VideoURL] [nvarchar](100) NULL,
	[CeresTime] [float] NULL,
	[DateSubmitted] [datetime] NULL,
	[SubmittedByUserId] [int] NULL,
 CONSTRAINT [PK_dt_Records] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
