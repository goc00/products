USE [DigevoUsers]
GO

/****** Object:  StoredProcedure [dbo].[SpFindCustodyByCode]    Script Date: 26-09-2017 8:16:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Digevo, Gastón Orellana C.
-- Create date: 02/08/2017
-- Description:	---
-- =============================================
CREATE PROCEDURE [dbo].[SpFindCustodyByCode]
	-- Add the parameters for the stored procedure here
	@code varchar(512)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT idCustody
      ,idProduct
      ,id_cliente
      ,value
      ,code
      ,active
      ,expirationDate
      ,creationDate
	FROM Custody
	WHERE code = @code

END

GO

