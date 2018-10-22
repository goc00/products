USE [DigevoUsers]
GO

/****** Object:  Table [dbo].[ClienteParam]    Script Date: 26-09-2017 8:12:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClienteParam](
	[idClienteParam] [int] IDENTITY(1,1) NOT NULL,
	[id_cliente] [decimal](18, 0) NOT NULL,
	[idParam] [int] NOT NULL,
	[value] [varchar](255) NOT NULL,
	[creationDate] [datetime] NOT NULL,
	[modificationDate] [datetime] NULL,
 CONSTRAINT [PK_ClienteParam] PRIMARY KEY CLUSTERED 
(
	[idClienteParam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ClienteParam]  WITH CHECK ADD  CONSTRAINT [FK_ClienteParam_ClienteParam] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[cliente] ([id_cliente])
GO

ALTER TABLE [dbo].[ClienteParam] CHECK CONSTRAINT [FK_ClienteParam_ClienteParam]
GO

ALTER TABLE [dbo].[ClienteParam]  WITH CHECK ADD  CONSTRAINT [FK_ClienteParam_Param] FOREIGN KEY([idParam])
REFERENCES [dbo].[Param] ([idParam])
GO

ALTER TABLE [dbo].[ClienteParam] CHECK CONSTRAINT [FK_ClienteParam_Param]
GO

