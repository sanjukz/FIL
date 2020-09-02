CREATE PROCEDURE [dbo].[KITMS_TransferCategoryTicketsToSponsor]-- 3450,2125,272, 5, 'sachin.jadhav@kyazoonga.com',2875 
(                                        
   @EventId BIGINT  = 3450,   
  @SponsorToId BIGINT = 2125,                     
  @VenueCatId BIGINT = 272,                         
  @TicketQty INT = 5,                        
  @UpdateBY VARCHAR(500) = 'sachin.jadhav@kyazoonga.com',   
  @SponsorFromId BIGINT    = 2875                
)                        
AS                        
BEGIN     
  
BEGIN TRANSACTION trans    
BEGIN TRY               
DECLARE @BlockTic BIGINT           
DECLARE @AvailableTic BIGINT           
DECLARE @TicketForSale BIGINT  
DECLARE @Price DECIMAL            
               
      
DECLARE @BlockTic_New BIGINT = 0        
DECLARE @AvailableTic_New BIGINT = 0          
DECLARE @ReturnVal VARCHAR(5000)              
DECLARE @Flag VARCHAR(50) = 'Default'            
            
DECLARE @TempTable TABLE  
(  
 BlockTic INT, AvailableTic INT, Status INT  
)   
             
DECLARE @KMTransferFromId BIGINT =0  
DECLARE @KMTransferToId BIGINT =0  
DECLARE @VmccId BIGINT =0 , @AltId NVARCHAR(500)

SELECT @AltId = AltId FROM Users WITH(NOLOCK) WHERE UserName = @UpdateBY 
  
SELECT @VmccId = A.Id  FROm EventTicketAttributes A  WITH(NOLOCK)   
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId =@EventId  
SET @Price=   (SELECT Price FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId = @SponsorFromId            
and EventTicketAttributeId = @VmccId)  
    
SELECT @KMTransferFromId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
AND SponsorId=@SponsorFromId AND IsEnabled = 1  
  
  
SELECT @KMTransferToId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
AND SponsorId=@SponsorToId AND IsEnabled = 1  
  
IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KMTransferFromId)    
BEGIN          
         
SELECT @BlockTic = AllocatedTickets,@AvailableTic=RemainingTickets FROM  CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id = @KMTransferFromId             
        
 IF(@TicketQty < @AvailableTic+1)            
 BEGIN            
          
   SET @BlockTic = @BlockTic-@TicketQty            
   SET @AvailableTic = @AvailableTic-@TicketQty          
            
   UPDATE CorporateTicketAllocationDetails set AllocatedTickets = @BlockTic, RemainingTickets = @AvailableTic WHERE Id = @KMTransferFromId          
             
   SELECT @BlockTic_New = ISNULL(AllocatedTickets,0),@AvailableTic_New=ISNULL(RemainingTickets,0) FROM  
   CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id = @KMTransferToId      
     
   SET @BlockTic_New = @BlockTic_New+@TicketQty          
   SET @AvailableTic_New = @AvailableTic_New+@TicketQty     
             
   IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KMTransferToId)     
   BEGIN       
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = @BlockTic_New, RemainingTickets = @AvailableTic_New WHERE Id = @KMTransferToId           
   END          
   ELSE          
   BEGIN  
   INSERT INTO CorporateTicketAllocationDetails VALUES(newId(),@VmccId,@SponsorToId,@TicketQty,@TicketQty,@Price,Null,null,1,GETDATE(),NewID(),getdate(),@AltId)           
   END   
     
   IF NOT EXISTS(SELECT * FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Sponsorid=@SponsorToId AND  EventTicketAttributeId=@VmccId)     
   BEGIN       
   INSERT INTO EventSponsorMappings VALUES(@SponsorToId, @EventId,1,NEWID(), GETDATE(),null,@AltId)          
   END            
    INSERT INTO CorporateTicketAllocationDetailLogs VALUES(@KMTransferFromId,NULL,3,@TicketQty,@Price,1,GetDate(),NEWID(),GETDATE(),@AltId)            
  
   INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 1)  
   SELECT * FROM @TempTable  
             
END     
ELSE                
BEGIN                
   INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 0)  
   SELECT * FROM @TempTable    
END           
END   
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                  
END


