CREATE PROC [dbo].[spGetCorporateRequesters]
(
	@CompanyName NVARCHAR(200)
)
AS
BEGIN
	SELECT SponsorName+' - '+FirstName +' '+ LastName AS CorporateName,Id
	FROM CorporateRequests WITH(NOLOCK) 
	WHERE SponsorName LIKE '%'+@CompanyName+'%'
	ORDER BY SponsorName ASC
END
