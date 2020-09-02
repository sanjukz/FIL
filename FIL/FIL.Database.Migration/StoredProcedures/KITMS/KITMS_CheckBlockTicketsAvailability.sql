CREATE PROCEDURE [dbo].[KITMS_CheckBlockTicketsAvailability]                                 
(                                                             
	 @EventIds VARCHAR(MAX),                                        
	 @VenueCatId BIGINT,                                         
	 @TicketQty INT                                         
)     

AS                                        
BEGIN  
           
DECLARE @vmccIds TABLE(VmccId BIGINT, EventId BIGINT)  
  
INSERT INTO @vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A WITH(NOLOCK) 
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,',')) 
DECLARE @Status TABLE(VmccId BIGINT, EventId BIGINT, EventName VARCHAR(500), TicketAvaliable BIGINT, Status BIGINT) 

DECLARE @EventId BIGINT, @VmccId BIGINT, @EventName VARCHAR(500), @TicketForSale INT
DECLARE curEvent CURSOR FOR SELECT * FROM @vmccIds
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN
		SELECT @TicketForSale =RemainingTicketForSale FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VmccId
		SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId

		IF(@TicketForSale-@TicketQty>=0)                
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@TicketForSale, 1)
		END
		ELSE
		BEGIN
			INSERT INTO @Status VALUES(@VmccId, @EventId, @EventName,@TicketForSale, 0)
		END    

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;  
        
SELECT * FROM  @Status              
END