CREATE PROC [dbo].[KITMS_InsertRePrintTransLog]
(
	@TransId BIGINT,
	@RePrintReason VARCHAR(500),
	@OtherReason VARCHAR(500),
	@UpdatedBy VARCHAR(500)
)
AS
BEGIN

IF(@OtherReason<>'')
BEGIN
	SET @RePrintReason = @RePrintReason +'-'+@OtherReason
END

DECLARE @AltId VARCHAR(500), @USerId BIGINT
SELECT @AltId = AltId, @USerId= Id FROm Users WITH(NOLOCK) WHERE UserName =@UpdatedBy
INSERT INTO ReprintRequests(UserId,TransactionId,RequestDateTime,Remarks,IsApproved,ApprovedBy,
ApprovedDateTime,ModuleId,CreatedUtc,CreatedBy)
VALUES(@USerId, @TransId, GETUTCDATE(),@RePrintReason,	0, NULL,NULL,3,GETUTCDATE(),@AltId)
 
END