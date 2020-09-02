CREATE PROC [dbo].[spGetLoginBasedVenueLayout]  --182272  
(                
    @EventId BIGINT,  
    @EventDetailId BIGINT,  
    @VenueId INT  
)                
AS                
BEGIN    
  
IF(@EventDetailId <= 0)  
BEGIN  
 SELECT TOP 1 @EventDetailId =  Id FROM EventDetails WITH (NOLOCK) WHERE VenueId = @VenueId AND EventId = @EventId AND StartDateTime >= GETUTCDATE()  
END  
SELECT ED.EventId AS EventType, ED.Name As EventName, ED.StartDateTime AS EventStartDate,    
CONVERT(TIME, ED.StartDateTime) AS EventStartTime, ED.EndDateTime AS EventEndDate,   
CONVERT(TIME, ED.EndDateTime) AS EventEndTime, VD.Id As VenueId,                
VD.Name As VenueAddress, CityM.Name As City_Name, ECD.Id As EventCatId, ECD.Name As EventCatName,   
ECD.EventCategoryId As P_id, ''AS PhotoPath, ED.Id As Match_id,   
'' AS InnerPageImage, '' AS EventHomePhoto,               
CASE ED.EventId WHEN 117849   
THEN '<span style="font-weight: bold; margin-right: 25px; font-size:13px;"> Only south stand </span>'          
ELSE '' END AS PackageDesc,  CASE WHEN ECD.IsEnabled=1 THEN 1 ELSE 0 END AS IsHomePage                
FROM EventDetails ED WITH(NOLOCK)                 
INNER JOIN Venues VD WITH(NOLOCK) ON VD.Id = ED.VenueId  
INNER JOIN Cities CityM WITH(NOLOCK) ON CityM.Id = VD.CityId  
INNER JOIN Events ECD WITH(NOLOCK) ON ED.EventId = ECD.Id            
WHERE ED.Id=@EventDetailId                 
  
SELECT DISTINCT           
ETA.Price AS PricePerTic,           
CM.Name As CurrencyName, CM.Id As Currencyid  
FROM EventDetails AS ED WITH (NOLOCK) INNER JOIN       
EventTicketDetails AS ETD WITH (NOLOCK) ON ED.Id = ETD.EventDetailId INNER JOIN  
EventTicketAttributes AS ETA WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId INNER JOIN   
CurrencyTypes CM WITH (NOLOCK) ON ETA.CurrencyId = CM.Id          
WHERE ED.Id=@EventDetailId AND ED.IsEnabled=1 AND ETA.IsEnabled=1                
AND ETA.RemainingTicketForSale > 0  
END 