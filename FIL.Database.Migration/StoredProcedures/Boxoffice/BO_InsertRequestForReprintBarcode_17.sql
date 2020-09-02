CREATE proc [dbo].[BO_InsertRequestForReprintBarcode_17]                                                     
(                                                        
	@TransID bigint,                     
	@UserName varchar(100),                     
	@Comments varchar(200),                    
	@barcode varchar(200),                
	@Retailer_Id bigint = null,              
	@TicNumID bigint = null                    
)                    
AS                                                        
BEGIN        
 DECLARE @AltId NVARCHAR(500), @RePrintRequestId BIGINT  
 SELECT @AltId = AltId FROM Users WITH(NOLOCK) WHERE Id = @Retailer_Id  
  
 IF NOT EXISTS (SELECT A.Id FROM TransactionDetails A WITH(NOLOCK)   
 INNER JOIN TransactionDeliveryDetails B WITH(NOLOCK) On A.Id = B.TransactionDetailId  
  WHERE TransactionId =@TransId  AND DeliveryTypeId<>1)          
 BEGIN  
    
  IF NOT EXISTS (SELECT Id FROM  ReprintRequests RR WITH(NOLOCK)  
  WHERE RR.TransactionId=@TransId AND ISNULL(IsApproved,0) =0)  
  BEGIN  
   INSERT INTO ReprintRequests( UserId,TransactionId,RequestDateTime,Remarks,IsApproved,ApprovedBy,ApprovedDateTime,  
    ModuleId,CreatedUtc,CreatedBy)  
    VALUES(@Retailer_Id, @TransID, GETUTCDATE(),@Comments, 0, NULL, NULL,1,GETUTCDATE(), @AltId)  
   SET @RePrintRequestId = SCOPE_IDENTITY()  
  END  
  ELSE  
  BEGIN  
   SELECT @RePrintRequestId = Id FROM  ReprintRequests RR WITH(NOLOCK)  
   WHERE RR.TransactionId=@TransId AND ISNULL(IsApproved,0) =0  
  END  
  IF NOT EXISTS (SELECT Id FROM  RePrintRequestDetails RR WITH(NOLOCK) 
  WHERE RR.RePrintRequestId=@RePrintRequestId AND RR.MatchSeatTicketDetaildId=@TicNumID and ISNULL(IsRePrinted,0) =0)  
  BEGIN  
    INSERT INTO RePrintRequestDetails(RePrintRequestId,MatchSeatTicketDetaildId,BarcodeNumber,  
    IsRePrinted,RePrintCount,CreatedUtc,CreatedBy)  
    VALUES(@RePrintRequestId, @TicNumID, @barcode,0,0,GETUTCDATE(), @AltId)  
   END  
   SELECT 1  
  END  
 ELSE        
 BEGIN         
  SELECT 3        
 -- Access denied        
 END  
END   