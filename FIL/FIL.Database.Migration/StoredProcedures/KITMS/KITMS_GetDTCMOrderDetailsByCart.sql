CREATE PROCEDURE [dbo].[KITMS_GetDTCMOrderDetailsByCart]      
(        
 @SponsorId BIGINT,    
 @EventId BIGINT,    
 @VenueCatId BIGINT,     
 @Quantity INT    
)        
AS        
BEGIN       
    
  DECLARE @VmccId INT    
select @VmccId=ETA.Id FROM EventTicketAttributes ETA With (NOLOCK) INNER JOIN EventTicketDetails ETD With (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId      
where ETD.EventDetailId =@EventId AND ETD.TicketCategoryId=@VenueCatId   
  
 SELECT 0 as MapId  
 Select 0 as DTCMOrderId  
 Select Price from EventTicketAttributes With (NOLOCK) where Id=@VmccId  
 Select 0 as Quantity  
 Select 0 as BasketId  
 Select 0 as BasketAmount  
  
    
END 