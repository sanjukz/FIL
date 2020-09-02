CREATE PROC [dbo].[KITMS_GetLatestAutoInvoiceId]
AS
BEGIN
	SELECT MAX(AttributeNumber) AS AutoInvoiceId FROM InvoiceDetails WITH(NOLOCK)
END