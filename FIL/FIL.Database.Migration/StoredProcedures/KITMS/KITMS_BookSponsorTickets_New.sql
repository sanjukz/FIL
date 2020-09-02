CREATE PROCEDURE [dbo].[KITMS_BookSponsorTickets_New]      
(                                                                                                    
	@VMCC_Id1 BIGINT,
	@NoOfTic1 INT,
	@TransId bigint = -1,
	@Othercharge decimal(18,2) = 0,
	@DiscountAmt decimal(18,2)= 0,
	@TotalCharge decimal(18,2)= 0,
	@TotalTicAmt decimal(18,2)=0,
	@ConvenienceCharge decimal(18,2)=0,
	@ServiceTax decimal(18,2)=0,
	@Tickettype INT =1,
	@DileveryType INT,
	@UserId bigint = null,
	@VenuePickupId int,
	@PricePerTic decimal(18,2)=0,
	@CardNumber Varchar(100)=null,
	@NameOnCard Varchar(100)=null,
	@CardType int=null,
	@CardExpire Varchar(10)=null,
	@ClientIP Varchar(50)=null,
	@PayType varchar(500) = null,
	@BookStatus int = 1,
	@BoughtStatus int = 0,
	@PayConfNumber Varchar(1000)= null,
	@ReceiptNo Varchar(1000) = null,
	@ErrorMessage Varchar(500) = null,
	@SmsSend int = 0,
	@EmailSend int = 0,
	@CurrencyType varchar(20),
	--Parameter use in case of seat selection
	@TicNumIds TEXT=null,
	@SeatNumbers TEXT=null,
	@KM_Id bigint,
	@TypeOfTransaction varchar(55),
	@Quantity bigint=null,
	@SubSponsorId bigint=null,
	@PaymentMethod NVARCHAR(200)
	--end trans input
)                                                             
AS
 BEGIN
 BEGIN TRANSACTION 
 DECLARE @AltId NVARCHAR(500),@SponsorId BIGINT, @CurrencyId INT, @TransactionDetailId BIGINT = 0
 SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE Id =@UserId
 SELECT @CurrencyId = Id From CurrencyTypes WITH(NOLOCK) WHERE Code=@CurrencyType
 
 DECLARE @totalTransactedTickets INT, @AllocatedTickets INT, @RemainingTickets INT
 SELECT @SponsorID=SponsorId,@AllocatedTickets = AllocatedTickets,@RemainingTickets = RemainingTickets FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_Id

 DECLARE @FirstName NVARCHAR(1000), @PhoneCode NVARCHAR(100), @PhoneNumber NVARCHAR(500),  @EmailId NVARCHAR(500)
 SELECT @FirstName = SponsorName,@PhoneCode =PhoneCode, @PhoneNumber = PhoneNumber,
 @EmailId = Email FROM Sponsors WITH(NOLOCK) WHERE Id = @SponsorID

SET @totalTransactedTickets = (ISNULL((SELECT SUM(TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1                                     
AND STA.EventTicketAttributeId = @VMCC_Id1  AND STA.TransactingOptionId IN(4,5) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.EventTicketAttributeId = @VMCC_Id1 AND T.TransactionStatusId<>8)),0))

IF(@totalTransactedTickets+@NoOfTic1<=@AllocatedTickets)
BEGIN
 IF @TransId = -1                                                                                                     
 BEGIN
	 
	 SELECT @SponsorId = SponsorId FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE Id= @KM_Id  
	 INSERT INTO Transactions(ChannelId, CurrencyId, TotalTickets, GrossTicketAmount, DeliveryCharges, ConvenienceCharges, ServiceCharge,
	 DiscountAmount, NetTicketAmount, TransactionStatusId, CreatedUtc, CreatedBy)
	 VALUES(8,@CurrencyId,@NoOfTic1,@TotalCharge,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,@TotalTicAmt,0,GETDATE(),@AltId)
	 SET @TransId = SCOPE_IDENTITY()

	UPDATE Transactions SET FirstName =@FirstName , LastName='' , 
	PhoneCode=@PhoneCode, PhonenUmber=@PhoneNumber, EmailId=@EmailId WHERE Id =@TransId

	 DECLARE @ActionTypeId INT
	 SET @ActionTypeId  = CASE WHEN @TypeOfTransaction='Paid' THEN 1 ELSE 2 END
	 INSERT INTO CorporateTransactionDetails(AltId,SponsorId,TransactionId,EventTicketAttributeId,TotalTickets,Price,TransactingOptionId,IsEnabled,CreatedUtc,CreatedBy)
	 VALUES(NewID(),@SponsorId,@TransId,@VMCC_Id1,@NoOfTic1,@PricePerTic,@ActionTypeId,0,GETDATE(),@AltId)

	 INSERT INTO TransactionDetails(TransactionId,EventTicketAttributeId,TotalTickets,PricePerTicket,DeliveryCharges,ConvenienceCharges,ServiceCharge,
	 DiscountAmount,TicketTypeId,CreatedUtc,CreatedBy)
	 VALUES(@TransId,@VMCC_Id1,@NoOfTic1,@PricePerTic,0,@ConvenienceCharge,@ServiceTax,@DiscountAmt,1,GETDATE(),@AltId)
	 SET @TransactionDetailId = SCOPE_IDENTITY()
	 UPDATE CorporateTicketAllocationDetails SET RemainingTickets = RemainingTickets - @NoOfTic1, UpdatedUtc= GETDATE(), UpdatedBy = @AltId WHERE Id = @KM_Id
	 INSERT INTO TransactionPaymentDetails(TransactionId,PaymentOptionId,PaymentGatewayId,UserCardDetailId,RequestType,Amount,PayConfNumber,PaymentDetail,
	 CreatedUtc,CreatedBy)
	 VALUES(@TransId,NULL,NULL,NULL, @TypeOfTransaction, @TotalTicAmt, @PaymentMethod, @PayConfNumber,GETDATE(),@AltId)
	 SET @TicNumIds= @SeatNumbers
	 IF @TicNumIds IS NOT NULL                                                        
	 BEGIN
	   UPDATE MatchSeatTicketDetails SET TransactionId =@TransId, SeatStatusId =2, SponsorId = @SponsorId,UpdatedUtc= GETDATE(),
	   UpdatedBy = @AltId  WHERE 
	   MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))
	   INSERT INTO TransactionSeatDetails
	   SELECT @TransactionDetailId, Id, GETUTCDATE(), NULL, @AltId,NULL FROM
	   MatchSeatTicketDetails WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))
	   UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT * FROM dbo.SplitString(@TicNumIds,','))
	 END
 END
 END
IF @@ERROR <> 0                                                                                                    
BEGIN                                                                                                    
  SELECT -1 AS Result                                                                                       
  ROLLBACK TRANSACTION                                                                                                    
   END                                                                    
ELSE                                                                                                    
   BEGIN                                                                                                    
  SELECT @TransId As TransId
  COMMIT TRANSACTION                                                                                                    
END                                                                               
END
