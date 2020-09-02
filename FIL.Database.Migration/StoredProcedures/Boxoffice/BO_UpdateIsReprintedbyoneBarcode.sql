CREATE PROC [dbo].[BO_UpdateIsReprintedbyoneBarcode]                                             
(                                                
	@TransID BIGINT,            
	@barcode VARCHAR(100)            
)            
AS                                                
BEGIN  
DECLARE @ReprintRequestId BIGINT
SELECT @ReprintRequestId = ReprintRequestId FROM ReprintRequestDetails A WITH(NOLOCK)
INNER JOIN ReprintRequests B WITH(NOLOCK) ON A.ReprintRequestId = B.Id
WHERE  TransactionId=@TransID AND BarcodeNumber = @barcode

IF(@barcode = '0' or @barcode = '')     
BEGIN
  UPDATE ReprintRequestDetails  SET IsRePrinted= 1, RePrintCount= RePrintCount +1 WHERE ReprintRequestId = @ReprintRequestId  
  AND IsApproved=1  
END
ELSE
BEGIN         
	UPDATE ReprintRequestDetails  SET IsRePrinted= 1, RePrintCount= RePrintCount +1 WHERE ReprintRequestId = @ReprintRequestId  
    AND BarcodeNumber = @barcode and IsApproved=1 
 END   
END 