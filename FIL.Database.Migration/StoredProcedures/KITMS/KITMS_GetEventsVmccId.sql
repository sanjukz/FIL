CREATE PROCEDURE [dbo].[KITMS_GetEventsVmccId]                                       
(                                                               
  @EventIds VARCHAR(MAX),                                            
  @VenueCatId BIGINT                                           
)                                            
AS                                            
BEGIN    
    
 SELECT  DISTINCT ETA.Id AS VmccId, ED.Id AS EventId, ED.Name AS EventName from EventTicketAttributes ETA WITH(NOLOCK) INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId  
 INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id    
where ETD.EventDetailId IN(SELECT *  FROM SplitString(@EventIds,',')) AND ETD.TicketCategoryId=@VenueCatId   
    
END 

