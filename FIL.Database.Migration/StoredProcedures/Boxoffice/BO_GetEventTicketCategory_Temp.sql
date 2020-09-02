CREATE PROC [dbo].[BO_GetEventTicketCategory_Temp] ---22423,5276                                                                                                               
(                                                                                      
	@EventId bigint = 119449,                                                                                                              
	@Retailerid bigint                                                                                                             
)                                                                                                                          
AS                                                                                                                          
BEGIN                                                                                                                          
DECLARE @EventStartDate DATE, @EventType BIGINT, @EventName VARCHAR(1000), @EventVenue VARCHAR(1000), 
@TicPerTrans BIGINT = 10                                                                                                              
                                                                                                              
SELECT @TicPerTrans = ISNULL(TicketLimit,0) FROM BoxofficeUserAdditionalDetails  WITH(NOLOCK) WHERE UserId=@Retailerid   
SET @TicPerTrans = 10                                                                                                         
                                                                                                                           
SELECT @EventStartDate = StartDateTime, @EventType = EventId, @EventName = Name, @EventVenue = VenueId 
FROM EventDetails WITH(NOLOCK) WHERE Id = @EventId                                  
                                                                                                                          
SELECT ETA.Id AS VMCC_Id,  --TC.Name as VenueCatName,
CASE  WHEN ISNULL(ETA.SRCitizenQty,0) <>0 THEN TC.Name +' (Senior Citizen Qty - ' + CONVERT(VARCHAR(50),ETA.SRCitizenQty) +')'                                                                          
ELSE TC.Name END  AS VenueCatName,                                                          
ETA.TicketCategoryDescription AS VenueCatDetail,ETA.Price AS PricePerTic,ETA.RemainingTicketForSale AS TotalTic, 
ED.Name AS EventName, ED.StartDateTime AS EventStartDate, ED.StartDateTime AS EventStartTime, 
CT.Code AS CurrencyName, V.Name AS VenueAddress, C.Name AS City_Name, 
CASE WHEN RemainingTicketForSale > @TicPerTrans THEN @TicPerTrans ELSE RemainingTicketForSale END AS TicPerTrans,                                                                                                 
1 AS EventStatus, CONVERT(INT, ISNULL(ETA.IsSeatSelection,0)) AS IsLayoutAvail,                                                                             
'false' AS IsFamilyPkgAvailable,0 AS FamilyPkgPrice, ED.ID AS EventID,                                                                            
0 AS DTCMCategoryId, E.Id as EventCatId, ISNULL(ETA.ChildQTY,0) As ChildQty, ISNULL(ETA.LocalPrice,0) AS LocalPricePerTic,                                                                                    
'false' AS IsSeason, 0 AS SeasonPrice,0 AS SeasonDiscount,ISNULL(ETA.SRCitizenQTY,0)  As SRCitizenQty,                                     
0 AS IsSenior,'False' AS IsSeasonTribe                                                    
FROM EventTicketAttributes ETA WITH(NOLOCK)         
Inner Join EventTicketDetails ETD  WITH(NOLOCK) ON ETD.ID=ETA.EventTicketDetailId                                                            
INNER JOIN TicketCategories TC WITH(NOLOCK) ON TC.Id =ETD.TicketCategoryId                                                                             
INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.Id = ETD.EventDetailId                                                                                                 
INNER JOIN Events E WITH(NOLOCK) ON E.Id = ED.EventId                                             
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.LocalCurrencyID                                                                  
INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId                                                                              
INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId                    
WHERE ETA.IsEnabled=1 AND ED.IsEnabled=1 and ED.Id=@EventId                                                                                                             
AND VenueId = @EventVenue  and ETA.RemainingTicketForSale > 0 
UNION    
SELECT ETA.Id AS VMCC_Id,--TC.Name as VenueCatName,           
CASE  WHEN ISNULL(ETA.SRCitizenQty,0) <>0 THEN TC.Name+' (Senior Citizen Qty - ' + CONVERT(VARCHAR(50),ETA.SRCitizenQty) +')'                                                                          
ELSE TC.Name END  AS VenueCatName, ETA.TicketCategoryDescription AS VenueCatDetail,ETA.Price AS PricePerTic,
ETA.RemainingTicketForSale AS TotalTic, ED.Name AS EventName, ED.StartDateTime AS EventStartDate, 
ED.StartDateTime AS EventStartTime, CT.Code AS CurrencyName, V.Name AS VenueAddress, C.Name AS City_Name, 
CASE WHEN RemainingTicketForSale > @TicPerTrans THEN @TicPerTrans ELSE RemainingTicketForSale END AS TicPerTrans,                                                                                                 
1 AS EventStatus, CONVERT(INT, ISNULL(ETA.IsSeatSelection,0)) AS IsLayoutAvail,                                                                             
'false' AS IsFamilyPkgAvailable,0 AS FamilyPkgPrice, ED.ID AS EventID,                                                                            
0 AS DTCMCategoryId, E.Id AS EventCatId, ISNULL(ETA.ChildQTY,0) AS ChildQty, --isnull(ETA.LocalPrice,0) As LocalPricePerTic,              
isnull(ETA.SeasonPackageLocalPrice,0) AS LocalPricePerTic,                                                                                
isnull(ETA.SeasonPackage,0) AS IsSeason,                                                                                
ISnull(ETA.SeasonPackageLocalPrice,0) AS SeasonPrice,0 AS SeasonDiscount ,                                                                      
 ISNULL(ETA.SRCitizenQTY,0)  As SRCitizenQty,                                     
 0 AS IsSenior,'False' AS IsSeasonTribe                                          
FROM EventTicketAttributes ETA WITH(NOLOCK)             
Inner Join EventTicketDetails ETD  WITH(NOLOCK) ON ETD.ID=ETA.EventTicketDetailId                                                                                          
INNER JOIN TicketCategories TC WITH(NOLOCK) ON TC.Id =ETD.TicketCategoryId                                                                             
INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.Id = ETD.EventDetailId                                                          
INNER JOIN Events E WITH(NOLOCK) ON E.Id = ED.EventId                                             
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.LocalCurrencyID                                                                  
INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId                                               
INNER JOIN Cities C WITH(NOLOCK) ON C.Id = V.CityId                    
WHERE ETA.IsEnabled=1 AND ED.IsEnabled=1 and ED.Id=@EventId                                                                                                             
AND VenueId = @EventVenue  AND ETA.RemainingTicketForSale > 0  AND ETA.SeasonPackage=1                
ORDER BY  ETA.ID, CASE  WHEN ISNULL(ETA.SRCitizenQty,0) <>0 THEN TC.Name +' (Senior Citizen Qty - ' + 
CONVERT(VARCHAR(50),ETA.SRCitizenQty) +')'                                                                          
ELSE TC.Name END            
                                                     
END 