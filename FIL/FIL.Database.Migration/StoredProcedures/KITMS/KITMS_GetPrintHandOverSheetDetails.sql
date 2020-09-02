CREATE PROC [dbo].[KITMS_GetPrintHandOverSheetDetails] -- 20032
(
	@TransId BIGINT --= 201145872
)                
AS                
BEGIN

SELECT DISTINCT A.TransactionId AS TransId, B.TotalTickets AS QuantityPrinted, SerialStart AS SerialFrom,
C.PhoneCode+'-'+ C.PhoneNumber AS Usermobile,
SerialEnd AS SerialTo, TicketHandedBy AS HandedBy, TicketHandedTo AS SubmittedTo
FROM HandoverSheets A WITH(NOLOCK) 
INNER JOIN Transactions B WITH(NOLOCK) On A.TransactionId =B.Id AND A.TransactionId = @TransId
INNER JOIN Users C WITH(NOLOCK) On A.CreatedBy = C.AltId

SELECT DISTINCT E.Name AS EventCatName, ED.Name AS EventName, V.Name AS VenueAddress, C.Name AS CityName,CONVERT(VARCHAR(15),ED.StartDateTime,113) AS EventStartDate,
CASE WHEN TransactingOptionId =4 THEN 'Paid' ELSE 'Complimentary' END AS PGType,
CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime, T.CreatedUtc AS BookingDate, TD.TotalTickets AS NoOfTic, 
TC.Name AS VenueCatName, CM.Code AS LocalCurrency, T.DiscountAmount AS DiscountAmt, NetTicketAmount AS TotalTicAmt, T.CurrencyId, 
TD.PricePerTicket AS LocalPricePerTic, TD.TotalTickets AS Qty, CONVERT(VARCHAR, GETDATE(), 107) AS DATE
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

DECLARE @SponsorId BIGINT
SELECT @SponsorId = SponsorId FROM CorporateTransactionDetails WITH(NOLOCK) WHERE TransactionId = @TransId

SELECT DISTINCT SM.SponsorName AS CompanyName,
CASE WHEN ISNULL(CMO.SponsorId,0) > 0 THEN CASE WHEN CMO.PickupRepresentativeFirstName <> '' 
THEN CMO.PickupRepresentativeFirstName +' '+ PickupRepresentativeLastName ELSE CMO.FirstName +' '+CMO.LastName END ELSE SM.FirstName+' '+SM.LastName END 
AS SponsorName,SM.Email AS EmailId,
CONVERT(VARCHAR, GETDATE(), 107) AS DATE, SM.PhoneCode+'-'+SM.PhoneNumber AS MobNo, 0 AS AltMobNo               
FROM Sponsors SM WITH(NOLOCK)  
LEFT OUTER JOIN CorporateRequests CMO WITH(NOLOCK) ON SM.Id = CMO.SponsorId 
WHERE SM.Id = @SponsorId

END