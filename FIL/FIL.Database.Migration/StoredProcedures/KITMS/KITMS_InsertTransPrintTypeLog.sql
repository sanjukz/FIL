CREATE  PROC [dbo].[KITMS_InsertTransPrintTypeLog]
(
  @TransId BIGINT,
  @PrintType VARCHAR(100),
  @IsPartialPrinting INT,
  @IsSponsorName INT,
  @UpdatedBy VARCHAR(100)
)
AS   
BEGIN  
	DECLARE @AltId VARCHAR(500), @TicketPrintingOptionId SMALLINT
	SELECT @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName = @UpdatedBy
	SET @TicketPrintingOptionId  = CASE WHEN @PrintType ='Zero Price' THEN 5
			  WHEN @PrintType ='Price' THEN 2
			  WHEN @PrintType ='Hospitality' THEN 6
			  WHEN @PrintType ='GovernmentComplimentary' THEN 7
			  WHEN @PrintType ='Complimentary + Zero Price (local)' THEN 4
			  WHEN @PrintType ='Complimentary' THEN 3
			  WHEN @PrintType ='ChildPrice' THEN 9
			  WHEN @PrintType ='ChildComplimentary' THEN 8
			  WHEN @PrintType ='Blank' THEN 1
		END

	INSERT INTO CorporateTicketPrintingLogs(TransactionId, TicketPrintingOptionId, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@TransId,@TicketPrintingOptionId,1,GETDATE(),@AltId)
END 