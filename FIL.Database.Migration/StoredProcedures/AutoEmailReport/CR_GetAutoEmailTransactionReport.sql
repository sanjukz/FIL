--CR_GetAutoEmailTransactionReport 2754 --243       
CREATE PROC CR_GetAutoEmailTransactionReport                                      
(                                       
--DECLARE                                           
 @EventId BIGINT                                                                                                       
)                                            
AS                                            
BEGIN                                            
 CREATE TABLE #TblTransactionTickets (TransactionId BIGINT, TotalTickets INT)                                            
 CREATE TABLE #TblTransactionPaymentConfirmation (TransactionId BIGINT, PayConfNumber NVARCHAR(200))                        
 CREATE TABLE #TblTransactionSponsorName (TransactionId BIGINT, SponsorName NVARCHAR(200))                                            
 CREATE TABLE #TblTransactionPaymentDetails (TransactionId BIGINT, CardNumber NVARCHAR(200), NameOnCard NVARCHAR(200), PaymentGateway NVARCHAR(200), CardType NVARCHAR(200), PaymentOptions NVARCHAR(200))                                            
                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                            
 INSERT INTO #TblTransactionTickets                                                    
 SELECT T.Id, SUM(TD.TotalTickets)                                            
 FROM Transactions T WITH (NOLOCK)                                            
 INNER JOIN TransactionDetails TD ON T.Id = TD.TransactionId                                                    
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id = TD.EventTicketAttributeId                                              
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId                                            
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ED.Id = ETD.EventDetailId                                            
 INNER JOIN Events E WITH (NOLOCK) ON E.Id = ED.EventId                                             
 LEFT OUTER JOIN EventAttributes EA WITH (NOLOCK) ON EA.EventDetailId = ED.Id                                                        
 WHERE                                                 
 T.TransactionStatusId IN (8) AND TD.TotalTickets > 0                                            
 AND E.Id = @EventId                                           
 GROUP BY T.Id                                            
                                            
 CREATE INDEX IDX_TblTransactionTickets_TransactionId ON #TblTransactionTickets(TransactionId)                                              
 --SELECT * FROM #TblTransactionTickets                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                            
                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                            
 INSERT INTO #TblTransactionPaymentConfirmation                                             
 SELECT TPD.TransactionId, TPD.PayConfNumber                                             
 FROM TransactionPaymentDetails TPD WITH (NOLOCK)                                            
 INNER JOIN #TblTransactionTickets TTT WITH (NOLOCK) ON TTT.TransactionId = TPD.TransactionId                                            
 WHERE TPD.TransactionId = TTT.TransactionId AND PayConfNumber IS NOT NULL AND PayConfNumber <> ''                                            
 ORDER BY Id                                     
                                            
