CREATE PROCEDURE [dbo].[spGetLoginBasedTicketCategories]  --2164, 182272, 57, 0, 4565  
(   
  @EventCatId INT,  
  @EventId  BIGINT,  
  @VenueId INT,  
  @IsSeason INT,  
  @UserId INT,  
  @DynamicStadiumCoordinateId INT  
)    
AS    
BEGIN  
 IF(@IsSeason=1)  
 BEGIN  
  SELECT TOP 1  @EventId = Id FROM EventDetails WITH(NOLOCK) WHERE EventId =@EventCatId AND VenueId = @VenueId   
  AND ISEnabled=1  
 END  
  
 DECLARE @tblTicketCategories TABLE(Id INT)  
 IF(@DynamicStadiumCoordinateId<>0)  
 BEGIN  
  INSERT INTO @tblTicketCategories  
  SELECT TicketCategoryId FROM DynamicStadiumTicketCategoriesDetails WITH(NOLOCK) WHERE DynamicStadiumCoordinateId = @DynamicStadiumCoordinateId  
 END  
 ELSE  
 BEGIN  
  INSERT INTO @tblTicketCategories  
  SELECT TC.Id FROM EventTicketAttributes ETA WITH(NOLOCK)    
  INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id AND ETD.EventDetailId = @EventId  
  INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id  
 END  
  
 SELECT ETA.Id AS VMCC_Id,     
 TC.Id AS VenueCatId,     
 TC.Name AS VenueCatName,     
 ETA.Price AS PricePerTic,     
 ETA.RemainingTicketForSale AS RemainingTicket,   
 ISNULL(    
  (SELECT SUM(KM.RemainingTickets)                                                            
  FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK)    
  INNER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id = KM.SponsorId                                
  WHERE ETA.Id=KM.EventTicketAttributeId AND  KM.IsEnabled = 1 AND SM.SponsorTypeId=1    
  ),0) AS Hold,   
 ED.NAME AS EventName,     
 CM.Id AS Currencyid,     
 CM.Code AS CurrencyName,    
 VD.Name +', '+CTM.Name  AS VenueAddress,    
 VD.ID AS VenueId,    
 CASE WHEN ISNULL(ETA.IsSeatSelection,0) =0 THEN 0 ELSE 1 END AS  IsLayoutAvail,  
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
 WHERE TC.Id IN(SELECT Id FROM @tblTicketCategories)  
 ORDER BY TC.Name ASC, ETA.Price DESC, StartDateTime ASC   
END  
  