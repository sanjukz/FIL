
CREATE PROC spBoxOfficeTransactionReport                   
(                          
--DECLARE                           
 @EventAltId UniqueIdentifier =null ,                           
 @UserAltId UniqueIdentifier = 'D0CE577B-5394-432B-9571-05303B190996',                           
 @EventDetailId Bigint = 0,                           
 @FromDate Datetime = NULL,                           
 @ToDate Datetime = NULL                          
)                          
AS                          
BEGIN                          
--Events Start                          
 SELECT * INTO #Events                           
 FROM Events WITH (NOLOCK)                           
 WHERE AltId = CASE WHEN @EventAltId ='00000000-0000-0000-0000-000000000000' THEN AltId WHEN @EventAltId=NULL THEN AltId ELSE  @EventAltId END                      
--Events End                          
                           
--Users Starts      
 DECLARE @UserTypeId INT=0;     
    
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
 FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)                          
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
                          
--TicketCategories Starts                          
 SELECT * INTO #TicketCategories                          
 FROM TicketCategories WITH(NOLOCK) WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)                          
--TicketCategories End                          
                          
--EventTicketAttributes Starts                          
 SELECT * INTO #EventTicketAttributes                          
 FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId IN (SELECT Id FROM #EventTicketDetails)                          
--EventTicketAttributes End                          
                          
--TicketFeeDetails Starts                          
 SELECT * INTO #TicketFeeDetails                          
 FROM TicketFeeDetails WITH(NOLOCK) WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes)                          
--TicketFeeDetails End                          
  
    
SELECT @UserTypeId=UserType FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE UserId In(select Id FROM #LoginUser)                            
--TransactionDetails Starts    
    
CREATE TABLE #TransactionDetails(Id bigint,TransactionId bigint,EventTicketAttributeId bigint,TotalTickets smallint,PricePerTicket decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,TicketTypeId smallint,CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,IsSeasonPackage int)      
CREATE TABLE #TransactionDetails1(Id bigint,TransactionId bigint,EventTicketAttributeId bigint,TotalTickets smallint,PricePerTicket decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,TicketTypeId smallint,CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,IsSeasonPackage int)      
CREATE TABLE #TransactionDetails2(Id bigint,TransactionId bigint,EventTicketAttributeId bigint,TotalTickets smallint,PricePerTicket decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,TicketTypeId smallint,CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,IsSeasonPackage int)      
CREATE TABLE #TransactionDetails3(Id bigint,TransactionId bigint,EventTicketAttributeId bigint,TotalTickets smallint,PricePerTicket decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,TicketTypeId smallint,CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,IsSeasonPackage int)      
   
 IF(@UserTypeId=3)    
BEGIN                      
 INSERT INTO #TransactionDetails1                          
 SELECT td.* FROM TransactionDetails td WITH(NOLOCK)                       
 INNER JOIN Transactions t WITH(NOLOCK) on t.id = td.TransactionId                           
 WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes) AND t.TransactionStatusId = 8  AND ChannelId=4                         
 AND T.CreatedUtc >= CASE WHEN @FromDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                          
 AND T.CreatedUtc <= CASE WHEN @ToDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END      
 AND T.CreatedBy IN(SELECT AltId FROM  Users WITH(NOLOCK) WHERE ID IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId IN(select Id FROM #Log
inUser))))     
   
 END   
 ELSE IF(@UserTypeId=2)    
