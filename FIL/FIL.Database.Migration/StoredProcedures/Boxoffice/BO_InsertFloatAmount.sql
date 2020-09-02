CREATE Proc [dbo].[BO_InsertFloatAmount] --4522,100,100                
(              
	@RetailerId bigint,
	@AMTUSD decimal(32,2),
	@AMTLocal decimal(32,2)              
)          
AS            
BEGIN         
DECLARE @Count INT
DECLARE @output VARCHAR(MAX)
DECLARE @Currency VARCHAR(10)
DECLARE @venueid INT
DECLARE @TimeZone VARCHAR (100)
DECLARE @EventType INT
DECLARE @CurrencyId INT
SELECT TOP 1 @venueid= VenueId FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId and IsEnabled=1  ORDER BY Id desc
SELECT TOP 1 @EventType= EventId FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId and IsEnabled=1  ORDER BY Id desc            
     
SELECT Top 1 @TimeZone=isnull(EA.TimeZone,0) from EventAttributes EA WITH(NOLOCK)      
LEFT OUTER JOIN EventDetails ED  WITH(NOLOCK) ON EA.EventDetailId=ED.Id      
WHERE VenueId=@VenueId            
AND EventId IN (SELECT DISTINCT EventId FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId AND IsEnabled=1)
SELECT @CurrencyId = LocalCurrencyId FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId IN (SELECT Id from EventTicketDetails where EventDetailId In(select top 1 Id from EventDetails where EventId=@EventType and VenueId=@venueid))      
         
SELECT @Currency = Code FROM CurrencyTypes WITH(NOLOCK) WHERE Id IN (@CurrencyId)          
               
SELECT @Count=count(Createddate) FROM BO_FloatAmount WITH(NOLOCK) WHERE 
CAST(Createddate AS DATE)=CAST(DATEADD(MINUTE, CONVERT(INT,@TimeZone),GETUTCDATE()) AS date) AND 
Retailer_id=@RetailerId                            
               
IF(@Count=0)  
BEGIN          
INSERT INTO [BO_FloatAmount](Retailer_id,[AmtIn(USD)],[AmtIn(Local)],Createddate,VenueId, LocalCurrencyId)values(@RetailerId,@AMTUSD,@AMTLocal, DATEADD(MINUTE, CONVERT(INT,@TimeZone),GETUTCDATE()),@VenueId,@CurrencyId)
SELECT 'Float amount for today is – USD '+ CONVERT(VARCHAR(20),@AMTUSD)+' – '+@Currency+' '+CONVERT(VARCHAR(20),@AMTLocal)+'' as [Message]                                       
END            
ELSE           
BEGIN         
	SELECT 'You can enter details only one time in a day. Float amount for today is – USD '+CONVERT(VARCHAR(20),[AmtIn(USD)])+' – '+@Currency+' '+CONVERT(VARCHAR(20),[AmtIn(Local)])+''as [Message]   from BO_FloatAmount  WITH(NOLOCK) where Cast(Createddate as date)=Cast(DATEADD(MINUTE, CONVERT(INT,@TimeZone),GETUTCDATE()) as date)  
	AND Retailer_id=@RetailerId
END            
END