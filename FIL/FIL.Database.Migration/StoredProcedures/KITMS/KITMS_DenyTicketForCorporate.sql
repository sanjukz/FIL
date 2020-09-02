CREATE PROC [dbo].[KITMS_DenyTicketForCorporate]
(
	@CorpMapId bigint,
	@ApprovedBy int
)
As
BEGIN
	DECLARE @ReturnVal VARCHAR(200)
	UPDATE CorporateRequestDetails
	SET ApprovedStatus = 2, ApprovedBy = (SELECT TOP 1 AltId FROM Users WITH (NOLOCK) WHERE Id = @ApprovedBy), ApprovedUtc = GETUTCDATE()
	WHERE Id = @CorpMapId

	SET @ReturnVal = 'Order Denied Successfully!'
	SELECT @ReturnVal
END
