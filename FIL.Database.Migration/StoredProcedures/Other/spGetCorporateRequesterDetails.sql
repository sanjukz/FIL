CREATE PROC [dbo].[spGetCorporateRequesterDetails] --31
(
	@CorporateRequestId BIGINT
)
AS
BEGIN
	SELECT A.Id AS CorpOrderId, SponsorName AS CorpName, A.FirstName, 	A.LastName,	A.PhoneCode,
	A.PhoneNumber, Email AS EmailId, 
	ISNULL(Address,'') AS Address,
	ISNULL(City,'') AS City, 
	ISNULL(State,'') AS State, 
	ISNULL(Country,0) AS Country,
	ISNULL(ZipCode,'') AS ZipCode,
	PickupRepresentativeFirstName,
	PickupRepresentativeLastName,
	PickupRepresentativeEmail, 
	CASE WHEN PickupRepresentativePhoneCode<>'' THEN 0 ELSE PickupRepresentativePhoneCode END
	AS PickupRepresentativePhoneCode,
	PickupRepresentativePhoneNumber,
	ISNULL(A.Classification,'') AS Classification	
	FROM CorporateRequests A WITH(NOLOCK)
	WHERE A.Id=@CorporateRequestId
END
