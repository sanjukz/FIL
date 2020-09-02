CREATE proc [dbo].[KITMS_UpdateInvoiceEmailDetails]
(
	@CorpMapIds nvarchar(200),
	@MailTo  nvarchar(200),
	@MailCC nvarchar(200),
	@MailBCC nvarchar(200)
)
AS
BEGIN
	UPDATE InvoiceDetails
	SET SentToEmail=@MailTo,SentCcEmail=@MailCC, SentBccEmail=@MailBCC ,SentUtc=GETUTCDATE() 
	WHERE CorporateRequestDetailId in(SELECT * FROM SplitString(@CorpMapIds,'-')) AND IsEnabled=1
	SELECT '1'
END