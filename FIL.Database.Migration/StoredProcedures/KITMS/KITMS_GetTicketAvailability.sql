CREATE PROCEDURE [dbo].[KITMS_GetTicketAvailability]                                      
(                                                               
  @EventIds VARCHAR(MAX),                                            
  @VenueCatId BIGINT,                                             
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
DECLARE @TotalTic BIGINT, @EventName VARCHAR(500)                      
    
DECLARE curEvent CURSOR FOR                                       
SELECT * FROM @tblEvent    
                   
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN                    
  SELECT @TotalTic =RemainingTicketForSale FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VmccId                      
  SELECT @EventName = ED.Name FROM EventDetails ED WITH(NOLOCK) WHERE ED.Id= @EventId     
      
  IF(@TotalTic-@TicketQty < 0)                    
  BEGIN     
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TotalTic) + ' Tickets are Availble for '+ @EventName + ',<br/> '    
  END    
      
FETCH NEXT FROM curEvent INTO @VmccId, @EventId     
END    
CLOSE curEvent;                     
DEALLOCATE curEvent;     
    
SELECT @ReturnVal AS Status    
    
END 

