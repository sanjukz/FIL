CREATE PROC [dbo].[BO_GetGetIdentityProofList]            
(            
	@Retailer_Id BIGINT
)            
AS            
BEGIN            
	SELECT 1 AS IdentityProofID,'Passport' AS IdentityProofName
	UNION
	SELECT 2 AS IdentityProofID,'Drivers License' AS IdentityProofName
	UNION
	SELECT 3 AS IdentityProofID,'Student Card' AS IdentityProofName   
	UNION
	SELECT 4 AS IdentityProofID,'Country/National ID' AS IdentityProofName       
END