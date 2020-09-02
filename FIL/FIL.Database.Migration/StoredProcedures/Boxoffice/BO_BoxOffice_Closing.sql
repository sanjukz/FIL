CREATE PROCEDURE [dbo].[BO_BoxOffice_Closing]           
(       
	@Ticket_SRNo BIGINT,            
	@WasteTicket BIGINT,            
	@CashAmount DECIMAL(32,2),            
	@CardAmount DECIMAL(32,2),            
	@Retailerid BIGINT,            
	@CreatedDate VARCHAR(100)       
)      
            
AS            
BEGIN            
DECLARE @venueid INT           
DECLARE @Count INT            
DECLARE @TimeZone VARCHAR(100)       
      
SELECT DISTINCT @venueid= VenueId FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId and IsEnabled=1      
      
IF NOT EXISTS(SELECT * FROM BoClosingDetails WITH(NOLOCK) WHERE CONVERT(DATE,CreatedUtc) = CONVERT(DATE,@CreatedDate) AND UserId = @Retailerid  )    
SELECT @Count=COUNT(CreatedUtc) from BoClosingDetails WITH(NOLOCK) WHERE CAST(CreatedUtc AS DATE)=CAST(DATEADD(MINUTE, CONVERT(INT,@TimeZone),GETDATE()) AS DATE) AND UserId=@RetailerId        
               
IF(@Count=0)    
BEGIN       
	INSERT INTO BoClosingDetails(TicketStockStartSrNo,WasteTickets,CashAmount,CardAmount,UserId,CreatedUtc,IsEnabled,CreatedBy)       
	VALUES(@Ticket_SRNo,@WasteTicket,@CashAmount,@CardAmount,@Retailerid,@CreatedDate,1,NewId());           
        
	SELECT 'BO closing details inserted successfully ' AS [Message] 
END  
ELSE  
BEGIN 
	SELECT 'Record Not Inserted Successfully .User can enter details only once in day' AS [Message]        
        
	UPDATE BoClosingDetails SET TicketStockStartSrNo = @Ticket_SRNo, WasteTickets = @WasteTicket, CashAmount = @CashAmount, CardAmount = @CardAmount,       
	UserId = @Retailerid WHERE CONVERT(DATE,CreatedUtc) = CONVERT(DATE,@CreatedDate)       
END        
        
END   