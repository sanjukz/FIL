CREATE PROC [dbo].[spGetCorporateRequestDetails] --3
(
	@CorporateRequestId BIGINT
)
AS
BEGIN

	SELECT DISTINCT A.Id AS CorpOrderId,A.SponsorName AS CorpName,
	A.FirstName +' '+A.LastName AS RepName,A.PhoneCode+'-'+A.PhoneNumber AS MobNo,
	A.Email AS EmailId,A.Address As Address , F.Name AS EventCatName, ISNULL(TermsAndConditions,'') AS TCPageUrl,
	PickupRepresentativeFirstName +' '+  PickupRepresentativeLastName As PickupRepresentativeName,
	PickupRepresentativeEmail,
	PickupRepresentativePhoneCode+'-'+PickupRepresentativePhoneNumber AS PickupRepresentativePhoneNumber,
	RequestOrderType AS IsCompTicket
	FROM CorporateRequests A WITH(NOLOCK)
	INNER JOIN CorporateRequestDetails B WITH(NOLOCK) ON A.Id=B.CorporateRequestId
	INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.EventTicketAttributeId=C.Id
	INNER JOIN EventTicketDetails D WITH(NOLOCK) ON C.EventTicketDetailId=D.Id
	INNER JOIN EventDetails E WITH(NOLOCK) ON D.EventDetailId=E.Id
	INNER JOIN Events F WITH(NOLOCK) ON E.EventId=F.Id
	WHERE A.Id=@CorporateRequestId

	SELECT DISTINCT E.Name  +' ('+CONVERT(VARCHAR(17),E.StartDateTime,113) +')' AS EventName ,
	 TC.Name AS VenueCatName, RequestedTickets AS NoOfTic,
	ISNULL(B.Price, C.Price) AS PricePerTic,
	dbo.spGetCorporateRequestSeatNumbers(B.Id) AS SeatNubmbers,
	CT.Code AS Currency,E.VenueId AS EventVenue,E.StartDateTime,
	RequestOrderType AS IsCompTicket
	FROM CorporateRequests A WITH(NOLOCK)
	INNER JOIN CorporateRequestDetails B WITH(NOLOCK) ON A.Id=B.CorporateRequestId
	INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.EventTicketAttributeId=C.Id
	INNER JOIN EventTicketDetails D WITH(NOLOCK) ON C.EventTicketDetailId=D.Id
	INNER JOIN EventDetails E WITH(NOLOCK) ON D.EventDetailId=E.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON D.TicketCategoryId=TC.Id
	INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON C.CurrencyId=CT.Id
	WHERE A.Id=@CorporateRequestId
	ORDER BY E.StartDateTime
END
