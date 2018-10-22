USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpCreateCredential]    Script Date: 26-09-2017 8:15:18 ******/
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
/* Comando Prueba    : EXEC SpCreateCredential 1,1,'abc123'
***********************************************************************************************************************
* Modificaciones:   
***********************************************************************************************************************
* Modificación:
* Fecha           :
* Programador:
* Motivo          :
***********************************************************************************************************************/

CREATE PROCEDURE [dbo].[SpCreateCredential]

@idProduct			int,
@idUserIdentify		int,
@password			varchar(512)

AS

BEGIN
	
	DECLARE @idState INT
	SET @idState = 1

	IF @password IS NOT NULL
		BEGIN
			INSERT INTO Credential(idProduct, idUserIdentify, idState, password, creationDate)
			VALUES (@idProduct, @idUserIdentify, @idState, @password, getdate())
		END
	ELSE
		BEGIN
			INSERT INTO Credential(idProduct, idUserIdentify, idState, creationDate)
			VALUES (@idProduct, @idUserIdentify, @idState, getdate())
		END

	SELECT SCOPE_IDENTITY()

END
GO