CREATE INDEX IDX_TblTransactionPaymentConfirmation_TransactionId ON #TblTransactionPaymentConfirmation(TransactionId)                                             
 --SELECT * FROM #TblTransactionPaymentConfirmation                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                        
                       
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                            
 INSERT INTO #TblTransactionSponsorName                                             
 SELECT DISTINCT MSTD.TransactionId, S.SponsorName FROM MatchSeatTicketDetails MSTD WITH (NOLOCK)                      
 INNER JOIN Sponsors S WITH (NOLOCK) ON S.Id = MSTD.SponsorId                                      
 INNER JOIN #TblTransactionTickets TTT WITH (NOLOCK) ON TTT.TransactionId = MSTD.TransactionId                                            
 WHERE MSTD.TransactionId = TTT.TransactionId                                             
 ORDER BY MSTD.TransactionId                                     
                                            
 CREATE INDEX IDX_TransactionSponsorName_TransactionId ON #TblTransactionSponsorName(TransactionId)                                             
 --SELECT * FROM #TblTransactionPaymentConfirmation                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                          
                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                         
 INSERT INTO #TblTransactionPaymentDetails                                            
 SELECT TPD.TransactionId, 'XXXXXXXXXXXX' + SUBSTRING(UCD.CardNumber, LEN(UCD.CardNumber) - 3, LEN(UCD.CardNumber)), UCD.NameOnCard, PG.PaymentGateway, CT.CardType, PO.PaymentOptions                                            
 FROM TransactionPaymentDetails TPD WITH (NOLOCK)                                            
 INNER JOIN UserCardDetails UCD WITH (NOLOCK) ON UCD.Id = TPD.UserCardDetailId                                            
 INNER JOIN PaymentGateways PG WITH (NOLOCK) ON PG.Id = TPD.PaymentGatewayId                                            
 INNER JOIN PaymentOptions PO WITH (NOLOCK) ON PO.Id = TPD.PaymentOptionId                                            
 LEFT OUTER JOIN CardTypes CT WITH (NOLOCK) ON CT.Id = UCD.CardTypeId                                            
 INNER JOIN #TblTransactionTickets TTT WITH (NOLOCK) ON TTT.TransactionId = TPD.TransactionId                                            
 WHERE TPD.TransactionId = TTT.TransactionId AND UserCardDetailId IS NOT NULL                                            
 ORDER BY TPD.Id                                     
                                            
 CREATE INDEX IDX_TblTransactionPaymentDetails_TransactionId ON #TblTransactionPaymentDetails(TransactionId)                                            
 --SELECT * FROM #TblTransactionPaymentDetails                                            
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                                            
                                            
                                            
 CREATE TABLE #TransactionReport (                                             
 Sno Bigint IDENTITY(1,1),                                            
 TransactionId BIGINT,                                                
 TransactionDate NVARCHAR(100),                                            
 TransactionTime NVARCHAR(500),                                            
 CustomerName NVARCHAR(500),                                            
 CustomerEmail NVARCHAR(500),                                 
 CustomerPhone NVARCHAR(500),                                            
 OutletBOName NVARCHAR(500),                                            
 Channel NVARCHAR(500),                                            
 Event NVARCHAR(500),                                            
 EventName NVARCHAR(500),                                            
 EventStartDateTime DATETIME,                                            
 VenueAddress NVARCHAR(500),                                            
 VenueCity NVARCHAR(500),                                            
 VenueCountry NVARCHAR(500),                                            
 TicketCategory NVARCHAR(500),                                            
 SeatNumber NVARCHAR(500),                               
 TicketType NVARCHAR(500),                                            
 Currency NVARCHAR(500),                                            
 PricePerTicket DECIMAL(18,2),                                            
 NumberOfTickets INT,                                            
 DeliveryType NVARCHAR(500),                                      
 GrossTicketAmount DECIMAL(18,2),                                            
 DiscountAmount DECIMAL(18,2),                                            
 PromoCode NVARCHAR(500),                                            
 ServiceCharge DECIMAL(18,2),                                            
 DeliveryCharges DECIMAL(18,2),                                             
 ConvenienceCharges DECIMAL(18,2),                                            
 ExchangeRate DECIMAL(18,2),                                            
 CustomerIP NVARCHAR(500),                                            
 IPBasedCountry NVARCHAR(500),                                            
 IPBasedState NVARCHAR(500),                                            
 IPBasedCity NVARCHAR(500),                                       
TransactionStatusId INT,                                            
 SaleStatus NVARCHAR(500),                                            
 PayConfNumber NVARCHAR(500)                                       
 )                                                
                                  
 INSERT INTO #TransactionReport                                     
