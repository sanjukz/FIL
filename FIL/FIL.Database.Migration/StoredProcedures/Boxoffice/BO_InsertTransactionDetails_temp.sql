CREATE PROC [dbo].[BO_InsertTransactionDetails_temp]                                
(                                    
	@TransId BIGINT,                                    
	@VMCC_Id BIGINT,                                    
	@EventId BIGINT,                                    
	@Quantity INT,                                    
	@PricePerTicket DECIMAL(18,2),                                     
	@DiscountAmount DECIMAL(18,2),                                    
	@ConvenienceCharge DECIMAL(18,2),                                    
	@ServiceTax DECIMAL(18,2),                                    
	@DeliveryCharge DECIMAL(18,2),                                    
	@TicNumIds VARCHAR(max)=null,                                                                                                                                        
	@SeatNumbers VARCHAR(max)=null,                                    
	@IsPkgTrans INT,                                    
	@PackageId INT,                                    
	@IsFamilyPkg INT,                                     
	@IsChild INT,                                     
	@IsSRCitizen INT,                            
	@OfferId bigint=0                                     
)                                    
AS                                    
BEGIN                                  
DECLARE  @ChildQty INT, @SrCitizenQty INT , @TicketTypeId INT                                             
DECLARE @MatchSeatTicketDetails TABLE(Id INT IDENTITY(1,1), MatchSeatTicketId BIGINT)                                     
DECLARE @IsSeatAllocated INT, @RemainingTicketCount INT, @BlockId VARCHAR(100), @BlockedTickets INT, @IsValidTrans BIT , @SeatCount INT,@SeatCounter INT =1                                     
SET  @IsSeatAllocated = (SELECT Isnull(IsSeatSelection,0) FROM EventTicketAttributes WITH(NOLOCK) WHERE ID = @VMCC_Id And IsEnabled = 1);                                      
SET @RemainingTicketCount = (SELECT RemainingTicketForSale FROM EventTicketAttributes WITH(NOLOCK) WHERE ID = @VMCC_Id And IsEnabled = 1)                                          
                      
                           
IF(@IsFamilyPkg=1)                          
BEGIN                          
 SET @Quantity = 4                          
END                                 
---------------                                     
SET @ChildQty  = (SELECT ISNULL(ChildQty,0) FROM EventTicketAttributes WITH(NOLOCK) WHERE Id = @VMCC_Id )                                                    
SET @SrCitizenQty  = (SELECT Isnull(SRCitizenQTY,0) FROM EventTicketAttributes WITH(NOLOCK) WHERE Id = @VMCC_Id )                                     
                                    
----------------                                      
                                      
IF (@RemainingTicketCount >= @Quantity)                                      
BEGIN                                    
                                    
 IF (@IsSeatAllocated = 1 AND @TicNumIds IS NOT NULL)                                      
 BEGIN      
  INSERT INTO @MatchSeatTicketDetails    
  SELECT CONVERT(BIGINT,keyword) from SplitString(@TicNumIds,',')     
                                  
  --UPDATE ITKTSEVNT_TicketNumberDetails SET Status = 2, BlockId = @BlockId, TicketType = 1, BlockDate = GETDATE()                                      
  --WHERE TicNumId IN (SELECT CONVERT(BIGINT,keyword) from SplitString(@TicNumIds,',')) AND Status <> 2                                      
                                      
  SELECT @BlockedTickets = COUNT(*) FROM MatchSeatTicketDetails WITH(NOLOCK)                                       
  WHERE Id IN (SELECT CONVERT(BIGINT,keyword) from SplitString(@TicNumIds,',')) --AND SeatStatusId = 5     
                                      
                                        
IF(@BlockedTickets >= @Quantity)                                      
BEGIN                                         
IF(@IsChild = 1)                                                     
BEGIN                                       
	IF(@ChildQty >= @Quantity)                                    
	BEGIN                                                   
		UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @Quantity, 
		ChildQty = ChildQty - @Quantity  where Id = @VMCC_Id                                   
		SET @IsValidTrans = 1                 
		SET @TicketTypeId=2
	END                                  
END                              
ELSE IF(@IsSrCitizen = 1)                                                           
BEGIN                                       
	If(@SrCitizenQty >= @Quantity)                                  
	BEGIN    
	UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @Quantity, 
	SRCitizenQTY = SRCitizenQTY - @Quantity  where Id = @VMCC_Id                                    
	SET @IsValidTrans = 1             
	SET @TicketTypeId=3            
