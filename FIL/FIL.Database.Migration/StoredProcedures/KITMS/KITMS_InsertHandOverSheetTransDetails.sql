CREATE PROC [dbo].[KITMS_InsertHandOverSheetTransDetails]  
(  
   @TransId BIGINT,  
   @QuantityPrinted INT,  
   @SerialFrom VARCHAR(50),  
   @SerialTo VARCHAR(50),  
   @HandedBy VARCHAR(500),  
   @SubmittedTo VARCHAR(500),  
   @IsPartialPrinting INT,
   @CreatedBy VARCHAR(500)
)  
AS   
BEGIN  
	DECLARE @AltId VARCHAR(500)
	SELECT @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName = @CreatedBy
	INSERT INTO HandoverSheets(TransactionId, SerialStart, SerialEnd, TicketHandedBy, TicketHandedTo, IsEnabled, CreatedUtc, CreatedBy)
	VALUES(@TransId,@SerialFrom,@SerialTo,@HandedBy,@SubmittedTo,1,GETDATE(),@AltId)
END 