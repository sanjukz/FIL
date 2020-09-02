CREATE PROC [dbo].[BO_TransactionRevert]            
(                                  
	@EventTransId BIGINT,                                   
	@ReturnVal BIGINT OUTPUT                                  
)                                  
AS                                  
BEGIN TRANSACTION
CREATE TABLE #Vmcc_ids(VmccId BIGINT, NoOfTic BIGINT, TicketTypeId INT)             
      
DECLARE @TransactionBy BIGINT                    
DECLARE @TotalTicAmount BIGINT                    
DECLARE @RetailerId BIGINT                    
SET @TransactionBy =(SELECT ChannelId FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransId AND TransactionStatusId = 8)                    
      
INSERT INTO #Vmcc_ids 
SELECT EventTicketAttributeId,TotalTickets,TicketTypeId FROM TransactionDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId

SELECT * FROM   #Vmcc_ids                 
IF(@TransactionBy=4)                    
BEGIN                     
  DECLARE VMCCIds CURSOR FOR                     
  SELECT * FROM #Vmcc_ids                    
  DECLARE @VMCC_Id BIGINT                    
  DECLARE @NoOfTic BIGINT             
  DECLARE @TicketType INT                    
                
  OPEN VMCCIds;                     
  FETCH NEXT FROM VMCCIds INTO @VMCC_Id,@NoOfTic,@TicketType
  WHILE @@FETCH_STATUS=0                            
  BEGIN               
   IF(@TicketType=2)                    
   BEGIN          
	   SELECT RemainingTicketForSale,CHildQty FROM EventTicketAttributes WITH(NOLOCK) WHERE ID =@VMCC_Id       
	   UPDATE EventTicketAttributes SET  RemainingTicketForSale += @NoOfTic, CHildQty+=@NoOfTic WHERE Id = @VMCC_Id                    
	   SELECT RemainingTicketForSale, CHildQty FROM EventTicketAttributes WITH(NOLOCK) WHERE Id =   @VMCC_Id                
   END                    
   ELSE IF(@TicketType=3)            
   BEGIN            
	   SELECT ISNULL(SRCitizenQTY,0)AS [SRCitizenQTY] FROM EventTicketAttributes WITH(NOLOCK) WHERE Id =@VMCC_Id                   
	   UPDATE EventTicketAttributes set RemainingTicketForSale += @NoOfTic, SRCitizenQTY += @NoOfTic WHERE Id=@VMCC_Id                   
	   SELECT SRCitizenQTY from EventTicketAttributes WITH(NOLOCK) WHERE Id =  @VMCC_Id             
   END                       
   ELSE                    
   BEGIN                 
	   SELECT RemainingTicketForSale FROM EventTicketAttributes WITH(NOLOCK) WHERE Id =   @VMCC_Id                   
	   UPDATE EventTicketAttributes SET RemainingTicketForSale += @NoOfTic WHERE ID = @VMCC_Id                   
	   SELECT RemainingTicketForSale FROM EventTicketAttributes WITH(NOLOCK) WHERE Id =   @VMCC_Id                 
   END                 
    FETCH NEXT FROM VMCCIds INTO  @VMCC_Id,@NoOfTic,@TicketType                      
   END                    
   CLOSE VMCCIds;                    
   DEALLOCATE VMCCIds;       
END         
       
UPDATE MatchSeatTicketDetails SET TransactionId= NULL, SeatStatusId =1,PrintStatusId=1,BarcodeNumber=NULL, 
TicketTypeId=NULL, Printcount=0 WHERE TransactionId= @EventTransId                    
UPDATE BO_TransactionRevertRequest SET IsRevert = 1 WHERE  Transid= @EventTransId      
UPDATE Transactions SET TransactionStatusId=0 WHERE ID=  @EventTransId         
DROP TABLE #Vmcc_ids           
IF (@@ERROR <> 0)                                         
BEGIN                                        
	ROLLBACK TRAN                                              
	SET @ReturnVal = 0                                      
END                                        
Else                                        
BEGIN                                        
	COMMIT TRANSACTION                                        
	SET @ReturnVal = 1                                
END   