CREATE PROCEDURE [dbo].[KITMS_BlockTicketsForSponsorByVenue_Temp]
(                                        
	 @SponsorId BIGINT,
	 @EventIds VARCHAR(MAX),
	 @VenueCatId BIGINT,
	 @tics INT,
	 @UpdateBY VARCHAR(500)
)                                        
AS  
BEGIN
BEGIN TRANSACTION trans
BEGIN TRY
                                      
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @KM_ID BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',
@Flag VARCHAR(50) = 'Default',@EventId BIGINT, @VmccId BIGINT,@AltId VARCHAR(500), @KITMSMasterId BIGINT, @Price DECIMAL(18,2)
SELECT  @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName =@UpdateBY
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                
INSERT INTO #vmccIds
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A WITH(NOLOCK) 
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))

DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds
OPEN  curEvent;                 
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
	WHILE @@FETCH_STATUS=0                
	BEGIN                
		SELECT @TicketForSale =RemainingTicketForSale, @Price= LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VmccId
		IF(@TicketForSale-@tics>=0)                
		BEGIN
			
			IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId
			AND SponsorId=@SponsorId AND IsEnabled = 1)
			BEGIN
				SELECT @KITMSMasterId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId
				AND SponsorId=@SponsorId AND IsEnabled = 1
				UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@tics, RemainingTickets = RemainingTickets+@tics,
				UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KITMSMasterId 
			     --PRINT 'Updated CorporateTicketAllocationDetails ' +CONVERT(VARCHAR(200),@KITMSMasterId) +'-'+CONVERT(VARCHAR(200),@VmccId)+'-'+CONVERT(VARCHAR(200),@SponsorId)
			END
			ELSE
			BEGIN
				INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price, 
				IsEnabled, CreatedUtc, CreatedBy)                      
				VALUES(NEWID(),@VMCCId, @SponsorId,@tics,@tics,@Price,1,GETDATE(),@AltId)
				SET @KITMSMasterId = SCOPE_IDENTITY()
			END    
			UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @tics WHERE Id=@VMCCId
			INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)
			VALUES(@KITMSMasterId,1,@tics,@Price,1,GETDATE(),@AltId);             

			SELECT @EventName = ED.Name FROM EventDetails ED WITH(NOLOCK) WHERE Id= @EventId
			SET @Flag='Changed'
			IF(@Flag='Changed')                            
			BEGIN                       
				SET @ReturnVal += CONVERT(VARCHAR(100),@tics) + ' Tickets Blocked Successfully for ' + @EventName+ ',<br/> '   
			END  
		END
		ELSE                            
		BEGIN                  
			SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId 
			SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TicketForSale) + ' Tickets are Availble for '+ @EventName + ',<br/> '
	   END                            

FETCH NEXT FROM curEvent INTO @VmccId, @EventId                
END                 
                      
CLOSE curEvent;                 
DEALLOCATE curEvent;                

DROP TABLE #vmccIds          
SELECT @ReturnVal                     
COMMIT TRANSACTION trans  
END TRY  
BEGIN CATCH  
ROLLBACK TRANSACTION trans   
END CATCH                                
END