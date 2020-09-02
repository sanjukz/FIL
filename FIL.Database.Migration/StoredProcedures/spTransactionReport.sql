CREATE PROC spTransactionReport  
(  
--DECLARE   
@EventAltId UniqueIdentifier,   
@UserAltId UniqueIdentifier,   
@EventDetailId Bigint = 0,   
@FromDate Datetime = NULL,   
@ToDate Datetime = NULL  
)  
AS  
BEGIN  
--Events Start        
 SELECT * INTO #Events         
 FROM Events WITH (NOLOCK)         
 WHERE AltId = @EventAltId        
--Events End        
         
--Users Starts        
 SELECT * INTO #LoginUser        
 FROM Users WITH (NOLOCK)         
 WHERE AltId = @UserAltId        
--Users End        
        
--EventDetails Starts (Depending on Role & Assignment)        
 CREATE TABLE #EventDetails(Id bigint, Name nvarchar(100), VenueId int, StartDateTime datetime)        
 INSERT INTO #EventDetails        
 SELECT Id, Name, VenueId, StartDateTime         
 FROM EventDetails WITH (NOLOCK)         
 WHERE Id = CASE WHEN @EventDetailId = 0 THEN Id ELSE @EventDetailId END         
 AND EventId IN (SELECT Id FROM #Events)        
         
 IF((SELECT RolesId FROM #LoginUser) <> 1)        
 BEGIN          
  DELETE FROM #EventDetails        
  INSERT INTO #EventDetails        
  SELECT ed.Id, ed.Name, ed.VenueId, ed.StartDateTime FROM EventDetails ed WITH (NOLOCK)         
  INNER JOIN EventsUserMappings eum WITH (NOLOCK) ON ed.Id = eum.EventDetailId and eum.UserId = (SELECT Id FROM #LoginUser)        
  WHERE ed.Id = CASE WHEN @EventDetailId = 0 THEN ed.Id ELSE @EventDetailId END AND ed.EventId IN (SELECT Id FROM #Events)          
 END        
--EventDetails End        
        
--EventTicketDetails Starts        
 SELECT * INTO #EventTicketDetails        
 FROM EventTicketDetails WHERE EventDetailId IN (SELECT Id FROM #EventDetails)        
--EventTicketDetails End        
        
--EventAttributes Starts        
 SELECT * INTO #EventAttributes        
 FROM EventAttributes WHERE EventDetailId IN (SELECT Id FROM #EventDetails)
 
 DECLARE @timeZone nvarchar(20)        
 SELECT @timeZone = ISNULL(TimeZone, '0') FROM #EventAttributes         
--EventAttributes End        
        
--EventTicketDetails Starts        
 SELECT * INTO #TicketCategories        
 FROM TicketCategories WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)        
--EventTicketDetails End        
        
--EventTicketAttributes Starts        
 SELECT * INTO #EventTicketAttributes        
 FROM EventTicketAttributes WHERE EventTicketDetailId IN (SELECT Id FROM #EventTicketDetails)        
--EventTicketAttributes End        
        
--TicketFeeDetails Starts        
 SELECT * INTO #TicketFeeDetails        
 FROM TicketFeeDetails WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes)        
--TicketFeeDetails End        
        
--TransactionDetails Starts        
 SELECT td.* INTO #TransactionDetails        
 FROM TransactionDetails td         
 INNER JOIN Transactions t on t.id = td.TransactionId         
 WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes) AND t.TransactionStatusId = 8  
 AND t.CreatedUtc >= CASE WHEN @FromDate IS NULL THEN t.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END        
 AND t.CreatedUtc <= CASE WHEN @ToDate IS NULL THEN t.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END       
--TransactionDetails End        
        
--Transactions Starts        
       
 SELECT * INTO #Transactions        
 FROM Transactions WHERE Id IN (SELECT TransactionId FROM #TransactionDetails)        
 AND TransactionStatusId = 8        
 AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END        
 AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END        
--Transactions End        
        
--Users Starts        
 SELECT * INTO #Users        
 FROM Users WHERE AltId IN (SELECT CreatedBy FROM #Transactions)        
--Users End        
        
--TransactionDeliveryDetails Starts        
 SELECT * INTO #TransactionDeliveryDetails        
 FROM TransactionDeliveryDetails WHERE TransactionDetailId IN (SELECT Id FROM #TransactionDetails)        
--TransactionDeliveryDetails End        
        
--TransactionPaymentDetails Starts        
 SELECT * INTO #TransactionPaymentDetails        
 FROM TransactionPaymentDetails WHERE TransactionId IN (SELECT Id FROM #Transactions)        
--TransactionPaymentDetails End     
        
--IPDetails Starts        
 SELECT * INTO #IPDetails        
 FROM IPDetails WHERE Id IN (SELECT IPDetailId FROM #Transactions)        
--IPDetails End        
        
--UserCardDetails Starts        
 SELECT * INTO #UserCardDetails        
 FROM UserCardDetails WHERE Id IN (SELECT UserCardDetailId FROM #TransactionPaymentDetails)        
--UserCardDetails End        
        
--CurrencyTypes Starts        
 SELECT * INTO #CurrencyTypes        
 FROM CurrencyTypes WHERE Id IN (SELECT CurrencyId FROM #Transactions)        
--CurrencyTypes End        
        
--Venues Starts        
 SELECT * INTO #Venues        
 FROM Venues WHERE Id IN (SELECT VenueId FROM #EventDetails)        
--Venues End        
        
--Cities Starts        
 SELECT * INTO #Cities        
 FROM Cities WHERE Id IN (SELECT CityId FROM #Venues)        
--Cities End        
        
--States Starts        
 SELECT * INTO #States        
 FROM States WHERE Id IN (SELECT StateId FROM #Cities)        
--States End        
        
--States Starts        
 SELECT * INTO #Countries        
 FROM Countries WHERE Id IN (SELECT CountryId FROM #States)        
--States End        
        
--ReportingColumnsUserMappings Starts        
 SELECT * INTO #ReportingColumnsUserMappings        
 FROM ReportingColumnsUserMappings WHERE UserId IN (SELECT Id FROM #LoginUser) AND IsEnabled = 1 ORDER BY SortOrder        
--ReportingColumnsUserMappings End        
        
--ReportingColumnsMenuMappings Starts        
 SELECT * INTO #ReportingColumnsMenuMappings        
 FROM ReportingColumnsMenuMappings WHERE Id IN (SELECT ColumnsMenuMappingId FROM #ReportingColumnsUserMappings) AND MenuId = 1        
--ReportingColumnsMenuMappings End        
        
--ReportingColumns Starts        
 SELECT * INTO #ReportingColumns        
 FROM ReportingColumns WHERE Id IN (SELECT ColumnId FROM #ReportingColumnsMenuMappings)         
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
