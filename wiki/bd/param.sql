USE [DigevoUsers]
GO

/****** Object:  Table [dbo].[Param]    Script Date: 26-09-2017 8:12:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Param](
	[idParam] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](48) NOT NULL,
	[description] [varchar](144) NULL,
	[tag] [varchar](24) NOT NULL,
	[creationDate] [datetime] NOT NULL,
	[modificationDate] [datetime] NULL,
 CONSTRAINT [PK_Param] PRIMARY KEY CLUSTERED 
(
	[idParam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

