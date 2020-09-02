CREATE PROC [dbo].[spGetLoginBasedDynamicLayout]--  2168,182395, 56, 4565    
(          
--DECLARE    
   @EventId BIGINT ,    
   @EventDetailId BIGINT,    
   @VenueId INT,    
   @UserId BIGINT    
)          
AS          
BEGIN          
    
DECLARE @tblVenue TABLE (VenueId BIGINT)               
IF(@EventDetailId > 0)    
BEGIN         
 INSERT INTO @tblVenue(VenueId)             
 SELECT DISTINCT VenueId FROM EventDetails WITH (NOLOCK) WHERE  Id = @EventDetailId     
END    
ELSE    
BEGIN    
 INSERT INTO @tblVenue(VenueId)             
 SELECT @VenueId    
 SELECT TOP 1 @EventDetailId =  Id FROM EventDetails WITH (NOLOCK) WHERE VenueId = @VenueId AND EventId = @EventId AND StartDateTime >= GETUTCDATE()    
END    
    
DECLARE @UserEventTicketAttributeMappingExists INT = 0    
SELECT @UserEventTicketAttributeMappingExists = ISNULL(COUNT(*),0) FROM UserEventTicketAttributeMappings WITH (NOLOCK) WHERE     
EventTicketAttributeId IN(SELECT ETA.Id FROM EventDetails AS ED WITH (NOLOCK) INNER JOIN         
EventTicketDetails AS ETD WITH (NOLOCK) ON ED.Id = ETD.EventDetailId AND ED.Id = @EventDetailId INNER JOIN    
EventTicketAttributes AS ETA WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId) AND UserId = @UserId    
          
SELECT DISTINCT TC.Id AS VenueCatId, TC.Name AS VenueCatName, ED.Id AS EventId,           
ISNULL(ETA.RemainingTicketForSale,0) AS TotalTic,           
ISNULL(ETA.AvailableTicketForSale, 0) TicketForSale,  ISNULL(ETA.Price, 0) AS PricePerTic,           
0 AS TotalTicBlocks,     
ED.Name AS EventName, ETA.Id AS VMCC_Id,    
ISNULL((SELECT SUM(RemainingTickets) FROM CorporateTicketAllocationDetails A WITH (NOLOCK) WHERE EventTicketAttributeId =ETA.Id AND IsEnabled = 1), 0)    
 AS BlockedTics,    
 CASE WHEN @UserEventTicketAttributeMappingExists > 0 THEN    
 CASE WHEN ISNULL((SELECT EventTicketAttributeId FROm UserEventTicketAttributeMappings WITH (NOLOCK) WHERE EventTicketAttributeId = ETA.Id     
 AND UserId = @UserId),0) =0 THEN 0 ELSE 1 END     
 ELSE CASE WHEN (ETA.Id in (549318,549319,549320,549321,549322,549323,549324,549325,549326,549327,549328,549329,549330,549331)) THEN 1   
 ELSE CASE WHEN ISNULL(ETA.IsEnabled,0) = 0 THEN 0 ELSE 1 END END END  
 AS Status    
--CASE WHEN ISNULL(ETA.IsEnabled,0) = 0 THEN 0 ELSE 1 END AS Status          
, DSC.SectionCoordinates AS Coordinates, DSC.SectionTextCoordinates AS TextCoordinates,     
DSC.DisplayName AS RowDisplayName, ISNULL(DSC.IsDisplay,0) AS ToShow,          
DSC.CircleRectangleValue AS CircleRectVal,      
DSC.Id AS ID,DSC.styles,     
CT.Name As CurrencyName, 0 AS IsFamilyPkgAvailable, 0 AS FamilyPkgPrice,          
CASE WHEN ISNULL(SeasonPackage,0)=0 THEN 'false' ELSE 'true' END AS IsSeason,         
SeasonPackagePrice AS SeasonPrice, 0 AS SeasonDiscount, V.Name As VenueAddress, V.Id AS VenueId, DSC.DisplayName,    
'' As StartAngle, '' As EndAngle    
FROM EventDetails AS ED WITH (NOLOCK) INNER JOIN         
EventTicketDetails AS ETD WITH (NOLOCK) ON ED.Id = ETD.EventDetailId INNER JOIN    
EventTicketAttributes AS ETA WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId INNER JOIN    
TicketCategories AS TC WITH (NOLOCK) ON ETD.TicketCategoryId = TC.Id            
INNER JOIN Venues V WITH (NOLOCK) ON ED.VenueId = V.Id              
INNER JOIN CurrencyTypes CT WITH (NOLOCK) ON ETA.CurrencyId = CT.Id    
RIGHT OUTER JOIN DynamicStadiumTicketCategoriesDetails AS DTCD WITH (NOLOCK) ON DTCD.TicketCategoryId = TC.Id    
RIGHT OUTER JOIN DynamicStadiumCoordinates AS DSC WITH (NOLOCK) ON DTCD.DynamicStadiumCoordinateId = DSC.ID     
AND ED.VenueId = DSC.VenueId AND DSC.IsEnabled = 1          
WHERE (DSC.SectionCoordinates IS NOT NULL)            
 AND ED.Id = @EventDetailId     
 OR (DTCD.TicketCategoryId IS NULL AND DSC.VenueId IN (SELECT VenueId FROM @tblVenue))          
ORDER BY DSC.ID DESC     
END 