CREATE PROCEDURE [dbo].[KITMS_GetMatchSponsors]  --1195               
(                    
  @MatchId BIGINT                    
)                    
AS                    
BEGIN                    
SELECT EVD.Name AS EventName, EVD.StartDateTime AS EventStartDate, V.Name AS VenueAddress                        
FROM EventDetails EVD WITH(NOLOCK)                     
INNER JOIN Venues V WITH(NOLOCK) ON EVD.VenueId = V.Id WHERE EVD.Id = @MatchId                    

SELECT M.SponsorName,M.Id AS SponsorId,MS.EventDetailId AS EventId FROM Sponsors M WITH(NOLOCK)                    
INNER JOIN  EventSponsorMappings MS WITH(NOLOCK) ON M.Id = MS.SponsorId  WHERE MS.EventDetailId = @MatchId                    

SELECT  M.SponsorName,M.Id AS SponsorId
FROM Sponsors M  WITH(NOLOCK)                      
WHERE M.Id NOT IN ( SELECT MM.SponsorId from EventSponsorMappings MM WITH(NOLOCK) WHERE MM.EventDetailId = @MatchId) 
AND M.IsEnabled=1 ORDER BY M.SponsorName ASC

Select  M.SponsorName,M.Id AS SponsorId FROM Sponsors M WHERE M.Id IN (SELECT MM.SponsorId FROM 
EventSponsorMappings MM WITH(NOLOCK) WHERE MM.EventDetailId = @MatchId) 
AND M.IsEnabled=1 ORDER BY M.SponsorName ASC
                 
END