USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFindCredentialsByProductAndUserIdentify]    Script Date: 26-09-2017 8:16:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Digevo, Gastón Orellana C.
-- Create date: 26/07/2017
-- Description:	Find all credentials related to user in function
-- idChannel and puntual Value
-- =============================================
CREATE PROCEDURE [dbo].[SpFindCredentialsByProductAndUserIdentify]
	-- Add the parameters for the stored procedure here
	@idProduct int,
	@idUserIdentify int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT idCredential, idProduct, idUserIdentify, idState, password, creationDate, modificationDate
	FROM Credential
	WHERE idProduct = @idProduct
		AND idUserIdentify = @idUserIdentify

END

GO