END                                  
END                              
ELSE                                  
BEGIN                                                  
	UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @Quantity WHERE Id = @VMCC_Id                                        
	SET @IsValidTrans = 1               
	SET @TicketTypeId=1
END                                  
END                                 
ELSE                                      
BEGIN                                         
	SET @IsValidTrans = 0                                      
END                                         
END                                   
ELSE                                      
BEGIN                                    
--------------------- For Child and Sr Citizen                                  
	IF(@IsChild = 1)                                      
	BEGIN                                       
	IF(@ChildQty >= @Quantity)                                    
	BEGIN                                                      
		UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @Quantity, 
		ChildQty = ChildQty - @Quantity  where Id = @VMCC_Id                                         
		SET @IsValidTrans = 1                 
		SET @TicketTypeId=2
	END                                  
	END                                                                   
	ELSE IF(@IsSrCitizen = 1)                                                                        
	BEGIN                                       
		IF(@SrCitizenQty >= @Quantity)                                    
		BEGIN                                                       
			UPDATE EventTicketAttributes set RemainingTicketForSale = RemainingTicketForSale - @Quantity, 
			SRCitizenQTY = SRCitizenQTY - @Quantity  where Id = @VMCC_Id                                                                     
			SET @IsValidTrans = 1                                   
			SET @TicketTypeId=3
		END                                  
	END                                    
ELSE                                  
BEGIN                                                                                                
	UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @Quantity where Id = @VMCC_Id                                        
	SET @IsValidTrans = 1            
	SET @TicketTypeId=1                              
END                                      
END                                   
                          
END                                      
ELSE                                      
BEGIN                                      
 SET @IsValidTrans = 0                                      
END                        
                    
                           
                                      
IF(@IsValidTrans = 1)                                      
BEGIN      
DECLARE @TransactionDetailsId BIGINT
INSERT INTO TransactionDetails(TransactionId, EventTicketAttributeId, TotalTickets,CreatedUtc, 
PricePerTicket,TicketTypeId, ConvenienceCharges, ServiceCharge, DeliveryCharges, DiscountAmount,
CreatedBy,IsSeasonPackage)
VALUES (@TransId, @VMCC_Id, @Quantity,GETUTCDATE(),@PricePerTicket,@TicketTypeId, @ConvenienceCharge, 
@ServiceTax, @DeliveryCharge, @DiscountAmount,NEWID(),@IsPkgTrans)
SET @TransactionDetailsId = SCOPE_IDENTITY()    
    
 SELECT @SeatCount= COUNT(*) FROM @MatchSeatTicketDetails    
 IF(@SeatCount>0)    
 BEGIN    
-- WHILE(@SeatCounter<=@SeatCount)    
--BEGIN    
	 INSERT INTO TransactionSeatDetails(TransactionDetailId,MatchSeatTicketDetailId,CreatedUtc,CreatedBy)    
	 SELECT @TransactionDetailsId, MatchSeatTicketId,GETUTCDATE(),NEWID() FROM @MatchSeatTicketDetails     
--END    
 END

 IF(@IsFamilyPkg=1)                          
 BEGIN                          
  SET @Quantity = 1                          
 END                          
                         
                                      
UPDATE Transactions SET                                      
 DiscountAmount = ISNULL(DiscountAmount,0) + @DiscountAmount,                                      
 ConvenienceCharges = ISNULL(ConvenienceCharges,0) + @ConvenienceCharge,                                      
 ServiceCharge = ISNULL(ServiceCharge,0) + @ServiceTax,                                      
 DeliveryCharges = ISNULL(DeliveryCharges,0) + @DeliveryCharge,                                      
 NetTicketAmount = ISNULL(NetTicketAmount,0) + @PricePerTicket * @Quantity,                                      
 GrossTicketAmount = ISNULL(GrossTicketAmount,0) + ((@PricePerTicket * @Quantity) + @ConvenienceCharge + @ServiceTax + @DeliveryCharge) - @DiscountAmount,          
 TotalTickets=ISNULL(TotalTickets,0)+ @Quantity           
 WHERE ID = @TransId               
                                      
 SELECT Id AS EventTransId,GrossTicketAmount as TotalTicAmt FROM Transactions WITH(NOLOCK) WHERE ID = @TransId                                  
END                               
ELSE                                      
BEGIN                                      
 SELECT 0, 0                                      
END                                      
END 


  