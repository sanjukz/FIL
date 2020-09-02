CREATE PROCEDURE [dbo].[KITMS_BookCartByVenue_Temp]    
(                                                                                                  
  @VmccId BIGINT=312579,                                                                                                  
  @NoOfTic INT,                                                                                                                                                                                              
  @TransId BIGINT = -1,                                                                                               
  @Othercharge DECIMAL(18,2) = 0,                                                                                                  
  @DiscountAmt DECIMAL(18,2)= 0,                                                                                                  
  @TotalCharge DECIMAL(18,2)= 0,                                                                                                  
  @TotalTicAmt DECIMAL(18,2)=0,                                                                                    
  @ConvenienceCharge DECIMAL(18,2)=0,                                                                                    
  @ServiceTax DECIMAL(18,2)=0,                                                                                                  
  @Tickettype INT =1,                                                                                        
  @DileveryType INT,                                                                                
  @UserId BIGINT = null,                                         
  @VenuePickupId INT=0,                                                                                 
  @PricePerTic DECIMAL(18,2)=0,                                         
  @CardNumber VARCHAR(100)=null ,                                                                                                  
  @NameOnCard VARCHAR(100)=null,                                                                                                  
  @CardType INT=null,                                                                                                  
  @CardExpire VARCHAR(10)=null,                                                                                                  
  @ClientIP VARCHAR(50)=null,                                                                                                  
  @PayType VARCHAR(500) = null,                                                                                                  
  @BookStatus INT = 1,                                                                                                  
  @BoughtStatus INT = 0,                                                                                                  
  @PayConfNumber VARCHAR(1000)= null,                                                                                                  
  @ReceiptNo VARCHAR(1000) = null,                                                                                                  
  @ErrorMessage VARCHAR(500) = null,                                      
  @SmsSend INT = 0,                                                                                                  
  @EmailSend INT = 0,                                                    
  @CurrencyType VARCHAR(20),                    
 --Parameter use in case of seat selection              
  @TicNumIds VARCHAR(MAX)=null,                                       
  @SeatNumbers VARCHAR(MAX)=null,                            
  @KM_Id BIGINT=0,                  
  @TypeOfTransaction varchar(55),                  
  @Quantity BIGINT,            
  @SubSponsorId BIGINT=null,
  @TotlatTickets INT,
  @ConvChargePerItem DECIMAL(18,2) = 0,
  @DiscountAmtPerItem DECIMAL(18,2) = 0,
  @ServiceChargePerItem DECIMAL(18,2) = 0
 --end trans input                   
)         
AS      
BEGIN
BEGIN TRANSACTION                                                           
DECLARE @AltId NVARCHAR(500),@SponsorId BIGINT, @CurrencyId INT, @TicketCount INT
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE Id =@UserId
SELECT @CurrencyId = Id From CurrencyTypes WITH(NOLOCK) WHERE Code=@CurrencyType       
 -- START TRANS INSERT
 SELECT @SponsorId = SponsorId FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id=@KM_Id
