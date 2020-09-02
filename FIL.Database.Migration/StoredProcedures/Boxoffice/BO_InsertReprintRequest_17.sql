CREATE PROC [dbo].[BO_InsertReprintRequest_17] ---201363102,'Test','Test',257163                                      
(                                                            
	@TransID bigint,                         
	@UserName varchar(100),                         
	@Comments varchar(200),                  
	@Retailer_Id bigint = null                      
)                        
AS                                                            
BEGIN        
DECLARE @AltId NVARCHAR(500), @RePrintRequestId BIGINT
SELECT @AltId = AltId FROM Users  WITH(NOLOCK) WHERE Id = @Retailer_Id

IF NOT EXISTS (SELECT A.Id FROM TransactionDetails A WITH(NOLOCK) 
INNER JOIN TransactionDeliveryDetails B WITH(NOLOCK) On A.Id = B.TransactionDetailId
 WHERE TransactionId =@TransId  AND DeliveryTypeId<>1)        
BEGIN  
	 INSERT INTO ReprintRequests( UserId,TransactionId,RequestDateTime,Remarks,IsApproved,ApprovedBy,ApprovedDateTime,
	 ModuleId,CreatedUtc,CreatedBy)
	 VALUES(@Retailer_Id, @TransID, GETUTCDATE(),@Comments, 0, NULL, NULL,1,GETUTCDATE(), @AltId)
	 SET @RePrintRequestId = SCOPE_IDENTITY()
	 DECLARE @tblTicNumbers TABLE(Sno INT IDENTITY(1,1),TicNumId BIGINT, barcode NVARCHAR(500))

	INSERT INTO @tblTicNumbers
	SELECT Id, BarcodeNumber FROM
	MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId  = @TransID
	DECLARE @Count INT, @Counter INT = 1, @TicNumId BIGINT, @Barcode NVARCHAR(200)
	SELECT @Count = COUNT(*) FROm @tblTicNumbers

	WHILE(@Counter<=@Count)
	BEGIN
	    SELECT @TicNumId = TicNumId, @Barcode = barcode FROm @tblTicNumbers WHERE Sno = @Counter
		IF NOT EXISTS (SELECT Id FROM  RePrintRequestDetails RR WITH(NOLOCK) 
		WHERE RR.RePrintRequestId=@RePrintRequestId AND ISNULL(IsRePrinted,0) =0 AND BarcodeNumber = @barcode)
		BEGIN
			INSERT INTO RePrintRequestDetails(RePrintRequestId,MatchSeatTicketDetaildId,BarcodeNumber,
			IsRePrinted,RePrintCount,CreatedUtc,CreatedBy)
			VALUES(@RePrintRequestId, @TicNumID, @barcode,0,0,GETUTCDATE(), @AltId)
	     END
		 SET @Counter = @Counter + 1
	END
	SELECT 1
 END       
ELSE      
BEGIN       
	SELECT 3      
-- Access denied      
END 
      
END  