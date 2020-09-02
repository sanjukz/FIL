CREATE proc [dbo].[BO_ShowReprintRequestBarcode]    --138773         
(            
	@RetailerId BIGINT      
)            
AS            
BEGIN
	DECLARE @AltId NVARCHAR(500)
	SELECT @AltId = AltId FROM Users WITH(NOLOCK) WHERE Id = @RetailerId
	
	SELECT MD.Id AS Id,RR.TransactionId AS TransId,U.FirstName AS UserName,RequestDateTime AS ReqDateTime,
	Remarks AS Comments, (CASE  WHEN MD.IsApproved = 1 THEN 'Yes' ELSE 'No' END) AS IsApproved,            
	MD.RePrintCount AS Reprint_Count,(CASE  WHEN MD.IsRePrinted = 1 THEN 'Yes' ELSE 'No' END) AS IsReprinted,
	MD.BarcodeNumber AS Barcode
	FROM ReprintRequests RR WITH(NOLOCK)
	INNER JOIN RePrintRequestDetails MD WITH(NOLOCK) on RR.Id=Md.ReprintRequestId --AND MD.IsApproved <>1
	INNER JOIN Users U WITH(NOLOCK) ON RR.CreatedBy = U.AltId
	WHERE RR.CreatedBy= @AltId   
	ORDER BY RequestDateTime DESC            
End 



