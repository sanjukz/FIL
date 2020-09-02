CREATE PROC [dbo].[Web_GetDynamicLayout] --182272
(      
--DECLARE
   @EventId BIGINT = 182272
)      
AS      
BEGIN      

DECLARE @tblVenue TABLE (VenueId BIGINT)           
      
INSERT INTO @tblVenue(VenueId)         
SELECT DISTINCT VenueId FROM EventDetails WITH (NOLOCK) WHERE  Id = @EventId 
      
SELECT DISTINCT TC.Id AS VenueCatId, TC.Name AS VenueCatName, ED.Id AS EventId,       
ISNULL(ETA.RemainingTicketForSale,0) TotalTic,       
ISNULL(ETA.AvailableTicketForSale, 0) TicketForSale,  ISNULL(ETA.Price, 0) AS PricePerTic,       
0 AS TotalTicBlocks, ED.Name AS EventName, ETA.Id AS VMCC_Id,
0 AS BlockedTics,
1 AS Status      
 , DSC.SectionCoordinates AS Coordinates, DSC.SectionTextCoordinates AS TextCoordinates, 
 DSC.DisplayName AS RowDisplayName, ISNULL(DSC.IsDisplay,0) AS ToShow,      
  DSC.CircleRectangleValue AS CircleRectVal,  
  DSC.Id AS ID,DSC.styles, 
  CT.Name As CurrencyName, 0 AS IsFamilyPkgAvailable, 0 AS FamilyPkgPrice,      
  0 IsSeason,     
  0 AS SeasonPrice, 0 AS SeasonDiscount, V.Name As VenueAddress, V.Id AS VenueId, DSC.DisplayName,
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
 AND ED.Id = @EventId 
 OR (DTCD.TicketCategoryId IS NULL AND DSC.VenueId IN (SELECT VenueId FROM @tblVenue))      
ORDER BY DSC.ID DESC 
END 

