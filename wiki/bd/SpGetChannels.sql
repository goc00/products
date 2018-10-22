USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpGetChannels]    Script Date: 26-09-2017 8:17:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpGetChannels]

AS

BEGIN

 SELECT idChannel, name, description
 FROM Channel

END
GO

