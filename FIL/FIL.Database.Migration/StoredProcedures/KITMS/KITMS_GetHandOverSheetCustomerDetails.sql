CREATE PROC [dbo].[KITMS_GetHandOverSheetCustomerDetails]
(                            
	@TransId BIGINT    
)                      
AS                              
BEGIN                   
	SELECT DISTINCT SM.SponsorName AS SponsorName,SM.Id AS SponsorId ,FirstName, LastName,Email,
	SM.PhoneCode+'-'+PhoneNumber AS MobileNo FROM                           
	Sponsors SM WITH(NOLOCK)
	INNER JOIN CorporateTransactionDetails ST WITH(NOLOCK) ON ST.SponsorId=SM.Id                        
	WHERE ST.TransactionId=@TransId
END 