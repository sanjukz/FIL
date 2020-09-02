CREATE proc [dbo].[BO_ApproveReprint]                                         
(                                            
	@IsApproved bigint,        
	@ID int         
)        
as                                            
BEGIN
	DECLARE @ReprintRequestId BIGINT
	SELECT @ReprintRequestId = ReprintRequestId FROm ReprintRequestDetails WITH(NOLOCK) WHERE  Id=@ID
	Update ReprintRequestDetails  SET IsApproved = @IsApproved, ApprovedDateTime = GETUTCDATE() WHERE Id=@ID
	IF NOT EXISTS(SELECT Id FROm ReprintRequestDetails WITH(NOLOCK) WHERE  ReprintRequestId = @ReprintRequestId AND ISNULL(IsApproved,0) = 0) 
	BEGIN
		UPDATE ReprintRequests  SET IsApproved = @IsApproved, ApprovedDateTime = GETUTCDATE() WHERE Id=@ReprintRequestId     
	END
End 
