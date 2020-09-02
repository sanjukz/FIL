CREATE PROCEDURE [dbo].[KITMS_GetMatchesByVenue2017] --12,17
(                                                         
	@VenueId BIGINT,
	@EventCatId BIGINT                                                   
)                                                      
AS                                                      
BEGIN
	SELECT ED.Id AS EventId,  ED.Name + ' - ' +CONVERT(VARCHAR(17),ED.StartDateTime,113)  AS EventName      
	FROM EventDetails ED  WITH(NOLOCK)                      
	WHERE ED.VenueId=@VenueId --AND Ed.StartDateTime>=(GETDATE()-45)
	AND ED.EventId= @EventCatId
	ORDER BY Ed.StartDateTime ASC
END