SET @TicketCount  = (SELECT ISNULL(RemainingTickets,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id = @KM_Id)
IF (@TicketCount >= @NoOfTic)                                                                                  
BEGIN   
 IF @TransId = -1                                                                                                       
 BEGIN
	 INSERT INTO Transactions(ChannelId, CurrencyId, TotalTickets, GrossTicketAmount, DeliveryCharges, ConvenienceCharges, ServiceCharge,
	 DiscountAmount, NetTicketAmount, TransactionStatusId, CreatedUtc, CreatedBy)
	 VALUES(8,@CurrencyId,@TotlatTickets,@TotalCharge,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,@TotalTicAmt,0,GETDATE(),@AltId)
    SET @TransId = SCOPE_IDENTITY() 
	
	INSERT INTO TransactionPaymentDetails(TransactionId,PaymentOptionId,PaymentGatewayId,UserCardDetailId,RequestType,Amount,PayConfNumber,PaymentDetail,
	 CreatedUtc,CreatedBy)
	 VALUES(@TransId,NULL,NULL,NULL, @TypeOfTransaction, @TotalTicAmt, @TypeOfTransaction, @PayConfNumber,GETDATE(),@AltId)
	                                  
 END                          
 ELSE                                                          
 BEGIN                                                          
   IF @TotalTicAmt <> 0 AND @TransId <>-1
   BEGIN
		UPDATE Transactions SET
		TotalTickets = @TotlatTickets, GrossTicketAmount = @TotalCharge, DeliveryCharges = 0, ConvenienceCharges = @ConvenienceCharge, ServiceCharge=@ServiceTax,
		DiscountAmount=@DiscountAmt, NetTicketAmount=@TotalTicAmt,  UpdatedBy = @AltId, UpdatedUtc = GETDATE()
		WHERE Id= @TransId   
   END                                                       
END
DECLARE @ActionTypeId INT
SET @ActionTypeId  = CASE WHEN @TypeOfTransaction='Paid' THEN 1 ELSE 2 END
UPDATE CorporateTicketAllocationDetails SET RemainingTickets = RemainingTickets - @NoOfTic, UpdatedUtc= GETDATE(), UpdatedBy = @AltId WHERE Id = @KM_Id
INSERT INTO CorporateTransactionDetails(AltId,SponsorId,TransactionId,EventTicketAttributeId,TotalTickets,Price,TransactingOptionId,IsEnabled,CreatedUtc,CreatedBy)
VALUES(NewID(),@SponsorId,@TransId,@VmccId,@NoOfTic,@PricePerTic,@ActionTypeId,0,GETDATE(),@AltId)
                 
 -- END TRANS INSERT                         
-- INSERT INTO VMCC_MapTrans    TABLE    
 DECLARE @countVMCC INT                                                    
 SET @countVMCC = (SELECT COUNT(*) FROM TransactionDetails WITH(NOLOCK) WHERE  TransactionId = @TransId                                                    
 AND  EventTicketAttributeId = @VmccId)                                                    
 IF  @countVMCC = 0                                     
 BEGIN
	INSERT INTO TransactionDetails(TransactionId,EventTicketAttributeId,TotalTickets,PricePerTicket,DeliveryCharges,ConvenienceCharges,ServiceCharge,
	DiscountAmount,TicketTypeId,CreatedUtc,CreatedBy)
	VALUES(@TransId,@VmccId,@NoOfTic,@PricePerTic,0,@ConvChargePerItem,@ServiceChargePerItem,@DiscountAmtPerItem,1,GETDATE(),@AltId)
  -- UPDATE SEAT NUMBER FOR BOOKED TKTS                                                    
  SET @TicNumIds= @SeatNumbers
  IF @TicNumIds IS NOT NULL                                        
  BEGIN
	UPDATE MatchSeatTicketDetails SET TransactionId =@TransId, SeatStatusId =2, SponsorId = @SponsorId,UpdatedUtc= GETDATE(), UpdatedBy = @AltId  WHERE 
	MatchLayoutSectionSeatId IN (SELECT CONVERT(BIGINT,Keyword) AS SeatId FROM dbo.SplitString(@TicNumIds,','))
	UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT CONVERT(BIGINT,keyword) AS SeatId FROM dbo.SplitString(@TicNumIds,',')) 
  END                                                                                              
 END
 -- END VMCC_MapTrans                                                                                                       
END
ELSE                                                                           
BEGIN                                                          
 SET @TransId =0  -- IF NO TIC FOUND                                                                              
END                       
                                                               
   IF @@ERROR <> 0                                                                                                      
   BEGIN
   SELECT -1 AS Result                                
   ROLLBACK TRANSACTION                                                                                     
   END                                                                      
ELSE                                                                                                      
   BEGIN                                                                                                      
   SELECT @TransId AS TransId
   COMMIT TRANSACTION                                                                               
 END                                                                                 
END
