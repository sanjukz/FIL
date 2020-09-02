CREATE PROCEDURE [dbo].[Bo_GetQtyForChildAndSrCitizen]  --132396  
(  
	@RetailerId BIGINT  
)  
AS  
BEGIN  
	SELECT ISNULL(ChildTicketLimit,0) AS NoOfChild,ISNULL(ChildForPerson,0) AS NoOfPerson, 
	ISNULL(SrCitizenLimit,0) AS NoOfSrCitizen, ISNULL(SrCitizenPerson,0) AS NoOfSrCitizenPerson 
	FROM BoxofficeUserAdditionalDetails WITH(NOLOCK) WHERE UserId=@RetailerId  
END  