SELECT DISTINCT T.Id AS TransactionId,                                            
 CONVERT(VARCHAR, DATEADD(MINUTE, CONVERT(INT, ISNULL(EA.TimeZone, '330')), T.CreatedUtc), 106) AS TransactionDate,                                            
 CONVERT(VARCHAR, DATEADD(MINUTE, CONVERT(INT, ISNULL(EA.TimeZone, '330')), T.CreatedUtc), 114) AS TransactionTime,                                            
 CASE T.ChannelId WHEN 8 THEN TTSN.SponsorName ELSE ISNULL(T.FirstName + ' ' + T.LastName, '-') END AS CustomerName,                                            
 ISNULL(T.EmailId, '-') AS CustomerEmail,                                            
 ISNULL(T.PhoneCode + '-' + T.PhoneNumber, '-') AS CustomerPhone,                                            
 CASE CH.Id WHEN 4 THEN U.Email ELSE '-' END AS OutletBOName,                                            
 CH.Channels AS Channel,                                            
 E.Name AS Event,                                            
 ED.Name AS EventName,                                            
 ED.StartDateTime AS EventStartDateTime,                                            
 V.Name AS VenueAddress,                                       
 C.Name AS VenueCity,                                            
 COU.Name AS VenueCountry,                                            
 TC.Name AS TicketCategory,     
 '-' AS SeatNumber,                                            
 TT.TicketType,                                            
 CT.Code AS Currency,                                            
 CASE (SELECT TOP 1 PayConfNumber FROM #TblTransactionPaymentConfirmation WHERE TransactionId = T.Id) WHEN 'Complimentary' THEN 0 ELSE CASE TD.TotalTickets                                     
 WHEN 0 THEN 0 ELSE ISNULL(TD.PricePerTicket, 0) END END AS PricePerTicket,                                               
 ISNULL(TD.TotalTickets, 0) AS NumberOfTickets,                                            
 CASE TDD.DeliveryTypeId WHEN 1 THEN 'Courier' WHEN 2 THEN 'Venue Pickup' WHEN 3 THEN 'Print-At-Home' WHEN 4 THEN 'M-Ticket' ELSE ISNULL(DT.DeliveryTypes, 'On-Stock') END AS DeliveryType,                              
 --CASE TTT.PayConfNumber WHEN 'Complimentary' THEN 'Complimentary' ELSE 'Paid' END AS TransactionType,                                            
 ----TTT.PayConfNumber,                                            
 (CASE (SELECT TOP 1 PayConfNumber FROM #TblTransactionPaymentConfirmation WHERE TransactionId = T.Id) WHEN 'Complimentary' THEN 0 ELSE                                     
 CASE TD.TotalTickets WHEN 0 THEN 0 ELSE ISNULL(TD.PricePerTicket, 0) END END) * ISNULL(TD.TotalTickets, 0) AS GrossTicketAmount,                                            
 --(ISNULL(T.DiscountAmount, 0) / TTT.TotalTickets) * TD.TotalTickets AS DiscountAmount,                                            
 ISNULL(TD.DiscountAmount, 0) AS DiscountAmount,                                    
 ISNULL(T.PromoCode,ISNULL(T.DiscountCode, '-')) AS PromoCode,                                            
 (ISNULL(T.ServiceCharge, 0) / TTT.TotalTickets) * TD.TotalTickets AS ServiceCharge,                                            
 (ISNULL(T.DeliveryCharges, 0) / TTT.TotalTickets) * TD.TotalTickets AS DeliveryCharges,                                            
 (ISNULL(T.ConvenienceCharges, 0) / TTT.TotalTickets) * TD.TotalTickets AS ConvenienceCharges,                                            
 ISNULL(CT.ExchangeRate, 1),                                            
 ISNULL(IP.IPAddress, '-') AS CustomerIP,                                            
 ISNULL(IP.CountryName, '-') AS IPBasedCountry,                                            
 ISNULL(IP.RegionName, '-') AS IPBasedState,                                            
 ISNULL(IP.City, '-') AS IPBasedCity,                                            
 T.TransactionStatusId,                                            
 CASE T.TransactionStatusId WHEN 16 THEN 'Reverted' ELSE '-' END AS SaleStatus,                                           
 (SELECT TOP 1 PayConfNumber FROM #TblTransactionPaymentConfirmation WHERE TransactionId = T.Id) AS PayConfNumber                                            
 FROM                                             
 Transactions T WITH (NOLOCK)                                            
 INNER JOIN #TblTransactionTickets TTT WITH (NOLOCK) ON T.Id = TTT.TransactionId                            
 LEFT OUTER JOIN #TblTransactionSponsorName TTSN WITH (NOLOCK) ON T.Id = TTSN.TransactionId                                  
 INNER JOIN TransactionDetails TD WITH (NOLOCK) ON T.Id = TD.TransactionId   INNER JOIN Users U WITH (NOLOCK) ON U.AltId = T.CreatedBy                                            
 INNER JOIN CurrencyTypes CT WITH (NOLOCK) ON CT.ID = T.CurrencyId                                            
 INNER JOIN Channels CH WITH (NOLOCK) ON CH.Id = T.ChannelId                                            
 INNER JOIN TicketTypes TT WITH (NOLOCK) ON TT.Id = TD.TicketTypeId                                             
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id = TD.EventTicketAttributeId                                              
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId                                            
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ED.Id = ETD.EventDetailId                                            
 INNER JOIN Events E WITH (NOLOCK) ON E.Id = ED.EventId                                             
 INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId                                                
 INNER JOIN Cities C WITH (NOLOCK) ON C.Id = V.CityId                                
 INNER JOIN States S WITH (NOLOCK) ON S.Id = C.StateId                                            
 INNER JOIN Countries COU WITH (NOLOCK) ON COU.Id = S.CountryId                                            
 INNER JOIN TicketCategories TC WITH (NOLOCK) ON TC.Id = ETD.TicketCategoryId                                           
 LEFT OUTER JOIN IPDetails IP WITH (NOLOCK) ON IP.Id = T.IPDetailId                                              
 LEFT OUTER JOIN EventAttributes EA WITH (NOLOCK) ON EA.EventDetailId = ED.Id                                            
 LEFT OUTER JOIN TransactionDeliveryDetails TDD WITH (NOLOCK) ON TD.Id = TDD.TransactionDetailId                                            
 LEFT OUTER JOIN DeliveryTypes DT WITH (NOLOCK) ON DT.Id = TDD.DeliveryTypeId                                            
 WHERE                                             
 E.Id = @EventId        
                                     
 CREATE INDEX IDX_TransactionReport_TransactionId ON #TransactionReport(TransactionId)                                            
                                            
 SELECT                                             
 Sno,                                             
 TransactionId,                                             
 TransactionDate,                                             
 TransactionTime,                                             
 ISNULL(CustomerName, '-') AS CustomerName,                                             
 ISNULL(CustomerEmail, '-') AS CustomerEmail,                                             
 ISNULL(CustomerPhone, '-') AS CustomerPhone,                                             
 OutletBOName,                                             
 Channel,                                             
 Event,                                             
 EventName,                                             
 EventStartDateTime,                                             
 VenueAddress,                                             
 VenueCity,                                             
 VenueCountry,                                  
 TicketCategory,                                             
 SeatNumber,                                             
 TicketType,                                             
 DeliveryType,                                             
 Currency,                                             
 ISNULL(CONVERT(DECIMAL(18,2), PricePerTicket), 0) AS PricePerTicket,                                             
 ISNULL(CONVERT(DECIMAL(18,2), NumberOfTickets), 0) AS NumberOfTickets,                                             
 ISNULL(CONVERT(DECIMAL(18,2), GrossTicketAmount), 0) AS GrossTicketAmount,                                             
 ISNULL(CONVERT(DECIMAL(18,2), DiscountAmount), 0) AS DiscountAmount,                                             
 CASE ISNULL(PromoCode, '') WHEN '' THEN '-' ELSE ISNULL(PromoCode, '-') END AS PromoCode,                                             
 ISNULL(CONVERT(DECIMAL(18,2), GrossTicketAmount - DiscountAmount), 0) AS NetTicketAmount,                        
 ISNULL(CONVERT(DECIMAL(18,2), (GrossTicketAmount - DiscountAmount) / ExchangeRate), 0) AS NetTicketAmountUSD,                                        
 ISNULL(CONVERT(DECIMAL(18,2), ServiceCharge), 0) AS ServiceCharge,                                             
 ISNULL(CONVERT(DECIMAL(18,2), DeliveryCharges), 0) AS DeliveryCharges,                                             
 ISNULL(CONVERT(DECIMAL(18,2), ConvenienceCharges), 0) AS ConvenienceCharges,                                             
 ISNULL(CONVERT(DECIMAL(18,2), ExchangeRate), 0) AS ExchangeRate,                                             
 ISNULL(CONVERT(DECIMAL(18,2), ((GrossTicketAmount - DiscountAmount) + ServiceCharge + DeliveryCharges + ConvenienceCharges)), 0) AS TotalTransactedAmount,                                            
 ISNULL(CONVERT(DECIMAL(18,2), (((GrossTicketAmount - DiscountAmount) + ServiceCharge + DeliveryCharges + ConvenienceCharges) / ExchangeRate)), 0) AS TotalTransactedAmountUSD,                                            
 CustomerIP,                                             
 IPBasedCountry,                                             
 IPBasedState,                                             
 IPBasedCity,                                             
 SaleStatus,                                             
 PayConfNumber,               
 '-' AS IsInternationalCard,              
 '-' AS CardIssuingCountry,              
 '-' AS SuspectTransaction,              
 '-' AS EventState,              
 '-' AS EntryCount,                                           
 CASE PayConfNumber WHEN 'Complimentary' THEN 'Complimentary' ELSE 'Paid' END AS TransactionType,                                            
 ISNULL((SELECT TOP 1 CardNumber FROM #TblTransactionPaymentDetails WHERE TransactionId = T.TransactionId), '-') AS CardNumber,                                            
 ISNULL((SELECT TOP 1 NameOnCard FROM #TblTransactionPaymentDetails WHERE TransactionId = T.TransactionId), '-') AS NameOnCard,                                            
 ISNULL((SELECT TOP 1 PaymentGateway FROM #TblTransactionPaymentDetails WHERE TransactionId = T.TransactionId), 'Cash') AS PaymentGateway,                                            
 ISNULL((SELECT TOP 1 CardType FROM #TblTransactionPaymentDetails WHERE TransactionId = T.TransactionId), '-') AS CardType,                                            
 ISNULL((SELECT TOP 1 PaymentOptions FROM #TblTransactionPaymentDetails WHERE TransactionId = T.TransactionId), 'Cash') AS ModeOfPayment                                             
 FROM #TransactionReport T                                       
 ORDER BY T.EventStartDateTime            
      
 DROP TABLE #TblTransactionTickets                                            
 DROP TABLE #TblTransactionPaymentConfirmation                       
 DROP TABLE #TblTransactionSponsorName                                           
 DROP TABLE #TblTransactionPaymentDetails                                           
 DROP TABLE #TransactionReport                                            
END 