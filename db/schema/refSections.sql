-- refSections is the table for top level sections (i.e. NMG, MG, Misc)

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [deertier].[refSections](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO
