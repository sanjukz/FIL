CREATE PROCEDURE [dbo].[KITMS_CheckBlockSeatAvailability]    
(                                                                
  @EventIds VARCHAR(MAX)='',                                            
  @VenueCatId BIGINT ,      
  @SponsorId INT,                                           
  @TicketQty INT                                             
)                                            
AS                                            
BEGIN    
     
DECLARE  @tblEvent  TABLE                  
(        
 EventId BIGINT,       
 VmccId BIGINT    
)                    
    
INSERT INTO @tblEvent    
SELECT ETA.Id , ETD.EventDetailId from EventTicketAttributes ETA WITH(NOLOCK) INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.Id=ETA.EventTicketDetailId    
where ETD.EventDetailId IN(SELECT *  FROM SplitString(@EventIds,',')) AND ETD.TicketCategoryId=@VenueCatId   
    
DECLARE @ReturnVal VARCHAR(MAX) =''     
DECLARE @EventId BIGINT                      
DECLARE @VmccId BIGINT    
DECLARE @KMId BIGINT, @EventName VARCHAR(500) , @AvailableTic INT                    
    
DECLARE curEvent CURSOR FOR                                       
SELECT * FROM @tblEvent    
                   
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN                    
  SELECT @KMId =ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @SponsorId          
  AND  EventTicketAttributeId = @VmccId                  
  SELECT @EventName = ED.Name FROM EventDetails ED WITH(NOLOCK) WHERE ED.Id= @EventId     
  SELECT @AvailableTic=ISNULL(RemainingTickets,0) FROM  CorporateTicketAllocationDetails  WITH(NOLOCK) where Id= @KMId     
      
  IF(@TicketQty > @AvailableTic)                    
  BEGIN     
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@AvailableTic) + ' Tickets are Availble for '+ @EventName + ',<br/> '    
  END    
      
FETCH NEXT FROM curEvent INTO @VmccId, @EventId     
END    
CLOSE curEvent;                     
DEALLOCATE curEvent;     
    
SELECT @ReturnVal AS Status    
    
END   