USE [CRMDB]
GO
/****** Object:  StoredProcedure [dbo].[SearchCustomer]    Script Date: 8/16/2022 3:41:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchCustomer]
	@Search nvarchar(max)
AS
BEGIN

SELECT        Name AS [نام مشتری], Phone AS [شماره تماس], RegDate AS [تاریخ ثبت]
FROM            dbo.Customers
WHERE        (DeleteStatus = 0) AND (Name like '%'+@Search+'%') OR (Phone like '%'+@Search+'%')

END
