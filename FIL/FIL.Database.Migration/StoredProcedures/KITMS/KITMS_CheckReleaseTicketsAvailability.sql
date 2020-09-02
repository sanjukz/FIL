CREATE PROCEDURE [dbo].[KITMS_CheckReleaseTicketsAvailability]                                 
(    
	 @SponsorId BIGINT,                                                
	 @EventIds VARCHAR(MAX),                                        
	 @VenueCatId BIGINT,                                         
	 @TicketQty INT                                         
)     

AS                                        
BEGIN  
           
DECLARE @vmccIds TABLE(VmccId BIGINT,EventId BIGINT)
INSERT INTO @vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A WITH(NOLOCK) 
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))       
DECLARE @Status TABLE(VmccId BIGINT, EventId BIGINT, EventName VARCHAR(500), TicketAvaliable BIGINT, Status BIGINT) 
DECLARE @EventId BIGINT, @VmccId BIGINT, @EventName VARCHAR(500), @AvailableTic INT

DECLARE curEvent CURSOR FOR SELECT * FROM @vmccIds            
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN      
	    
		SELECT @AvailableTic =RemainingTickets FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId=@VmccId AND SponsorId = @SponsorId AND IsEnabled=1
		SELECT @EventName = ED.Name FROM EventDetails ED WITH(NOLOCK) WHERE Id= @EventId

		IF(@TicketQty<=@AvailableTic)                
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@AvailableTic, 1)
		END
		ELSE
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@AvailableTic, 0)
		END    

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;  
        
SELECT * FROM  @Status              
END