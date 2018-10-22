USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpGetByIdClienteAndIdParam]    Script Date: 26-09-2017 8:17:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpGetByIdClienteAndIdParam]

@id_cliente decimal(18,0),
@idParam int

AS

BEGIN

 SELECT idClienteParam,id_cliente,idParam,value,creationDate,modificationDate
 FROM ClienteParam
 WHERE id_cliente = @id_cliente AND idParam = @idParam

END
GO

