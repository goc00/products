USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[pa_InsCliente2]    Script Date: 26-09-2017 8:13:55 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

/***********************************************************************************************************************
* Nombre del Procedimiento: pa_InsCliente
* Descripción: Inserta un registro en la tabla CLIENTE
* Autor: Fabián Novales
* Fecha de Creación: 08/01/2007
* Base de Datos: Clientes
***********************************************************************************************************************/
/* Comando Prueba    : EXEC pa_InsCliente '1','email','fabian','1','1'
***********************************************************************************************************************
* Modificaciones:   
***********************************************************************************************************************
* Modificación:
* Fecha           :
* Programador:
* Motivo          :
***********************************************************************************************************************/

CREATE PROCEDURE [dbo].[pa_InsCliente2]

@ani  varchar(64),
@email  varchar(128),
@usuario varchar(64),
@pass  varchar(64),
@id_operador decimal(18,0)

AS

BEGIN


   INSERT INTO CLIENTE (ani,email,usuario,pass,id_operador, fechahora_alta)
   VALUES (@ani,@email,@usuario,@pass,@id_operador, getdate())
 

  SELECT SCOPE_IDENTITY()

END
GO

