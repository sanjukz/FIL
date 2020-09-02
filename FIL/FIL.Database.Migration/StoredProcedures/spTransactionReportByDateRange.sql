CREATE PROC spTransactionReportByDateRange --'2C411AC1-AA33-43E1-BA95-221684EB04F4','D0CE577B-5394-432B-9571-05303B190996',0,'2018-01-01 12:00:00','2050-12-31 12:00:00', 2, 50
(        
--DECLARE         
	@EventAltId UniqueIdentifier = '2C411AC1-AA33-43E1-BA95-221684EB04F4' ,         
	@UserAltId UniqueIdentifier = 'D0CE577B-5394-432B-9571-05303B190996',         
	@EventDetailId Bigint = 0,         
	@FromDate Datetime = NULL,         
	@ToDate Datetime = NULL,
	@PageNumber INT = 1,  
	@NoRecordsPerPage INT  = 100
)        
AS        
BEGIN

CREATE TABLE #TransactionDetailsTemp(
	[RowId] [int] IDENTITY(1,1),
	[Id] [bigint] NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[EventTicketAttributeId] [bigint] NOT NULL,
	[TotalTickets] [smallint] NOT NULL,
	[PricePerTicket] [decimal](19, 5) NOT NULL,
	[DeliveryCharges] [decimal](19, 5) NULL,
	[ConvenienceCharges] [decimal](19, 5) NULL,
	[ServiceCharge] [decimal](19, 5) NULL,
	[DiscountAmount] [decimal](19, 5) NULL,
	[TicketTypeId] [smallint] NULL,
	[CreatedUtc] [datetime] NOT NULL,
	[UpdatedUtc] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[IsSeasonPackage] [int] NULL
 )

--TransactionDetails Starts        
 INSERT INTO #TransactionDetailsTemp        
 SELECT td.*  FROM TransactionDetails td WITH(NOLOCK)         
 INNER JOIN Transactions t WITH(NOLOCK) on t.id = td.TransactionId         
 WHERE  t.TransactionStatusId = 8 AND t.CreatedUtc >=  @FromDate AND t.CreatedUtc <=  @ToDate
 Order By td.TransactionId DESC        

 SELECT COUNT(*) AS NoOfRecords INTO #Pagination
 FROM #TransactionDetailsTemp

 SELECT Id,TransactionId,EventTicketAttributeId,TotalTickets,PricePerTicket,DeliveryCharges,ConvenienceCharges,ServiceCharge,DiscountAmount,
 TicketTypeId,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,IsSeasonPackage INTO #TransactionDetails
 FROM #TransactionDetailsTemp 
 WHERE RowId BETWEEN ((@PageNumber *  @NoRecordsPerPage) - @NoRecordsPerPage) + 1 AND (@PageNumber *  @NoRecordsPerPage)
--TransactionDetails End  

