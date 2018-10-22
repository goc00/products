USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFindCliente]    Script Date: 26-09-2017 8:16:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpFindCliente]

@id_cliente decimal(18,0)

AS

BEGIN

 SELECT id_cliente AS id_cliente,
   ani  AS ani,
   email  AS email, 
   usuario  AS usuario,
   pass  AS pass,
   id_operador AS id_operador

 FROM CLIENTE

 WHERE id_cliente = @id_cliente

END
GO

