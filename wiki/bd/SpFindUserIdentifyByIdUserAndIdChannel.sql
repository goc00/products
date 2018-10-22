USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFindUserIdentifyByIdUserAndIdChannel]    Script Date: 26-09-2017 8:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpFindUserIdentifyByIdUserAndIdChannel]
	-- Add the parameters for the stored procedure here
	@id_cliente		decimal(18,0), 
	@idChannel		int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT idUserIdentify, id_cliente, idChannel, value, creationDate
	FROM UserIdentify
	WHERE id_cliente = @id_cliente AND idChannel = @idChannel

END

GO

