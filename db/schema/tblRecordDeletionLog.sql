-- tblRecordDeletionLog keeps a copy of every record that is deleted from tblRecords

-- TODO:
-- - consider leaving deleted records in tblRecords with an IsDeleted flag instead of copying them here

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [deertier].[tblRecordDeletionLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Moderator] [nvarchar](100) NOT NULL,
	[DeletionDate] [datetime] NULL,
	[CategoryId] [int] NOT NULL,
	[Player] [nvarchar](100) NOT NULL,
	[RealTimeString] [nvarchar](100) NOT NULL,
	[GameTimeString] [nvarchar](100) NULL,
	[RealTimeSeconds] [int] NOT NULL,
	[GameTimeSeconds] [int] NULL,
	[Comment] [nvarchar](100) NULL,
	[VideoURL] [nvarchar](100) NULL,
	[CeresTime] [float] NULL,
	[DateSubmitted] [datetime] NULL,
	[SubmittedByUserId] [int] NULL,
	[IPAddress] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dt_RecordDeletionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
