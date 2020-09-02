CREATE PROCEDURE [dbo].[KITMS_GetTicketAvaialabilityForTransaction_Temp]  --140,'58618,58620,58622','14180,14181',2641,10,'KyaZoongaTom'                                    
(                                      
	 @SponsorId BIGINT,                                      
	 @EventIds VARCHAR(MAX),    
	 @KM_Ids VARCHAR(MAX),                                  
	 @VenueCatId BIGINT,                                       
	 @tics INT,                                      
	 @UpdateBY VARCHAR(500)                                       
)                                      
AS                                      
BEGIN

BEGIN TRANSACTION trans
BEGIN TRY
                                    
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @EventTicketAttributeId BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='' 
DECLARE @ReturnValFail VARCHAR(5000) ='', @Count int =0                 
            
DECLARE  @KMIDS TABLE(KMID BIGINT)
INSERT INTO @KMIDS 
SELECT * FROM SplitString(@KM_Ids,',')

DECLARE @KM_ID BIGINT
DECLARE curEvent CURSOR FOR                                 
SELECT * FROM @KMIDS              
OPEN  curEvent;               
FETCH NEXT FROM curEvent INTO @KM_ID              
WHILE @@FETCH_STATUS=0              
BEGIN   

	SELECT @BlockTic = AllocatedTickets, @AvailableTic = RemainingTickets, @EventTicketAttributeId = EventTicketAttributeId
	FROM CorporateTicketAllocationDetails WITH(NOLOCK) 
	WHERE SponsorId= @SponsorId AND Id=@KM_ID

	IF(@AvailableTic >= @tics)              
	BEGIN
		SET @ReturnVal=1   	                      
	END                  
	ELSE                          
	BEGIN                
		SELECT @EventName = ED.Name 
		FROm EventDetails ED WITH(NOLOCK) 
		INNER JOIN EventTicketDetails ETD WITH(NOLOCK) on ED.Id = ETD.EventDetailId
		INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on ETD.Id = SM.EventTicketDetailId
		WHERE SM.Id= @EventTicketAttributeId 
		SET @Count+=1   
		SET @ReturnValFail += 'Only '+ CONVERT(Varchar(100),@AvailableTic) + ' tickets are available for transaction for <b>'+ @EventName + '</b>,<br/> '           
	END                          
FETCH NEXT FROM curEvent INTO @KM_ID              
END               
CLOSE curEvent;               
DEALLOCATE curEvent;              

IF(@Count>0)                          
BEGIN                       
	SELECT @ReturnValFail AS Result 
END 
ELSE
BEGIN
	SELECT @ReturnVal AS Result
END             

COMMIT TRANSACTION trans
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION trans 
END CATCH                              
END 