CREATE PROC [dbo].[KITMS_GetPreConfirmationDetailsByVenue]                                                       
(                                                                          
	@TransId BIGINT,  
	@SponsorId INT                                                                          
)                                                                          
AS 
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, EA.Id AS VmCC_Id, V.Name +', '+ C.Name AS VenueAddress,CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN STD.TransactingOptionId =1 THEN 'Paid' ELSE 'Complimentary' END AS PGType, V.Id AS VenueId,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME))  AS EventStartTime,T.ServiceCharge AS ServiceTax, T.ConvenienceCharges AS ConvenienceCharge, 0 AS Othercharge,
	ED.Id AS EventId, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType, S.PhoneCode+'-'+ S.PhoneNumber AS ContactNumber,
	T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, S.FirstName AS Cust_FirstName, S.LastName As Cust_LastName, '' AS Cust_IdType, '' AS Cust_IdTypeNumber,
	S.PhoneCode+'-'+ S.PhoneNumber AS Cust_MobileNumber, S.Email AS Cust_Email,
	TC.Name AS VenueCatName, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, GrossTicketAmount  AS TotalCharge,NetTicketAmount AS TotalTicAmt, 4 AS DeliveryType,
	'' AS Courier_Charge, '' AS VP_Name, '' AS Venue_Add, T.CurrencyId, TD.PricePerTicket AS PricePerTic
	FROM Transactions T WITH(NOLOCK) 
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD WITH(NOLOCK) On T.Id = STD.TransactionId AND STD.SponsorId = @SponsorId
	INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON TD.EventTicketAttributeId=EA.Id
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON T.CurrencyId=CM.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Sponsors S WITH(NOLOCK) On STD.SponsorId = S.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END