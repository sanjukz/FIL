CREATE PROC spReportTransactionData --'2C411AC1-AA33-43E1-BA95-221684EB04F4','D0CE577B-5394-432B-9571-05303B190996',0,'2018-01-01 12:00:00','2050-12-31 12:00:00', 2, 5000
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

--Events Start        
 SELECT * INTO #Events         
 FROM Events WITH (NOLOCK) WHERE AltId = @EventAltId

 CREATE CLUSTERED INDEX Index_Events_Id ON #Events(Id)
--Events End        
         
--Users Starts        
 SELECT * INTO #LoginUser        
 FROM Users WITH (NOLOCK) WHERE AltId = @UserAltId
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

 CREATE CLUSTERED INDEX Index_EventDetails_Id ON #EventDetails(Id)
--EventDetails End        
        
--EventTicketDetails Starts        
 SELECT * INTO #EventTicketDetails        
 FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)

 CREATE CLUSTERED INDEX Index_EventTicketDetails_Id ON #EventTicketDetails(Id)
--EventTicketDetails End        
        
--EventAttributes Starts        
 SELECT * INTO #EventAttributes        
 FROM EventAttributes WITH(NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)
 DECLARE @timeZone nvarchar(20)        
 SELECT @timeZone = ISNULL(TimeZone, '0') FROM #EventAttributes 
 IF(@timeZone IS NULL)  
 BEGIN  
   SET @timeZone='0'  
 END

--EventAttributes End        
      
--EventTicketAttributes Starts        
 SELECT * INTO #EventTicketAttributes        
 FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId IN (SELECT Id FROM #EventTicketDetails)
 
 CREATE CLUSTERED INDEX Index_EventTicketAttributes_Id ON #EventTicketAttributes(Id)        
--EventTicketAttributes End

--TransactionDetails Starts        
 SELECT td.* INTO #TransactionDetails       
 FROM TransactionDetails td WITH(NOLOCK)         
 INNER JOIN Transactions t WITH(NOLOCK) on t.id = td.TransactionId         
 WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes) AND t.TransactionStatusId = 8        
 AND T.CreatedUtc >= CASE WHEN @FromDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END        
 AND T.CreatedUtc <= CASE WHEN @ToDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END
 Order By td.TransactionId DESC
 
 CREATE CLUSTERED INDEX Index_TransactionDetails_Id ON #TransactionDetails(Id)                
--TransactionDetails End        

 SELECT COUNT(*) AS NoOfRecords INTO #Pagination
 FROM #TransactionDetails

 DELETE FROM #TransactionDetails WHERE Id NOT IN(SELECT Id FROM #TransactionDetails ORDER BY TransactionId DESC 
 OFFSET CASE WHEN @PageNumber =1 THEN 0 ELSE @PageNumber * @NoRecordsPerPage END ROWS FETCH NEXT @NoRecordsPerPage ROWS ONLY)

--Transactions Starts        
 SELECT * INTO #Transactions        
 FROM Transactions WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT TransactionId FROM #TransactionDetails)
 
 CREATE CLUSTERED INDEX Index_Transactions_Id ON #Transactions(Id)
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
 FROM IPDetails WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT IPDetailId FROM #Transactions)
--IPDetails End        
        
--UserCardDetails Starts        
 SELECT * INTO #UserCardDetails        
 FROM UserCardDetails WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT UserCardDetailId FROM #TransactionPaymentDetails)
--UserCardDetails End        
        
--CurrencyTypes Starts        
 SELECT * INTO #CurrencyTypes        
 FROM CurrencyTypes WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT CurrencyId FROM #Transactions)
--CurrencyTypes End        

        
SELECT * FROM #Transactions        
SELECT * FROM #TransactionDetails
SELECT * FROM #TransactionDeliveryDetails        
SELECT * FROM #TransactionPaymentDetails       
SELECT * FROM #CurrencyTypes
SELECT * FROM #Users       
SELECT * FROM #UserCardDetails       
SELECT * FROM #IPDetails       
SELECT * FROM #Pagination

--DROP TABLE  #Transactions        
--DROP TABLE  #TransactionDetails        
--DROP TABLE  #TransactionDeliveryDetails        
--DROP TABLE  #TransactionPaymentDetails       
--DROP TABLE  #CurrencyTypes
--DROP TABLE  #Users       
--DROP TABLE  #UserCardDetails       
--DROP TABLE  #IPDetails       
--DROP TABLE  #Pagination
      
        
END





