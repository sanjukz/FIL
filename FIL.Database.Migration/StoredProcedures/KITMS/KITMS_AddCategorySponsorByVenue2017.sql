CREATE PROC [dbo].[KITMS_AddCategorySponsorByVenue2017]     
(                         
	@SponsorId BIGINT,   
	@VenueId BIGINT, 
	@EventCatId BIGINT,                
	@VenueCatId BIGINT,                       
	@BlockTic BIGINT,                            
	@BlockBy VARCHAR(500)                       
)                              
AS                            
BEGIN               
           
CREATE TABLE #Events(EventId BIGINT,VmccId BIGINT, Price DECIMAL(18,2))          
          
INSERT INTO #Events 
SELECT B.EventDetailId, A.Id, A.LocalPrice FROM EventTicketAttributes A WITH(NOLOCK)
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT Id FROM EventDetails WITH(NOLOCK) WHERE EventId=@EventCatId)
        
DECLARE @EventName VARCHAR(500),  @Count BIGINT, @Msg VARCHAR(500), @AltId VARCHAR(500)
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@BlockBy
         
DECLARE @EventId BIGINT, @VmccId BIGINT, @Price DECIMAL(18,2)
DECLARE curEvent CURSOR FOR  SELECT * FROM #Events 
OPEN  curEvent;          
FETCH NEXT FROM curEvent INTO @EventId, @VmccId, @Price
WHILE @@FETCH_STATUS=0          
BEGIN      
IF((SELECT COUNT(*) FROM EventSponsorMappings WITH(NOLOCK) WHERE SponsorId= @SponsorId AND EventDetailId= @EventId)=0)      
BEGIN  
	INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
	VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)  
END
            
SET @EventName = (SELECT Name FROM EventDetails WITH(NOLOCK) WHERE Id =  @EventId)
IF((SELECT COUNT(*) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId= @SponsorId AND EventTicketAttributeId= @VmccId)=0)
BEGIN
	INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
	IsEnabled, CreatedUtc, CreatedBy)                      
	VALUES(NEWID(),@VmccId, @SponsorId,@BlockTic,@BlockTic,@Price,1,GETDATE(),@AltId)
END
FETCH NEXT FROM curEvent INTO @EventId, @VmccId, @Price        
END
SET @Msg += 'Inserted for ' + @EventName + ','                    
CLOSE curEvent;        
DEALLOCATE curEvent;        
           
SELECT @Msg  

DROP TABLE #Events;
END 