--spGetTransactionReport '9AB6585C-1A72-4EDA-8DB3-B0FD3C97C400'    
CREATE PROC spGetTransactionReport     
(    
  @EventAltId UniqueIdentifier = '2C411AC1-AA33-43E1-BA95-221684EB04F4' ,               
  @UserAltId UniqueIdentifier = 'D0CE577B-5394-432B-9571-05303B190996',               
  @EventDetailId Bigint = 0,               
  @FromDate Datetime = NULL,               
  @ToDate Datetime = NULL,      
  @PageNumber INT = 1,        
  @NoRecordsPerPage INT  = 100      
)    
AS    
BEGIN    
 SELECT top 100000  
 T.Id AS EventTransId,    
 ISNULL(T.EmailId, '') AS EmailId,     
 ISNULL(IP.IPAddress, '') AS IPAddress,    
 CASE WHEN ISNULL(T.PhoneNumber ,'') = '' THEN T.PhoneNumber ELSE T.PhoneCode + '-' + T.PhoneNumber END AS PhoneNumber,    
 T.FirstName + ' ' + T.LastName AS CustomerName,     
 '' AS ModeOfPayment,    
 '' AS CardType,    
 '' AS SaleStatus,    
 CONVERT(VARCHAR,CONVERT(DATE,T.CreatedUtc)) AS CreatedDate,     
 CONVERT(VARCHAR,CONVERT(TIME,T.CreatedUtc)) AS CreatedTime,     
 E.Name AS EventName,     
 ED.Name AS SubEventName,     
 C.Name AS EventCity,    
 CH.Channels,    
 CT.Code AS CurrencyName,    
 '' AS OutletName,    
 IP.CountryName AS CustomerCountry,    
 '' AS CustomerState,    
 IP.City AS CustomerCity,    
 '' AS CardIssuingCountry,    
 '' AS SuspectTransaction,    
 CONVERT(VARCHAR,CONVERT(DATE,ED.StartDateTime)) AS EventDate,    
 V.Name + ', '+ C.Name AS VenueAddress,    
 T.DiscountCode AS Promocode,    
 TC.Name AS TicketCategoty,     
 '' AS SeatNumber,    
 TT.TicketType AS TicketType,    
 ISNULL(TD.TotalTickets, 0) AS NoOfTicket,     
 ISNULL(TD.PricePerTicket, 0) AS PricePerTicket,     
 (TD.TotalTickets * TD.PricePerTicket) AS GrossTicketAmount,     
 ISNULL(TD.DiscountAmount, 0) AS DiscountAmount,    
 ((TD.TotalTickets * TD.PricePerTicket) - (ISNULL(TD.DiscountAmount, 0))) AS NetTicketAmount,    
 ISNULL(T.ConvenienceCharges, 0) AS ConvenienceCharges,     
 ISNULL(T.ServiceCharge, 0) AS ServiceTax,     
 ISNULL(T.NetTicketAmount, 0) AS TotalTransactedAmount,    
 0 AS CourierCharge,    
 '' AS DeliveryType,    
 '' AS TransactionType,    
 '' AS PaymentGateway,    
 '' AS PayConfNumber,    
 0 AS EntryCount,    
 '' AS NameOnCard    
 FROM Events E WITH (NOLOCK)    
 INNER JOIN EventDetails ED WITH (NOLOCK) ON E.Id = ED.EventId    
 INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId    
 INNER JOIN Cities C WITH (NOLOCK) ON C.Id = V.CityId    
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.EventDetailId = ED.Id    
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.EventTicketDetailId = ETD.Id    
 INNER JOIN TicketCategories TC WITH (NOLOCK) ON TC.Id = ETD.TicketCategoryId    
 INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.EventTicketAttributeId = ETA.Id    
 INNER JOIN TicketTypes TT WITH (NOLOCK) ON TT.Id = TD.TicketTypeId    
 INNER JOIN Transactions T WITH (NOLOCK) ON T.ID = TD.TransactionId    
 INNER JOIN CurrencyTypes CT WITH (NOLOCK) ON CT.ID = T.CurrencyId    
 INNER JOIN Channels CH WITH (NOLOCK) ON CH.Id = T.ChannelId    
 LEFT OUTER JOIN IPDetails IP WITH (NOLOCK) ON IP.Id = T.IPDetailId    
 WHERE E.AltId=@EventAltId  ANd T.TransactionStatusId=8 --AND Convert(DATE, DATEADD(MINUTE, 600, T.CreatedUtc)) <= '2018-09-30'    
 ORDER By T.Id DESC    
END