BEGIN         
  INSERT INTO #TransactionDetails2                          
 SELECT td.* FROM TransactionDetails td WITH(NOLOCK)                       
 INNER JOIN Transactions t WITH(NOLOCK) on t.id = td.TransactionId                           
 WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes) AND t.TransactionStatusId = 8  AND ChannelId=4                         
 AND T.CreatedUtc >= CASE WHEN @FromDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                          
 AND T.CreatedUtc <= CASE WHEN @ToDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END    
 AND T.CreatedBy IN(SELECT AltId FROM  Users WITH(NOLOCK) WHERE ID IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId IN(select Id FROM #LoginUser)))     
  
END  
ELSE  
BEGIN              
   
 INSERT INTO #TransactionDetails3                          
 SELECT td.* FROM TransactionDetails td WITH(NOLOCK)        
 INNER JOIN Transactions t WITH(NOLOCK) on t.id = td.TransactionId                           
 WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes) AND t.TransactionStatusId = 8  AND ChannelId=4                         
 AND T.CreatedUtc >= CASE WHEN @FromDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                          
 AND T.CreatedUtc <= CASE WHEN @ToDate IS NULL THEN T.CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END   
 AND T.CreatedBy IN(select altId FROM #LoginUser)      
   
 END                          
  
  
INSERT INTO #TransactionDetails        
SELECT * FROM #TransactionDetails1        
UNION        
SELECT * FROM #TransactionDetails2        
UNION        
SELECT * FROM #TransactionDetails3  
  
--TransactionDetails End                          
                      
       
       
           
--Transactions Starts           
  
    
CREATE TABLE #Transactions(Id bigint,ChannelId smallint,CurrencyId int,TotalTickets smallint,GrossTicketAmount decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,NetTicketAmount decimal,DiscountCode nvarchar(40),TransactionStatusId smallint,IPDetailId int,IsEmailSend bit,IsSmsSend bit,OTP int,IsOTPVerified bit,FirstName nvarchar(1000),LastName nvarchar(500),PhoneCode nvarchar(20),PhoneNumber nvarchar(40),EmailId nvarchar(256),CountryName nvarchar(200),CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,AltId uniqueidentifier,ReportExportStatus smallint)      
CREATE TABLE #Transactions1(Id bigint,ChannelId smallint,CurrencyId int,TotalTickets smallint,GrossTicketAmount decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,NetTicketAmount decimal,DiscountCode nvarchar(40),TransactionStatusId smallint,IPDetailId int,IsEmailSend bit,IsSmsSend bit,OTP int,IsOTPVerified bit,FirstName nvarchar(1000),LastName nvarchar(500),PhoneCode nvarchar(20),PhoneNumber nvarchar(40),EmailId nvarchar(256),CountryName nvarchar(200),CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,AltId uniqueidentifier,ReportExportStatus smallint)      
CREATE TABLE #Transactions2(Id bigint,ChannelId smallint,CurrencyId int,TotalTickets smallint,GrossTicketAmount decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,NetTicketAmount decimal,DiscountCode nvarchar(40),TransactionStatusId smallint,IPDetailId int,IsEmailSend bit,IsSmsSend bit,OTP int,IsOTPVerified bit,FirstName nvarchar(1000),LastName nvarchar(500),PhoneCode nvarchar(20),PhoneNumber nvarchar(40),EmailId nvarchar(256),CountryName nvarchar(200),CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,AltId uniqueidentifier,ReportExportStatus smallint)      
CREATE TABLE #Transactions3(Id bigint,ChannelId smallint,CurrencyId int,TotalTickets smallint,GrossTicketAmount decimal,DeliveryCharges decimal,ConvenienceCharges decimal,ServiceCharge decimal,DiscountAmount decimal,NetTicketAmount decimal,DiscountCode nvarchar(40),TransactionStatusId smallint,IPDetailId int,IsEmailSend bit,IsSmsSend bit,OTP int,IsOTPVerified bit,FirstName nvarchar(1000),LastName nvarchar(500),PhoneCode nvarchar(20),PhoneNumber nvarchar(40),EmailId nvarchar(256),CountryName nvarchar(200),CreatedUtc datetime,UpdatedUtc datetime,CreatedBy uniqueidentifier,UpdatedBy uniqueidentifier,AltId uniqueidentifier,ReportExportStatus smallint)      
    
print @UserTypeId    
    
IF(@UserTypeId=3)    
BEGIN    
INSERT INTO #Transactions1                         
SELECT * FROM Transactions WITH(NOLOCK) WHERE Id IN (SELECT TransactionId FROM #TransactionDetails)                          
AND TransactionStatusId = 8  AND ChannelId=4                  
AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                       
AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END           
AND CreatedBy IN(SELECT AltId FROM  Users WITH(NOLOCK) WHERE ID IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE         
ParentId IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId IN(select Id FROM #LoginUser))))     
END    
ELSE IF(@UserTypeId=2)    
BEGIN    
INSERT INTO #Transactions2                         
SELECT * FROM  Transactions WITH(NOLOCK) WHERE Id IN (SELECT TransactionId FROM #TransactionDetails)                          
AND TransactionStatusId = 8  AND ChannelId=4                  
AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                          
AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END           
AND CreatedBy IN(SELECT AltId FROM  Users WITH(NOLOCK) WHERE ID IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId IN(select Id FROM #LoginUser)))     
   
END    
ELSE    
BEGIN    
INSERT INTO #Transactions3                             
SELECT * FROM  Transactions WITH(NOLOCK) WHERE Id IN (SELECT TransactionId FROM #TransactionDetails)                          
AND TransactionStatusId = 8  AND ChannelId=4                  
AND CreatedUtc >= CASE WHEN @FromDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @FromDate) END                          
AND CreatedUtc <= CASE WHEN @ToDate IS NULL THEN CreatedUtc ELSE DATEADD(MINUTE, CONVERT(INT,@timeZone), @ToDate) END           
AND CreatedBy IN(select altId FROM #LoginUser)     
END    
    
INSERT INTO #Transactions        
SELECT * FROM #Transactions1        
UNION        
SELECT * FROM #Transactions2        
UNION        
SELECT * FROM #Transactions3     
   
                   
--Transactions End                          
                          
--Users Starts                          
 SELECT * INTO #Users                          
 FROM Users WITH(NOLOCK) WHERE AltId IN (SELECT CreatedBy FROM #Transactions)                          
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
                          
--Venues Starts                          
 SELECT * INTO #Venues                          
 FROM Venues WITH(NOLOCK) WHERE Id IN (SELECT VenueId FROM #EventDetails)         
--Venues End                          
                          
--Cities Starts                          
 SELECT * INTO #Cities                          
 FROM Cities WITH(NOLOCK) WHERE Id IN (SELECT CityId FROM #Venues)                          
--Cities End                          
                          
--States Starts                          
 SELECT * INTO #States                          
 FROM States WITH(NOLOCK) WHERE Id IN (SELECT StateId FROM #Cities)                          
--States End                          
                          
--Countries Starts                          
 SELECT * INTO #Countries                          
 FROM Countries WITH(NOLOCK) WHERE Id IN (SELECT CountryId FROM #States)                          
--Countries End                          
                          
--ReportingColumnsUserMappings Starts                          
 SELECT * INTO #ReportingColumnsUserMappings                          
 FROM ReportingColumnsUserMappings WITH(NOLOCK) WHERE UserId=4910 AND IsEnabled = 1 ORDER BY SortOrder                          
--ReportingColumnsUserMappings End                
                          
--ReportingColumnsMenuMappings Starts                          
 SELECT * INTO #ReportingColumnsMenuMappings                          
 FROM ReportingColumnsMenuMappings WITH(NOLOCK) WHERE Id IN (SELECT ColumnsMenuMappingId FROM #ReportingColumnsUserMappings) AND MenuId = 1                    
--ReportingColumnsMenuMappings End                          
                          
--ReportingColumns Starts                          
 SELECT * INTO #ReportingColumns                          
 FROM ReportingColumns WITH(NOLOCK) --  WHERE Id IN (SELECT ColumnId FROM #ReportingColumnsMenuMappings)            
--ReportingColumns End                          
                 
                
SELECT DISTINCT                
T.Id,                
T.ChannelId,                 
T.CurrencyId,                
T.TotalTickets,                
T.GrossTicketAmount,                 
T.DeliveryCharges,                 
T.ConvenienceCharges,                 
T.ServiceCharge,                 
T.DiscountAmount,                 
T.NetTicketAmount,                
T.DiscountCode, T.TransactionStatusId, T.IPDetailId, T.IsEmailSend, T.IsSmsSend, T.OTP, T.IsOTPVerified,                 
ISNULL(T.FirstName, SM.SponsorName) AS FirstName,               
CASE ED.EventId  WHEN 2180 THEN T.LastName + ' (Pickup - ' + TDD.PickUpAddress + ')' ELSE T.LastName END AS LastName,              
T.PhoneCode, T.PhoneNumber, T.EmailId, T.CountryName, T.CreatedUtc, T.UpdatedUtc, T.CreatedBy, T.UpdatedBy, T.AltId, T.ReportExportStatus               
FROM #Transactions T              
INNER JOIN TransactionDetails TD WITH (NOLOCK) ON T.Id = TD.TransactionId                
INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id = TD.EventTicketAttributeId              
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId              
INNER JOIN EventDetails ED WITH (NOLOCK) ON ED.Id = ETD.EventDetailId              
LEFT OUTER JOIN TransactionDeliveryDetails TDD WITH (NOLOCK) ON TD.Id = TDD.TransactionDetailId                
LEFT OUTER JOIN CorporateTransactionDetails CTD WITH(NOLOCK) ON CTD.TransactionId = T.Id                
LEFT OUTER JOIN Sponsors SM WITH(NOLOCK) ON SM.Id =  CTD.SponsorId              
            
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
    
END 