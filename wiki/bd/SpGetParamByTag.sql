USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpGetParamByTag]    Script Date: 26-09-2017 8:17:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpGetParamByTag]

@tag varchar(24)

AS

BEGIN

 SELECT idParam,name,description,tag,creationDate,modificationDate
 FROM Param
 WHERE tag = @tag

END
GO

