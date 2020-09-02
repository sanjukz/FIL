CREATE PROCEDURE [dbo].[BO_GetEventVenueid]   
(            
	@EventCatId BIGINT,            
	@Venueid BIGINT             
)            
AS             
BEGIN  
	SELECT DISTINCT ED.Id As EventId,ED.Name  + ' - ' + CONVERT(VARCHAR(11),ED.StartDateTime,113)+ ', ' + 
	CONVERT(varchar(5),ED.StartDateTime,108) as EventName, ED.StartDateTime,Convert(Varchar(5), 
	ED.StartDateTime,108) FROM EventDetails ED  WITH(NOLOCK)
	WHERE EventId =@EventCatId  and ED.VenueId in(@Venueid) AND ED.IsEnabled = 1 ORDER BY ED.StartDateTime
END    