CREATE PROC [dbo].[KITMS_GetBarcodesByTransId]    
(    
  @TransId BIGINT    
)    
AS    
BEGIN    
	SELECT MSTD.Id AS TicNumId,T.Id AS TransId, BarcodeNumber AS barcode, 
	ED.name AS EventName, TC.Name AS VenueCatName, CONVERT(VARCHAR(20), StartDateTime, 107)  AS EventStartDate, 
	CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime,
    VD.Name AS VenueAddress, C.NAme As City_Name, MLSS.SeatTag, CONVERT(VARCHAR(20), EntryDateTime, 100) AS EntryDate
	FROM Transactions T WITH(NOLOCK) 
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId
	INNER JOIN EventTicketAttributes ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
	INNER JOIN EventAttributes EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id
	INNER JOIN Cities C WITH(NOLOCK) ON VD.CityId = C.Id
	INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id
	INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId
	INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id
	INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id
	INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id
	WHERE T.Id = @TransId AND T.TransactionStatusId =8 --AND PrintStatusId <>2
	ORDER BY ED.StartDateTime, ETA.Id
	  
END
