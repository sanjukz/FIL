CREATE PROCEDURE [dbo].[KITMS_GetHandOverSheetTransDetails]  --201178839                       
(                                                
	@TransId BIGINT            
)                                                
AS                                                
BEGIN
	SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, V.Name AS VenueAddress, C.Name AS CityName,CONVERT(VARCHAR(15),ED.StartDateTime,113) AS EventStartDate,
	CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType,
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime, T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, 
	TC.Name AS Category, CM.Code AS CurrencyName, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, T.CurrencyId, TD.PricePerTicket AS PricePerTic,
	T.TotalTickets AS Quantity
	FROM Transactions T WITH(NOLOCK) 
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id=TD.TransactionId  AND T.Id = @TransId --AND T.TransactionStatusId = 8
	INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId
	INNER JOIN  CorporateTransactionDetails STD WITH(NOLOCK) On T.Id = STD.TransactionId
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