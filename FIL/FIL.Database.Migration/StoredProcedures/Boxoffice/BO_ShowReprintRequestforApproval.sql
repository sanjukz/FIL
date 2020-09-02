CREATE proc [dbo].[BO_ShowReprintRequestforApproval]  --4525               
(            
	@RetailerId BIGINT=NULL            
)              
AS                  
BEGIN
	SELECT MD.Id AS Id,RR.TransactionId AS TransId,U.FirstName AS [UserName],
	CONVERT( VARCHAR(100),RequestDateTime,20) AS ReqDateTime, Remarks as Comments,
	(CASE  WHEN RR.IsApproved = 1 THEN 'Yes' ELSE 'No' END) AS IsApproved,            
	MD.RePrintCount as Reprint_Count,
	(CASE  WHEN MD.IsRePrinted = 1 THEN 'Yes' ELSE 'No' END) AS IsReprinted,
	MD.BarcodeNumber as Barcode
	from ReprintRequests RR WITH(NOLOCK) 
	Inner join RePrintRequestDetails MD WITH(NOLOCK) on RR.Id=Md.ReprintRequestId
	INNER JOIN Users U WITH(NOLOCK) ON RR.CreatedBy = U.AltId
	WHERE RR.CreatedBy IN (SELECT AltId  FROm USers WITH(NOLOCK) WHERE Id IN(SELECT UserId FROM
	BoxofficeUserAdditionalDetails WITH(NOLOCK) WHERE ParentId = @RetailerId)) AND ISNULL(MD.IsApproved,0) = 0
	ORDER BY RequestDateTime DESC
END
