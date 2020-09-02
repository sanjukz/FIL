CREATE PROC spTransactionReport_Test --'00000000-0000-0000-0000-000000000000','D0CE577B-5394-432B-9571-05303B190996',0,'2018/04/01','2018/09/14'        
(          
--DECLARE           
@EventAltId UniqueIdentifier = '2C411AC1-AA33-43E1-BA95-221684EB04F4' ,           
@UserAltId UniqueIdentifier = 'D0CE577B-5394-432B-9571-05303B190996',           
@EventDetailId Bigint = 0,           
@FromDate Datetime = NULL,           
@ToDate Datetime = NULL          
)          
AS          
BEGIN           
--Users Starts          
 SELECT * INTO #LoginUser          
 FROM Users WITH (NOLOCK)           
 WHERE AltId = @UserAltId          
--Users End     
  
--Transactions Starts          
 SELECT * INTO #Transactions          
 FROM Transactions WITH (NOLOCK) WHERE TransactionStatusId = 8          
 AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE CONVERT(DATE, @FromDate) END          
 AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE CONVERT(DATE, @ToDate) END          
--Transactions End   
  
--TransactionDetails Starts          
 SELECT * INTO #TransactionDetails          
 FROM TransactionDetails WITH (NOLOCK)           
 WHERE TransactionId IN (SELECT Id FROM #Transactions)          
 AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE CONVERT(DATE, @FromDate) END          
 AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE CONVERT(DATE, @ToDate) END          
--TransactionDetails End   
  
--TransactionDeliveryDetails Starts          
 SELECT * INTO #TransactionDeliveryDetails          
 FROM TransactionDeliveryDetails WITH (NOLOCK) WHERE TransactionDetailId IN (SELECT Id FROM #TransactionDetails)          
--TransactionDeliveryDetails End    
          
--TransactionPaymentDetails Starts          
 SELECT * INTO #TransactionPaymentDetails          
 FROM TransactionPaymentDetails WITH (NOLOCK) WHERE TransactionId IN (SELECT Id FROM #Transactions)          
--TransactionPaymentDetails End  
  
--CurrencyTypes Starts          
 SELECT * INTO #CurrencyTypes          
 FROM CurrencyTypes WITH (NOLOCK) WHERE Id IN (SELECT CurrencyId FROM #Transactions)          
--CurrencyTypes End    
  
--EventTicketAttributes Starts          
 SELECT * INTO #EventTicketAttributes          
 FROM EventTicketAttributes WITH (NOLOCK) WHERE ID IN (SELECT EventTicketAttributeId FROM #TransactionDetails)          
--EventTicketAttributes End  
  
--EventTicketDetails Starts          
 SELECT * INTO #EventTicketDetails          
 FROM EventTicketDetails WITH (NOLOCK) WHERE ID IN (SELECT EventTicketDetailId FROM #EventTicketAttributes)          
--EventTicketDetails End   
  
--TicketCategories Starts          
 SELECT * INTO #TicketCategories          
 FROM TicketCategories WITH (NOLOCK) WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)          
--TicketCategories End  
  
--EventDetails Starts (Depending on Role & Assignment)          
 CREATE TABLE #EventDetails(Id bigint, Name nvarchar(100), VenueId int, StartDateTime datetime)         
  INSERT INTO #EventDetails          
  SELECT ed.Id, ed.Name, ed.VenueId, ed.StartDateTime FROM EventDetails ed WITH (NOLOCK)           
  INNER JOIN EventsUserMappings eum WITH (NOLOCK) ON ed.Id = eum.EventDetailId and eum.UserId = (SELECT Id FROM #LoginUser)          
  WHERE ed.Id IN (SELECT EventDetailId FROM #EventTicketDetails)  
--EventDetails End   
  
--EventAttributes Starts          
 SELECT * INTO #EventAttributes          
 FROM EventAttributes WITH (NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)     
 DECLARE @timeZone nvarchar(20)          
 SELECT @timeZone = ISNULL(TimeZone, '0') FROM #EventAttributes   
 IF(@timeZone IS NULL)    
 BEGIN    
   SET @timeZone='0'    
 END         
--EventAttributes End   
  
--Venues Starts          
 SELECT * INTO #Venues          
 FROM Venues WITH (NOLOCK) WHERE Id IN (SELECT VenueId FROM #EventDetails)          
--Venues End          
          
--Cities Starts          
 SELECT * INTO #Cities          
 FROM Cities WITH (NOLOCK) WHERE Id IN (SELECT CityId FROM #Venues)          
--Cities End          
          
--States Starts          
 SELECT * INTO #States          
 FROM States WITH (NOLOCK) WHERE Id IN (SELECT StateId FROM #Cities)          
--States End   
  
--Countries Starts          
 SELECT * INTO #Countries          
 FROM Countries WITH (NOLOCK) WHERE Id IN (SELECT CountryId FROM #States)          
--Countries End          
          
--Events Start          
 CREATE TABLE #Events(Id bigint, AltId uniqueidentifier, Name nvarchar(100))        
 INSERT INTO #Events           
 SELECT e.Id, e.AltId, e.Name FROM Events e WITH (NOLOCK)   
 INNER JOIN EventsUserMappings eum WITH (NOLOCK) ON e.Id = eum.EventId and eum.UserId = (SELECT Id FROM #LoginUser)         
 WHERE e.Id IN (SELECT EventId FROM #EventDetails)          
--Events End   
                
--Users Starts          
 SELECT * INTO #Users          
 FROM Users WITH (NOLOCK) WHERE AltId IN (SELECT CreatedBy FROM #Transactions)          
--Users End   
  
--UserCardDetails Starts          
 SELECT * INTO #UserCardDetails          
 FROM UserCardDetails WITH (NOLOCK) WHERE Id IN (SELECT UserCardDetailId FROM #TransactionPaymentDetails)          
--UserCardDetails End   
  
--IPDetails Starts          
 SELECT * INTO #IPDetails          
 FROM IPDetails WITH (NOLOCK) WHERE Id IN (SELECT IPDetailId FROM #Transactions)          
--IPDetails End   
  
--TicketFeeDetails Starts          
 SELECT * INTO #TicketFeeDetails          
 FROM TicketFeeDetails WITH (NOLOCK) WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes)          
--TicketFeeDetails End       
          
--ReportingColumnsUserMappings Starts          
 SELECT * INTO #ReportingColumnsUserMappings          
 FROM ReportingColumnsUserMappings WITH (NOLOCK) WHERE UserId IN (SELECT Id FROM #LoginUser) AND IsEnabled = 1 ORDER BY SortOrder          
--ReportingColumnsUserMappings End          
          
--ReportingColumnsMenuMappings Starts          
 SELECT * INTO #ReportingColumnsMenuMappings          
 FROM ReportingColumnsMenuMappings WITH (NOLOCK) WHERE Id IN (SELECT ColumnsMenuMappingId FROM #ReportingColumnsUserMappings) AND MenuId = 1          
--ReportingColumnsMenuMappings End          
          
--ReportingColumns Starts          
 SELECT * INTO #ReportingColumns          
 FROM ReportingColumns WITH (NOLOCK) WHERE Id IN (SELECT ColumnId FROM #ReportingColumnsMenuMappings)           
--ReportingColumns End          
          
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
SELECT * FROM #ReportingColumnsUserMappings          
SELECT * FROM #ReportingColumnsMenuMappings          
SELECT * FROM #ReportingColumns        
        
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
  