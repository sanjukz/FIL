CREATE PROCEDURE [dbo].[KITMS_ReleaseTicketsForSponsorByVenue]
(                                              
 @SponsorId BIGINT,                                              
 @EventIds varchar(max),                                              
 @VenueCatId BIGINT,                                               
 @tics INT,                                              
 @UpdateBY VARCHAR(500)                                               
)                                              
AS                                              
BEGIN      
      
BEGIN TRANSACTION trans      
BEGIN TRY      
                                
declare @BlockTic BIGINT=0, @AvailableTic BIGINT=0, @KM_ID BIGINT=0, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',    
@Flag VARCHAR(50) = 'Default',@AltId VARCHAR(500),@EventId BIGINT, @EventTicketDetailId BIGINT, @BlockSeatCount INT, @VmccId BIGINT, @Price DECIMAL(18,2)    
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY       
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                    
INSERT INTO #vmccIds    
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A WITH(NOLOCK)    
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))    
    
DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds    
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                     
while @@FETCH_STATUS=0                      
begin           
 SET @Flag='Default'     
 SELECT @KM_ID = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId AND SponsorId=@SponsorId AND IsEnabled = 1    
 SELECT @BlockTic = ISNULL(AllocatedTickets,0),@AvailableTic=ISNULL(RemainingTickets,0),@Price =Price From  CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id= @KM_ID    
 IF(@tics < @AvailableTic+1)          
 BEGIN          
  UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @tics,     
  RemainingTickets = RemainingTickets -@tics  WHERE Id = @KM_ID                    
  UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale + @tics WHERE Id=@VmccId     
  DECLARE @tblBlockedSeatIds TABLE(MatchLayoutSectionSeatId BIGINT)        
 INSERT INTO @tblBlockedSeatIds        
 SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails A WITH(NOLOCK)        
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON A.EventTicketDetailId=ETD.Id  
 INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId=ETD.Id  
 WHERE A.SponsorId = @SponsorId AND ETA.Id = @VmccId AND B.SeatTypeId =3        
 AND A.SeatStatusId=1    
    
  UPDATE MatchSeatTicketDetails SET SponsorId= NULL WHERE MatchLayoutSectionSeatId IN (SELECT top ( @tics) MatchLayoutSectionSeatId FROM @tblBlockedSeatIds order by MatchLayoutSectionSeatId desc)        
  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 1 WHERE Id IN (SELECT top ( @tics) MatchLayoutSectionSeatId FROM @tblBlockedSeatIds order by MatchLayoutSectionSeatId desc)        
           
  INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)    
  VALUES(@KM_ID,2,@tics,@Price,1,GETDATE(),@AltId);    
  SET @Flag='Changed'    
 END          
 SELECT @EventName = ED.Name from EventDetails ED WITH(NOLOCK) WHERE Id= @EventId    
 IF(@Flag='Changed')                  
 BEGIN                  
  SET @ReturnVal += CONVERT(Varchar(100),@tics) + ' tickets released successfully for ' +  @EventName+ ',<br/> '                   
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