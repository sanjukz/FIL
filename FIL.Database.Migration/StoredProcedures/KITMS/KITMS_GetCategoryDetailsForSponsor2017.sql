CREATE PROC [dbo].[KITMS_GetCategoryDetailsForSponsor2017]      
(        
 @VenueId BIGINT,      
 @VenueCatId BIGINT,      
 @EventCatId BIGINT,      
 @SponsorId BIGINT                        
)      
AS                    
BEGIN         
      
DECLARE @EventType BIGINT      
DECLARE @Cnt BIGINT            
      
SET @EventType = @EventCatId                       
      
DECLARE @tblVmmc TABLE      
(      
 VmccId BIGINT,      
 EventID BIGINT      
)                                        
      
INSERT INTO @tblVmmc  select ETA.Id,ETD.EventDetailId FROM EventTicketAttributes ETA WITH(NOLOCK)
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId    
where ETD.EventDetailId IN(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND eventId =@EventType) 
AND ETD.TicketCategoryId=@VenueCatId     
      
SELECT @Cnt= COUNT(*) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  SponsorId=@SponsorId     
 AND EventTicketAttributeId IN (SELECT DISTINCT VmccId FROM @tblVmmc)      
      
IF (@Cnt <> 0)      
BEGIN           
 SELECT SM.Id AS SponsorId, SM.SponsorName, ISNULL(SUM(CTAD.AllocatedTickets),0) AS SponsorBlocked, ISNULL(SUM(CTAD.RemainingTickets),0) AS UnClassifiedBlocked          
 FROM CorporateTicketAllocationDetails CTAD WITH(NOLOCK) LEFT OUTER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id= CTAD.SponsorId                          
 WHERE  EventTicketAttributeId IN (SELECT DISTINCT VmccId FROM @tblVmmc)  AND CTAD.SponsorId=@SponsorId AND CTAD.IsEnabled = 1      
 GROUP BY  SM.Id, SM.SponsorName       
END                                      
ELSE      
BEGIN           
   SELECT Id As SponsorId, SponsorName,0 AS SponsorBlocked, 0 AS UnClassifiedBlocked      
   FROm Sponsors WITH(NOLOCK) WHERE Id=@SponsorId      
END      
      
SELECT ISNULL(SUM(AvailableTicketForSale),0) AS AvailableTicket FROM EventTicketAttributes ETA WITH(NOLOCK) INNER JOIN EventTicketDetails ETD  WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId    
where ETD.EventDetailId IN(SELECT Id FROM EventDetails  WITH(NOLOCK) WHERE VenueId = @VenueId AND eventId =@EventType) AND ETD.TicketCategoryId=@VenueCatId     
      
END 


