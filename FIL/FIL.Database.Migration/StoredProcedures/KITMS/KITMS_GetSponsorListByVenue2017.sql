CREATE PROCEDURE [dbo].[KITMS_GetSponsorListByVenue2017] --55, 55                    
(                            
	@VenueId BIGINT,
	@EventCatId BIGINT
)                            
AS                            
BEGIN    
                        
SELECT  SM.SponsorName,SM.Id AS SponsorId FROM Sponsors SM WITH(NOLOCK) WHERE SM.IsEnabled=1 
ORDER BY SM.SponsorName ASC  
       
SELECT  SM.SponsorName,SM.Id AS SponsorId                             
FROM Sponsors SM  WITH(NOLOCK)                              
WHERE SM.Id IN ( SELECT DISTINCT SponsorId FROM EventSponsorMappings MS WITH(NOLOCK) WHERE MS.EventDetailId IN
(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND EventId= @EventCatId) AND  MS.IsEnabled =1) 
AND SM.IsEnabled=1 ORDER BY SM.SponsorName ASC  
                             
END 