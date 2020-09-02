CREATE PROCEDURE [dbo].[KITMS_ReleaseSeatsByMatch]                                         
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
                    
declare @BlockTic BIGINT=0, @AvailableTic BIGINT=0, @KM_ID BIGINT=0, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',    
@Flag VARCHAR(50) = 'Default',@AltId VARCHAR(500),@EventId BIGINT, @VmccId BIGINT, @Price DECIMAL(18,2)    
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY    
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                    
INSERT INTO #vmccIds    
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A  WITH(NOLOCK)   
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))                   
  
DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds    
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                     
while @@FETCH_STATUS=0                  
BEGIN         
 SET @Flag='Default'   
   
  SELECT @KM_ID = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId AND SponsorId=@SponsorId AND IsEnabled = 1       
  SELECT @BlockTic = ISNULL(AllocatedTickets,0),@AvailableTic=ISNULL(RemainingTickets,0),@Price =Price From  CorporateTicketAllocationDetails WITH(NOLOCK) where Id= @KM_ID        
  
 IF(@TicketQty < @AvailableTic+1)        
 BEGIN        
	UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @TicketQty,     
	RemainingTickets = RemainingTickets -@TicketQty  WHERE Id = @KM_ID   
         
	UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale + @TicketQty WHERE Id=@VmccId

	update MatchLayoutSectionSeats set SeatTypeId=1 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS  WITH(NOLOCK)
	INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN
	MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )
	and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))

	update MatchSeatTicketDetails set SponsorId=null where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD WITH(NOLOCK) Inner Join MatchLayoutSectionSeats MLSS WITH(NOLOCK) On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
WITH(NOLOCK)
	ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON  ETD.ID=MSTD.EventTicketDetailId 
	where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1
	and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,','))) 
    
	INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, 
	CreatedUtc, CreatedBy)
	VALUES(@KM_ID,2,@TicketQty,@Price,1,GETDATE(),@AltId);

  SET @Flag='Changed'    
 END        
  SELECT @EventName = ED.Name from EventDetails ED WITH(NOLOCK) WHERE Id= @EventId    
 IF(@Flag='Changed')                  
 BEGIN                  
  SET @ReturnVal += CONVERT(Varchar(100),@TicketQty) + ' tickets released successfully for ' +  @EventName+ ',<br/> '                   
 END       
 ELSE               
 BEGIN                 
  SET @ReturnVal += 'Only '+ CONVERT(Varchar(100),@AvailableTic) + ' tickets are avaialble to release for '+ @EventName + ',<br/> '                  
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

