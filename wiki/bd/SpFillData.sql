USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFillData]    Script Date: 26-09-2017 8:15:54 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

/***********************************************************************************************************************
* Nombre del Procedimiento: 
* Descripción: 
* Autor: 
* Fecha de Creación: 
* Base de Datos: Clientes
***********************************************************************************************************************/
/* Comando Prueba    : EXEC 
***********************************************************************************************************************
* Modificaciones:   
***********************************************************************************************************************
* Modificación:
* Fecha           :
* Programador:
* Motivo          :
***********************************************************************************************************************/

CREATE PROCEDURE [dbo].[SpFillData]

@id_cliente decimal(18,0),
@idParam int,
@value varchar(255)


AS

BEGIN

	INSERT INTO ClienteParam(id_cliente, idParam, value, creationDate)
	VALUES (@id_cliente, @idParam, @value, getdate())

	-- Last ID inserted
	SELECT SCOPE_IDENTITY()
END
GO

