CREATE PROCEDURE [dbo].[BO_GetRASVEvents] --4773                                         
(                                            
	@RetailerId BIGINT                                            
)                                                               
AS                                   
BEGIN                
	SELECT ED.Id, 
	CASE ED.Id WHEN 23857 THEN 'Day Tickets' WHEN 23859 THEN 'After 5pm Tickets' ELSE ED.Name END AS Name 
	FROM Events E WITH(NOLOCK)                  
	INNER JOIN BoUserVenues UV WITH(NOLOCK)  ON E.Id = UV.EventId    
	INNER JOIN EventDetails ED WITH(NOLOCK) ON UV.EventId=ED.EventId and UV.VenueId=ED.VenueId               
	AND UV.UserId =@RetailerId   
	AND UV.IsEnabled = 1 AND ED.Id Not In(23855,23856,23858,23860)                   
	Group by ED.Id, ED.Name          
END