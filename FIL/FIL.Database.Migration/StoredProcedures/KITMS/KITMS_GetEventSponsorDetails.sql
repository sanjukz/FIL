CREATE PROC [dbo].[KITMS_GetEventSponsorDetails]
(
	@EventCatId BIGINT,
	@SponsorId INT
)
AS
BEGIN
	SELECT Name AS EventCatName FROM Events WITH(NOLOCK) WHERE Id=@EventCatId
	SELECT SponsorName,FirstName,LastName, Email AS EmailId,PhoneCode+'-'+PhoneNumber AS MobileNo
	FROM Sponsors WITH(NOLOCK) WHERE Id=@SponsorId
END