--Transactions Starts        
 SELECT * INTO #Transactions        
 FROM Transactions WITH(NOLOCK) WHERE Id IN (SELECT TransactionId FROM #TransactionDetails)        
 AND TransactionStatusId = 8 AND CreatedUtc >= @FromDate AND CreatedUtc <=  @ToDate
--Transactions End   
       
--Users Starts        
 SELECT * INTO #Users        
 FROM Users WITH(NOLOCK) WHERE AltId IN (SELECT DISTINCT CreatedBy FROM #Transactions)        
--Users End        
        
--TransactionDeliveryDetails Starts        
 SELECT * INTO #TransactionDeliveryDetails        
 FROM TransactionDeliveryDetails WITH(NOLOCK) WHERE TransactionDetailId IN (SELECT Id FROM #TransactionDetails)        
--TransactionDeliveryDetails End        
        
--TransactionPaymentDetails Starts        
 SELECT * INTO #TransactionPaymentDetails        
 FROM TransactionPaymentDetails WITH(NOLOCK) WHERE TransactionId IN (SELECT Id FROM #Transactions)        
--TransactionPaymentDetails End     
        
--IPDetails Starts        
 SELECT * INTO #IPDetails        
 FROM IPDetails WITH(NOLOCK) WHERE Id IN (SELECT IPDetailId FROM #Transactions)        
--IPDetails End        
        
--UserCardDetails Starts        
 SELECT * INTO #UserCardDetails        
 FROM UserCardDetails WITH(NOLOCK) WHERE Id IN (SELECT UserCardDetailId FROM #TransactionPaymentDetails)        
--UserCardDetails End        
        
--CurrencyTypes Starts        
 SELECT * INTO #CurrencyTypes        
 FROM CurrencyTypes WITH(NOLOCK) WHERE Id IN (SELECT CurrencyId FROM #Transactions)        
--CurrencyTypes End   

--EventTicketAttributes Starts        
 SELECT * INTO #EventTicketAttributes        
 FROM EventTicketAttributes WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT EventTicketAttributeId FROM #TransactionDetails)        
--EventTicketAttributes End 

--EventTicketDetails Starts        
 SELECT * INTO #EventTicketDetails        
 FROM EventTicketDetails WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT EventTicketDetailId FROM #EventTicketAttributes)        
--EventTicketDetails End 

--TicketCategories Starts        
 SELECT * INTO #TicketCategories        
 FROM TicketCategories WITH(NOLOCK) WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)        
--TicketCategories End      

--EventDetails Starts (Depending on Role & Assignment)        
 CREATE TABLE #EventDetails(Id bigint, Name nvarchar(100), EventId bigint, VenueId int, StartDateTime datetime)        
 INSERT INTO #EventDetails        
 SELECT Id, Name, EventId, VenueId, StartDateTime         
 FROM EventDetails WITH (NOLOCK)         
 WHERE Id IN(SELECT DISTINCT EventDetailId FROM #EventTicketDetails)       
--EventDetails End        
        

--Events Start        
 SELECT * INTO #Events         
 FROM Events WITH (NOLOCK) WHERE Id IN(SELECT DISTINCT EventId FROM #EventDetails)   
--Events End        
         
--Users Starts        
 SELECT * INTO #LoginUser        
 FROM Users WITH (NOLOCK)  WHERE AltId = @UserAltId        
--Users End    
        
--EventAttributes Starts        
 SELECT * INTO #EventAttributes        
 FROM EventAttributes WITH(NOLOCK) WHERE EventDetailId IN (SELECT DISTINCT Id FROM #EventDetails)
--EventAttributes End        
   
        
--TicketFeeDetails Starts        
 SELECT * INTO #TicketFeeDetails        
 FROM TicketFeeDetails WITH(NOLOCK) WHERE EventTicketAttributeId IN (SELECT DISTINCT Id FROM #EventTicketAttributes)        
--TicketFeeDetails End        
     
        
--Venues Starts        
 SELECT * INTO #Venues        
 FROM Venues WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT VenueId FROM #EventDetails)        
--Venues End        
        
--Cities Starts        
 SELECT * INTO #Cities        
 FROM Cities WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT CityId FROM #Venues)        
--Cities End        
        
--States Starts        
 SELECT * INTO #States        
 FROM States WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT StateId FROM #Cities)        
--States End        
        
--Countries Starts        
 SELECT * INTO #Countries        
 FROM Countries WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT CountryId FROM #States)        
--Countries End  

SELECT * FROM #Transactions        
SELECT * FROM #TransactionDetails        
SELECT * FROM #TransactionDeliveryDetails        
SELECT * FROM #TransactionPaymentDetails       
SELECT * FROM #CurrencyTypes        
SELECT * FROM #EventTicketAttributes       
SELECT * FROM #EventTicketDetails      
SELECT * FROM #TicketCategories      
SELECT * FROM #EventDetails       
SELECT * FROM #EventAttributes        
SELECT * FROM #Venues        
SELECT * FROM #Cities 
SELECT * FROM #States        
SELECT * FROM #Countries        
SELECT * FROM #Events        
SELECT * FROM #Users       
SELECT * FROM #UserCardDetails       
SELECT * FROM #IPDetails        
SELECT * FROM #TicketFeeDetails        
SELECT * FROM #Pagination
      
--DROP TABLE #Events        
--DROP TABLE #LoginUser        
--DROP TABLE #EventDetails        
--DROP TABLE #EventTicketDetails        
--DROP TABLE #EventAttributes        
--DROP TABLE #TicketCategories        
--DROP TABLE #EventTicketAttributes        
--DROP TABLE #TicketFeeDetails        
--DROP TABLE #TransactionDetails        
--DROP TABLE #Transactions        
--DROP TABLE #Users        
--DROP TABLE #TransactionDeliveryDetails        
--DROP TABLE #TransactionPaymentDetails        
--DROP TABLE #IPDetails        
--DROP TABLE #UserCardDetails        
--DROP TABLE #CurrencyTypes        
--DROP TABLE #Venues        
--DROP TABLE #Cities        
--DROP TABLE #States        
--DROP TABLE #Countries        
--DROP TABLE #ReportingColumnsUserMappings        
--DROP TABLE #ReportingColumnsMenuMappings        
--DROP TABLE #ReportingColumns      
        
END




