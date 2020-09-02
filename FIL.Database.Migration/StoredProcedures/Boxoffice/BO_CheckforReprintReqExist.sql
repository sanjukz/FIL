CREATE proc [dbo].[BO_CheckforReprintReqExist]        
(        
  @TransId BIGINT        
)        
AS        
BEGIN        
	SELECT * FROM  ReprintRequests RR WITH(NOLOCK) 
	WHERE RR.TransactionId=@TransId AND ISNULL(IsApproved,0) =0
END