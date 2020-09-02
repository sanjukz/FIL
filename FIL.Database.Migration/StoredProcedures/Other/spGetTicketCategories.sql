CREATE PROCEDURE [dbo].[spGetTicketCategories]  --2019 
( 
	@EventCatId INT,
	 @EventId  BIGINT,
	 @VenueId INT,
	 @IsSeason INT,
	 @UserId INT  
)  
AS  
BEGIN
	IF(@IsSeason=1)
	BEGIN
		SELECT TOP 1  @EventId = Id FROM EventDetails WITH(NOLOCK) WHERE EventId =@EventCatId AND VenueId = @VenueId 
		AND ISEnabled=1
	END

  SELECT ETA.Id AS VMCC_Id,   
  TC.Id AS VenueCatId,   
  TC.Name AS VenueCatName,   
  ETA.Price AS PricePerTic,   
  ETA.RemainingTicketForSale AS RemainingTicket, 
  ISNULL(  
      ( SELECT SUM(KM.RemainingTickets)                                                          
        FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK)  
		INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                              
        WHERE ETA.Id=KM.EventTicketAttributeId AND  KM.IsEnabled = 1 AND SM.SponsorTypeId=1  
      ),0) AS Hold, 
  ED.NAME AS EventName,   
  CM.Id AS Currencyid,   
  CM.Code AS CurrencyName,  
  VD.Name +', '+CTM.Name  AS VenueAddress,  
  VD.ID AS VenueId,  
  ISNULL(ETA.IsSeatSelection,0) AS  IsLayoutAvail,
  CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStartTime, ISNULL(SeasonPackagePrice,0) AS SeasonPrice, 
  --CASE WHEN ISNULL(SeasonPackage,0) = 0 THEN 0 ELSE 1 END AS IsSeasonAvailable
   ISNULL(SeasonPackage,'False') AS IsSeasonAvailable
  FROM EventTicketAttributes ETA WITH(NOLOCK)  
  INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id AND ETD.EventDetailId = @EventId 
  --AND ETA.RemainingTicketForSale > 0
  INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
  INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
  INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON ETA.CurrencyId = CM.Id  
  INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id  
  INNER JOIN Cities CTM WITH(NOLOCK) ON VD.CityId = CTM.Id
  ORDER BY TC.Name ASC, ETA.Price DESC, StartDateTime ASC 
END
