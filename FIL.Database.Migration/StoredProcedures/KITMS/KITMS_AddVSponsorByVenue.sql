CREATE PROC [dbo].[KITMS_AddVSponsorByVenue]
(                                
	@VenueId BIGINT,   
	@SponsorId BIGINT,          
	@UpdatedBy VARCHAR(500)                         
)                              
AS                            
BEGIN   
   DECLARE @Events TABLE   
   (        
	   EventId BIGINT        
   )        
           
   INSERT INTO @Events SELECT EventId FROM EventDetails WITH(NOLOCK) WHERE VenueId =  @VenueId        
   DECLARE @EventId BIGINT        
   DECLARE @AltId VARCHAR(500)
   SELECT  @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName =@UpdatedBy

   DECLARE curEventId CURSOR FOR                         
   SELECT DISTINCT EventId FROM @Events        
   OPEN  curEventId;        
          
   FETCH NEXT FROM  curEventId INTO @EventId        
   WHILE @@FETCH_STATUS=0        
   BEGIN      
		IF((SELECT COUNT(*) FROM EventSponsorMappings WITH(NOLOCK) WHERE SponsorId= @SponsorId AND EventDetailId= @EventId)=0)      
		BEGIN  
			INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
			VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)  
		END  
		ELSE  
		BEGIN  
		PRINT 'Already inserted'  
		END  
     
  FETCH NEXT FROM  curEventId INTO @EventId        
  END        
                 
  CLOSE curEventId;  
  DEALLOCATE curEventId;
                            
END 