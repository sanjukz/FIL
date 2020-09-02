CREATE PROCEDURE [dbo].[KITMS_GetExistingCategorySponsors]      
(      
      
 @EventId BIGINT,      
 @VenueCatId BIGINT      
)      
AS      
BEGIN      
      
SELECT MS.Id AS SponsorId, CONVERT(VARCHAR(200),MS.SponsorName) +' (Avaiable Tic ' + CONVERT(VARCHAR(200), SUM(RemainingTickets)) +')' AS SponsorName                                     
FROM Sponsors MS WITH(NOLOCK)                                                      
INNER JOIN CorporateTicketAllocationDetails S WITH(NOLOCK) ON MS.Id = S.Sponsorid                                             
Where MS.IsEnabled=1 AND S.EventTicketAttributeId IN (
SELECT ETA.Id  FROM EventTicketAttributes ETA WITH(NOLOCK) 
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId    
where ETD.EventDetailId IN(SELECT Id FROM EventDetails  WITH(NOLOCK) WHERE Id =@EventId) AND ETD.TicketCategoryId=@VenueCatId)      
GROUP BY MS.Id,MS.SponsorName      
      
END 

