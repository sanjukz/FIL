Create PROCEDURE [dbo].[KITMS_TransferDTCMOrderbyCart]      
(        
 @SponsorId BIGINT,    
 @EventId BIGINT,    
 @VenueCatId BIGINT,     
 @Quantity INT,    
 @TransferToSponsorId BIGINT    
)        
AS        
BEGIN       
    
  DECLARE @VmccId INT   
      
select @VmccId=ETA.Id FROM EventTicketAttributes ETA With (NOLOCK)INNER JOIN EventTicketDetails ETD With (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId      
where ETD.EventDetailId =@EventId  
AND ETD.TicketCategoryId=@VenueCatId     
    
END 