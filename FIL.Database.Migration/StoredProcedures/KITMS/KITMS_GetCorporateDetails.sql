CREATE PROC [dbo].[KITMS_GetCorporateDetails]-- 2180,0,0,3
(
--	DECLARE
	@EventCatId BIGINT=0,
	@VenueId BIGINT=0,
	@EventId BIGINT=0,
	@UserId BIGINT=9
)
AS
BEGIN
DECLARE @TblEventId TABLE (EventId BIGINT)

	IF(@EventId=0)
	BEGIN
		INSERT INTO @TblEventId SELECT DISTINCT ED.Id FROM UserVenueMappings KVR WITH(NOLOCK)
		INNER JOIN EventDetails ED  WITH(NOLOCK) ON ED.VenueId=KVR.VenueId WHERE UserId=@UserId and 
		EventId=CASE @EventCatId WHEN 0 THEN ED.EventId ELSE @EventCatId END 
		AND KVR.VenueId= CASE @VenueId WHEN 0 THEN KVR.VenueId ELSE @VenueId END
	END
	ELSE
	BEGIN
		INSERT INTO @TblEventId VALUES(@EventId)
	END
	
	SELECT ED.VenueId AS EventVenue, ED.Id AS EventId, CR.Id AS CorpOrderId, SponsorName AS CorpName,
	FirstName +' '+ LastName AS RepName, PhoneCode +' '+ PhoneNumber AS MobNo,Email AS EmailId,
	Address, Country AS CountryName, ED.Name AS EventName,
	CONVERT(NVARCHAR(17),ED.StartDateTime,113) AS EventStartDate, TC.Name AS VenueCatName, 
	CRD.RequestedTickets AS NoOfTic, CRD.Price AS PricePerTic, ISNULL(ApprovedTickets,0) AS ApprovedQuantity,
	ETA.RemainingTicketForSale AS TotalTic, CRD.EventTicketAttributeid AS VMCC_Id,
	CASE CRD.Approvedstatus WHEN 3 THEN 'Exchange' WHEN 4 THEN 'Partial Exchange'  
	WHEN  0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Denied' 
	WHEN -1 THEN 'Original before Exchange' END AS ApproveStatus, PickupRepresentativeEmail AS PickUpRepEmailId,
	PickupRepresentativePhoneCode +' '+ PickupRepresentativePhoneNumber AS PickUpRepMobNo, CRD.Id AS CorpMapId,
	CASE ISNULL(RequestOrderType,0) WHEN  0 THEN 'Paid' ELSE 'Complimentary' END AS IsCompOrder,
	ISNULL((SELECT UserName FROM Users WHERE  AltId  = CR.CreatedBy),'') AS LoginUserName,
	CASE ISNULL(PickupRepresentativeFirstName,'') WHEN '' THEN 'Self' ELSE PickupRepresentativeFirstName +' '+ 
	PickupRepresentativeLastName END AS PickUpRepName, 
	CASE WHEN ISNULL(IND.Id,'') ='' THEN '' ELSE CONVERT(VARCHAR(200),IND.Id) END AS InvoiceStatus,'' AS OldMapId,
	CTM.Name AS VenueAddress
	FROM CorporateRequests CR  WITH(NOLOCK)
	INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id
	LEFT OUTER JOIN InvoiceDetails IND ON CRD.Id=IND.CorporateRequestDetailId 
	INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON ETA.CurrencyId = CM.Id  
	INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id  
	INNER JOIN Cities CTM WITH(NOLOCK) ON VD.CityId = CTM.Id
	WHERE ED.Id IN (SELECT EventId FROM @TblEventId) 
	ORDER BY CR.Id DESC, StartDateTime ASC,CRD.ApprovedStatus DESC

END
