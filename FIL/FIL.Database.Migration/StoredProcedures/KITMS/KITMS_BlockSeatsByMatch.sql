CREATE PROCEDURE [dbo].[KITMS_BlockSeatsByMatch]                               
(                                          
  @SponsorId BIGINT,                                          
  @EventIds VARCHAR(MAX),  
  @Seats VARCHAR(MAX),        
  @VenueCatId BIGINT,                                           
  @TicketQty INT,                                          
  @UpdateBY VARCHAR(500)                                           
)                                          
AS                                          
BEGIN    
    
BEGIN TRANSACTION trans    
BEGIN TRY    
                                        
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @KM_ID BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',    
@Flag VARCHAR(50) = 'Default',@EventId BIGINT, @VmccId BIGINT,@AltId VARCHAR(500), @KITMSMasterId BIGINT, @Price DECIMAL(18,2)    
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY    
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                    
INSERT INTO #vmccIds    
  
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A  WITH(NOLOCK)   
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))                        
  
DECLARE @SponsorType INT = 1  
SELECT @SponsorType = SponsorTypeId FROm Sponsors WITH(NOLOCK) WHERE Id= @SponsorId  
  
DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds    
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN    
   
 IF NOT EXISTS(select MSTD.Id FROM  MatchSeatTicketDetails MSTD WITH(NOLOCK) 
 INNER JOIN MatchLayoutSectionSeats MLSS  WITH(NOLOCK) On MSTD.MatchLayoutSectionSeatId=MLSS.Id 
 INNER JOIN MatchLayoutSections MLS  WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id 
 INNER JOIN MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId 
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON  ETD.ID=MSTD.EventTicketDetailId   
 where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
 and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
    BEGIN              
  SELECT @TicketForSale =RemainingTicketForSale, @Price= LocalPrice FROM EventTicketAttributes WITH(NOLOCK) WHERE Id=@VmccId                    
  
   IF(@TicketForSale-@TicketQty>=0)                    
  BEGIN                  
  
   IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
   AND SponsorId=@SponsorId AND IsEnabled = 1)    
   BEGIN    
    SELECT @KITMSMasterId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
    AND SponsorId=@SponsorId AND IsEnabled = 1    
     
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@TicketQty, RemainingTickets = RemainingTickets+@TicketQty,    
    UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KITMSMasterId     
  
  INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)    
   VALUES(@KITMSMasterId,1,@TicketQty,@Price,1,GETDATE(),@AltId);
                               
  END  
  ELSE    
   BEGIN    
    INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price,     
    IsEnabled, CreatedUtc, CreatedBy)                          
    VALUES(NEWID(),@VMCCId, @SponsorId,@TicketQty,@TicketQty,@Price,1,GETDATE(),@AltId)    
    SET @KITMSMasterId = SCOPE_IDENTITY()    
   END        
   UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @TicketQty WHERE Id=@VMCCId    
   INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)    
   VALUES(@KITMSMasterId,1,@TicketQty,@Price,1,GETDATE(),@AltId);                    
    
  SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId                   
  IF(@SponsorType=0)  
  BEGIN  
  update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in 
  (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS WITH(NOLOCK) 
  INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id 
  INNER JOIN MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId 
  where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )  
  and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    
  update MatchSeatTicketDetails set SponsorId=@SponsorId where id in 
  (select MSTD.Id from  MatchSeatTicketDetails MSTD WITH(NOLOCK) 
  Inner Join MatchLayoutSectionSeats MLSS  WITH(NOLOCK) On MSTD.MatchLayoutSectionSeatId=MLSS.Id 
  INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id 
  INNER JOIN MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId 
  INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON  ETD.ID=MSTD.EventTicketDetailId   
  where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
  END  
  ELSE  
  BEGIN  
   update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in 
   (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS  WITH(NOLOCK)  
  INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN  
   MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )  
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    update MatchSeatTicketDetails set SponsorId=@SponsorId where id in 
	(select MSTD.Id from  MatchSeatTicketDetails MSTD  WITH(NOLOCK) Inner Join MatchLayoutSectionSeats MLSS  WITH(NOLOCK) On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS   WITH(NOLOCK)
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,','))  and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
  END  
   SET @Flag='Changed'    
   IF(@Flag='Changed')                                
   BEGIN                           
    SET @ReturnVal += CONVERT(VARCHAR(100),@TicketQty) + ' Tickets Blocked Successfully for ' + @EventName+ ',<br/> '       
   END   
  END                      
  
  ELSE                              
  BEGIN                    
   SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId   
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TicketForSale) + ' seats are availble for <b>'+ @EventName + '</b>, '  
    END                              
 END  
 ELSE                              
  BEGIN                    
   SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId   
   SET @ReturnVal += 'Sorry, seats are no longer availble for <b>'+ @EventName + '</b>, Please select again.'  
    END     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                  
END                   
                        
CLOSE curEvent;                   
DEALLOCATE curEvent;                  
  
SELECT @ReturnVal                       
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                                  
END



