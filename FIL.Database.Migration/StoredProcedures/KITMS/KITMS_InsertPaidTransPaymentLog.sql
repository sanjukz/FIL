CREATE PROC [dbo].[KITMS_InsertPaidTransPaymentLog]
(
	@EventTransId BIGINT,
	@PaymentType VARCHAR(100),
	@PaymentReceivedIn VARCHAR(500),
	@PaymentConfirmedBy VARCHAR(100),
	@PaymentApprovedDate VARCHAR(500),
	@ChequeNumber VARCHAR(100),
	@DrawnonBank VARCHAR(100),
	@ChequeDate VARCHAR(100),
	@ApprovalEmailFrom VARCHAR(2000),
	@ApprovalEmailReceivedOn VARCHAR(100),
	@ReferenceNumber VARCHAR(100)
)
AS
BEGIN
	DECLARE @PaymentTypeId INT, @AltId NVARCHAR(500), @TransactingOptionId INT
	SET @PaymentTypeId = CASE WHEN @PaymentType ='Bank Transfer' THEN 1
	WHEN @PaymentType ='Cash' THEN 2
	WHEN @PaymentType ='CC/DD' THEN 3
	WHEN @PaymentType ='Cheque' THEN 4
	WHEN @PaymentType ='Credit Card' THEN 5
	WHEN @PaymentType ='Sale on Credit' THEN 6
	WHEN @PaymentType ='Payment with Client' THEN 7
	WHEN @PaymentType ='Retail - Printed to be sold' THEN 8
	ELSE 0 END
	IF(@PaymentTypeId=0)
	BEGIN
		SELECT @PaymentTypeId = Id FROm PaymentTypes WITH(NOLOCK) WHERE PaymentType = @PaymentType
	END

	SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id =@EventTransId
	SELECT  @TransactingOptionId = TransactingOptionId FROM CorporateTransactionDetails WITH(NOLOCK) WHERE TransactionId =@EventTransId

	INSERT INTO CorporateTransactionPaymentDetails(AltId, TransactionId,TransactingOptionId,PaymentTypeId,PaymentReceivedIn,PaymentConfirmedBy,PaymentApprovedUtc,
	ChequeNumber,ChequeDrawnonBank, ChequeUtc,ApprovalEmailFrom,ApprovalEmailReceivedOn,BankReferenceNumber,IsEnabled,CreatedUtc,CreatedBy)
	VALUES(NEWID(), @EventTransId,@TransactingOptionId,@PaymentTypeId,@PaymentReceivedIn,@PaymentConfirmedBy, CONVERT(DATETIME,@PaymentApprovedDate), 
	@ChequeNumber,@DrawnonBank,CONVERT(DATETIME,@ChequeDate),
	@ApprovalEmailFrom, CONVERT(DATETIME,@ApprovalEmailReceivedOn),@ReferenceNumber,1,GETDATE(),@AltId)

	UPDATE TransactionPaymentDetails SET PayConfNumber = @PaymentType  WHERE TransactionId =@EventTransId
END