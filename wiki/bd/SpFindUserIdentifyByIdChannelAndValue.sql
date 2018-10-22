USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFindUserIdentifyByIdChannelAndValue]    Script Date: 26-09-2017 8:16:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Digevo, Gastón Orellana C.
-- Create date: 26/07/2017
-- Description:	Find all identifies related to user in function
-- idChannel and puntual Value
-- =============================================
CREATE PROCEDURE [dbo].[SpFindUserIdentifyByIdChannelAndValue]
	-- Add the parameters for the stored procedure here
	@idChannel int, 
	@value varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT idUserIdentify, id_cliente, idChannel, value, creationDate
	FROM UserIdentify
	WHERE idChannel = @idChannel AND cast(value as varchar(max)) = @value

END

GO

