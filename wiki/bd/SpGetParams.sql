USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpGetParams]    Script Date: 26-09-2017 8:17:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpGetParams]

AS

BEGIN

 SELECT idParam,name,description,tag,creationDate,modificationDate
 FROM Param

END
GO

