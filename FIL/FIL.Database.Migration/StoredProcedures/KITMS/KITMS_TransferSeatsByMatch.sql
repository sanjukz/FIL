CREATE PROCEDURE [dbo].[KITMS_TransferSeatsByMatch]                                             
(                                                
 @SponsorId BIGINT,        
 @SponsorToId BIGINT,                                              
 @EventId BIGINT,        
 @Seats VARCHAR(MAX),                                                    
 @VenueCatId BIGINT,                                                 
 @TicketQty INT,                                                
 @UpdateBY VARCHAR(500)                                                 
)                                                
AS                                                
BEGIN        
        
BEGIN TRANSACTION trans        
BEGIN TRY        
                                      
DECLARE @BlockTic BIGINT=0                                    
DECLARE @AvailableTic BIGINT=0                                    
DECLARE @KM_ID BIGINT=0                          
DECLARE @EventName VARCHAR(255)                                  
DECLARE @TicketForSale BIGINT                                    
DECLARE @ReturnVal VARCHAR(5000) =''                                     
DECLARE @Flag VARCHAR(50) = 'Default'        
DECLARE @Price DECIMAL        
       
           
DECLARE @KM_IDNew  BIGINT        
DECLARE @VmccId_New BIGINT              
DECLARE @BlockTic_New BIGINT               
DECLARE @AvailableTic_New BIGINT             
           
SET @Flag='Default'       
                       
DECLARE @VmccId BIGINT,@AltId NVARCHAR(500)

SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY

SELECT @VmccId = ETA.Id FROM EventTicketAttributes ETA WITH (NOLOCK) 
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId 
WHERE TicketCategoryId = @VenueCatId AND ETD.EventDetailId = @EventId         
       
      
SET @KM_ID = (SELECT ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @SponsorId            
and EventTicketAttributeId = @VmccId)       
SET @Price=   (SELECT Price FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @SponsorId            
and EventTicketAttributeId = @VmccId)          
            
SELECT @BlockTic = AllocatedTickets,@AvailableTic=RemainingTickets FROM  CorporateTicketAllocationDetails WITH (NOLOCK) WHERE EventTicketAttributeId = @KM_ID                 
 IF(@KM_ID > 0)       
 BEGIN      
 IF(@TicketQty < @AvailableTic+1)                
 BEGIN                
              
  SET @BlockTic = @BlockTic-@TicketQty                
  SET @AvailableTic = @AvailableTic-@TicketQty              
            
  UPDATE CorporateTicketAllocationDetails set AllocatedTickets = @BlockTic, RemainingTickets = @AvailableTic WHERE Id = @KM_ID              
                
  SELECT @KM_IDNew=Id,@BlockTic_New = AllocatedTickets,@AvailableTic_New=RemainingTickets  FROM  CorporateTicketAllocationDetails WITH (NOLOCK)    
  WHERE SponsorId = @SponsorToId AND EventTicketAttributeId = @VmccId              
                 
  SET @BlockTic_New = @BlockTic_New+@TicketQty              
  SET @AvailableTic_New = @AvailableTic_New+@TicketQty              
                
  IF(@KM_IDNew IS NOT NULL)              
  BEGIN              
  UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = @BlockTic_New, RemainingTickets = @AvailableTic_New WHERE Id = @KM_IDNew               
  END              
  ELSE              
  BEGIN              
    INSERT INTO CorporateTicketAllocationDetails VALUES(newId(),@VmccId,@SponsorToId,@TicketQty,@TicketQty,@Price,Null,null,1,GETDATE(),NewID(),getdate(),@AltId)             
  END              
        
  update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId 
  from MatchLayoutSectionSeats MLSS  WITH(NOLOCK)    
  INNER JOIN MatchLayoutSections MLS  WITH(NOLOCK) ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN    
   MatchLayouts ML  WITH(NOLOCK) ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId =@EventId)    
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))
       
    update MatchSeatTicketDetails set SponsorId=@SponsorId where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD  WITH(NOLOCK)
	 Inner Join MatchLayoutSectionSeats MLSS WITH(NOLOCK) On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  WITH(NOLOCK)   
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId     
   where ML.EventDetailId =@EventId and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1    
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))    
         
  INSERT INTO CorporateTicketAllocationDetailLogs VALUES(@KM_ID,@KM_IDNew,3,@TicketQty,@Price,1,GetDate(),NEWID(),GETDATE(),@AltId)        
       
     SET @Flag='Changed'        
END             
END      
IF(@Flag='Changed')                    
BEGIN           
 DECLARE @SponsorName VARCHAR(500), @SponsorToName VARCHAR(500)      
     SELECT @SponsorName = SponsorName FROM Sponsors WITH(NOLOCK) WHERE Id = @SponsorId      
     SELECT @SponsorToName = SponsorName FROM Sponsors WITH(NOLOCK) WHERE Id = @SponsorToId       
           
     SELECT @EventName = ED.name FROM EventDetails ED WITH (NOLOCK) INNER JOIN EventTicketDetails VMCC WITH (NOLOCK) ON       
 VMCC.EventDetailId = ED.EventId where VMCC.EventDetailId= @EventId        
           
 SET @ReturnVal += CONVERT(VARCHAR(100),@TicketQty) + ' setas transfered successfully for <b>' +  @EventName+ '</b> from '+@SponsorName+ ' to '+ @SponsorToName                    
END         
ELSE                    
BEGIN             
 SELECT @EventName = ED.name FROM EventDetails ED WITH(NOLOCK) INNER JOIN EventTicketDetails VMCC  WITH(NOLOCK) ON       
 VMCC.EventDetailId = ED.EventId where VMCC.EventDetailId= @EventId                      
 SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@AvailableTic) + ' seats are avaialble for transfer for <b>'+ @EventName + '</b>'                    
END                    
           
SELECT @ReturnVal                             
COMMIT TRANSACTION trans        
END TRY        
BEGIN CATCH        
ROLLBACK TRANSACTION trans         
END CATCH                                 
END 


