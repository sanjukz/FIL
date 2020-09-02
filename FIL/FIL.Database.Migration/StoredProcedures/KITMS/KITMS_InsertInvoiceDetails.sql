CREATE proc [dbo].[KITMS_InsertInvoiceDetails]              
(              
	 @CorpMapId BIGINT,              
	 @TotalPrice DECIMAL(11,2),              
	 @ConvCharge DECIMAL(11,2),              
	 @ServiceCharge DECIMAL(11,2),              
	 @DiscountAmt DECIMAL(11,2),              
	 @TotalAmt DECIMAL(11,2),              
	 @TotalOrderAmt DECIMAL(11,2),              
	 @CurrencyName VARCHAR(10),              
	 @InvDate DATETIME,              
	 @DueDate DATETIME,              
	 @BankDetails NVARCHAR(MAX),              
	 @ManualInvId NVARCHAR(100),              
	 @KZFirmName NVARCHAR(500),              
	 @UserId INT,              
	 @AutoInvoiceId BIGINT,        
	 @IntermediaryBankDetails NVARCHAR(MAX)=null ,
	 @PricePerTicket DECIMAL(18,2),
	 @BankDetailId INT = 0,
	 @IntermediaryDeatilId INT = 0
)              
AS              
BEGIN              
 DECLARE @InvoiceId BIGINT              
 DECLARE @CorpOrderId BIGINT              
 DECLARE @SponsorId BIGINT              
 DECLARE @SponsorName NVARCHAR(1000), @AltId VARCHAR(500)
 SELECT @AltId = AltId FROm USers WITH(NOLOCK) WHERE Id = @UserId
 UPDATE InvoiceDetails SET IsEnabled=0 WHERE CorporateRequestDetailId=@CorpMapId              
 
 DECLARE @CurrencyId INT
 SELECT @CurrencyId =Id FROm CurrencyTypes WITH(NOLOCK) WHERE Code = @CurrencyName

 DECLARE @TotalTickets INT
 SELECT @TotalTickets =ApprovedTickets  FROm CorporateRequestDetails WITH(NOLOCK) WHERE Id = @CorpMapId
       
 INSERT INTO InvoiceDetails              
 (InvoiceNumber,TotalTickets,Price,ConvenienceCharges,ServiceCharge,ValueAddedTax,DiscountAmount,
 GrossTicketAmount,NetTicketAmount,BankDetailId,GeneratedBy,GeneratedUtc,SentToEmail,SentCcEmail,SentBccEmail,
 SentUtc,IsEnabled,CreatedUtc,CreatedBy,CorporateRequestDetailId,AttributeNumber,KzFirmName,
 IntermediaryDeatilId, DueUtc, CurrencyId)              
 VALUES(@ManualInvId,@TotalTickets,@PricePerTicket,@ConvCharge, @ServiceCharge, 0, @DiscountAmt,
 @TotalPrice,((@TotalPrice+@ConvCharge+@ServiceCharge) - @DiscountAmt),@BankDetailId,@AltId,GETUTCDATE(),
 NULL,NULL,NULL,NULL,1,GETUTCDATE(),@AltId,@CorpMapId, 
 @AutoInvoiceId, @KZFirmName, @IntermediaryDeatilId ,@DueDate, @CurrencyId)            

 SET @InvoiceId=SCOPE_IDENTITY()
              
 SELECT @CorpOrderId=COM.Id, @SponsorName=SponsorName+' - '+ FirstName+' '+ LastName              
 FROM CorporateRequestDetails CODM WITH(NOLOCK)               
 INNER JOIN CorporateRequests COM WITH(NOLOCK) ON COM.Id = CODM.CorporateRequestId              
 WHERE CODM.Id=@CorpMapId              
              
 SELECT @SponsorId=Id FROM Sponsors WITH(NOLOCK) WHERE SponsorName=@SponsorName AND IsEnabled=1              
    
 DECLARE @venueId Bigint          
 select @venueId=VenueId from EventDetails WHERE 
 Id IN (SELECT D.Id from CorporateRequestDetails A WITH(NOLOCK) 
 INNER JOIN EventTicketAttributes B WITH(NOLOCK) On A.EventTicketAttributeId = B.Id
 INNER JOIN EventTicketDetails C WITH(NOLOCK) On B.EventTicketDetailId = C.Id
 INNER JOIN EventDetails D WITH(NOLOCK) On C.EventDetailId = D.Id
 where A.Id=@CorpMapId)          
 
 IF EXISTS(SELECT Id FROM ValueAddedTaxDetails WITH(NOLOCK) WHERE VenueId=@venueId AND IsEnabled=1)              
 BEGIN              
  DECLARE @VatAmount DECIMAL(18,2), @perVatVal NVARCHAR(100)              
  SELECT @perVatVal=REPLACE(Value, '%', '') FROM ValueAddedTaxDetails WITH(NOLOCK) WHERE VenueId=@venueId AND IsEnabled=1   
  SET @VatAmount= @TotalPrice -(@TotalPrice / ((CONVERT(DECIMAL(18,2),@perVatVal)/100)+1)) 
  UPDATE InvoiceDetails SET ValueAddedTax=@VatAmount WHERE Id=@InvoiceId                
 END              
       
 SELECT @CorpOrderId AS Result              
              
END 




