CREATE proc [dbo].[BO_GetVenueforBO]   
(                                                        
	@EventCatId INT ,                
	@RetailerId BIGINT                                                       
)                
AS                                                        
BEGIN
  SELECT V.Name AS VenueAddress,V.Id AS VenueId FROM EventDetails ED WITH(NOLOCK)               
  INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId=V.Id                
  INNER JOIN BoUserVenues UV WITH(NOLOCK) ON UV.venueid=ED.VenueId                
  WHERE UV.EventId=@EventCatId and UV.UserId=@RetailerId and UV.IsEnabled=1               
  GROUP BY V.Name,V.Id                
  UNION
  SELECT V.Name as VenueAddress,V.Id as VenueId FROM EventDetails ED WITH(NOLOCK)               
  INNER JOIN Venues V WITH(NOLOCK) on ED.VenueId=V.Id  WHERE ED.EventId=@EventCatId             
  GROUP BY V.Name,V.Id   
END     