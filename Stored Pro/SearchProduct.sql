USE [CRMDB]
GO
/****** Object:  StoredProcedure [dbo].[SearchProduct]    Script Date: 8/16/2022 3:41:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchProduct]
	@Search nvarchar(max)
AS
BEGIN
 
SELECT        Name AS [نام محصول], Price AS [قیمت محصول], Stock AS موجودی
FROM            dbo.Products
WHERE        (Name like '%'+@Search+'%') AND (DeleteStatus = 0) OR (Stock like '%'+@Search+'%')

END
