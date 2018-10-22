USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpUpdatePasswordCredential]    Script Date: 26-09-2017 8:18:20 ******/
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
/* Comando Prueba    : EXEC SpUpdatePasswordCredential
***********************************************************************************************************************
* Modificaciones:   
***********************************************************************************************************************
* Modificación:
* Fecha           :
* Programador:
* Motivo          :
***********************************************************************************************************************/

CREATE PROCEDURE [dbo].[SpUpdatePasswordCredential]

@idCredential		int,
@newPass			varchar(512),
@idState			int

AS

BEGIN

	IF @idState <> 0
		BEGIN
			UPDATE Credential
			SET password = @newPass,
				idState = @idState,
				modificationDate = getdate()
			WHERE idCredential = @idCredential
		END
	ELSE
		BEGIN
			UPDATE Credential
			SET password = @newPass,
				modificationDate = getdate()
			WHERE idCredential = @idCredential
		END

END
GO

