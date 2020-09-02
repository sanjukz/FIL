CREATE proc [dbo].[KITMS_AddMatchSponsor] --1195,8, 'sachin.jadhav@kyazoonga.com'
(          
	@EventId BIGINT,           
	@SponsorId BIGINT,          
	@UpdatedBy VARCHAR(500)           
)                  
AS                
BEGIN    
IF((SELECT COUNT(*) FROM EventSponsorMappings WITH(NOLOCK) WHERE SponsorId= @SponsorId and EventDetailId = @EventId)=0)    
BEGIN
	DECLARE @AltId VARCHAR(500)
	SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdatedBy
	INSERT INTO EventSponsorMappings(SponsorId, EventDetailId, IsEnabled, CreatedBy, CreatedUtc, UpdatedBy, UpdatedUtc)                  
	VALUES(@SponsorId, @EventId, 1, @AltId, GETDATE(),NULL,NULL)       
END       
END