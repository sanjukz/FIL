CREATE PROC spScanningReport_Test --'2C411AC1-AA33-43E1-BA95-221684EB04F4','D0CE577B-5394-432B-9571-05303B190996',0,'2018-09-25','2018-09-25'            
(                    
--DECLARE                     
@EventAltId UniqueIdentifier='2C411AC1-AA33-43E1-BA95-221684EB04F4',    
@UserAltId UniqueIdentifier='D0CE577B-5394-432B-9571-05303B190996',    
@EventDetailId Bigint=0,    
@FromDate Datetime='2018-09-25 00:00:00',          
@ToDate Datetime='2018-09-25 23:59:00'    
)                    
AS                    
BEGIN        
          
--SET @ToDate = DATEADD(MI,((24*60)-1),CONVERT(DATETIME,CONVERT(DATE,@ToDate)))            
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
                    
--EventTicketDetails Starts                    
 SELECT * INTO #EventTicketDetails                    
 FROM EventTicketDetails WITH (NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)                    
--EventTicketDetails End             
            
                      
--TicketCategories Starts                      
 SELECT * INTO #TicketCategories                      
 FROM TicketCategories WITH(NOLOCK) WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)                      
--TicketCategories End              
        
      
 Select Distinct mstd.EventTicketDetailId,mstd.BarcodeNumber,     
 CASE WHEN EntryCountAllowed >1 THEN CASE ROW_NUMBER() OVER(PARTITION BY mstd.BarcodeNumber ORDER BY mstd.BarcodeNumber) WHEN 1 THEN 1 ELSE 0 END ELSE 1 END As EntryCount,     
 EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,TransactionId,TicketTypeId INTO #ScanningDetails1    
 FROM BarcodeScanLogs bcl WITH (NOLOCK)     
 INNER JOIN MatchSeatTicketDetails mstd WITH (NOLOCK) ON bcl.EventTicketDetailId=mstd.EventTicketDetailId AND mstd.BarcodeNumber=bcl.BarcodeNumber    
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id = mstd.EventTicketDetailId    
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ED.Id = ETD.EventDetailId    
INNER JOIN Transactions t WITH (NOLOCK) ON t.id = mstd.TransactionId     
where t.TransactionStatusId = 8     
AND mstd.EventTicketDetailId IS NOT NULL     
AND mstd.EntryStatus=1     
AND isnull(mstd.EntryCount, 0) <> 0     
AND mstd.EntryDateTime is not null     
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) >= CONVERT(DATE,DATEADD(MI,600,@FromDate))     
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) <= CONVERT(DATE,DATEADD(MI,600,@ToDate))       
    
Select obd.EventTicketDetailId,obd.BarcodeNumber,     
1 As EntryCount, EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,'' AS TransactionId,TicketTypeId INTO #ScanningDetails2     
FROM BarcodeScanLogs bcl WITH (NOLOCK)     
INNER JOIN OffLineBarcodeDetails obd WITH (NOLOCK) ON bcl.EventTicketDetailId=obd.EventTicketDetailId AND obd.BarcodeNumber=bcl.BarcodeNumber     
where obd.EventTicketDetailId IS NOT NULL     
AND obd.EventTicketDetailId IN(451722,451715)     
AND obd.EntryStatus=1     
AND isnull(obd.EntryCount, 0) <> 0     
AND obd.EntryDateTime is not null    
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) >= CONVERT(DATE,DATEADD(MI,600,@FromDate))     
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) <= CONVERT(DATE,DATEADD(MI,600,@ToDate))      
    
Select Distinct obd.EventTicketDetailId,obd.BarcodeNumber,     
CASE WHEN EntryCountAllowed >1 THEN CASE ROW_NUMBER() OVER(PARTITION BY obd.BarcodeNumber ORDER BY obd.BarcodeNumber) WHEN 1 THEN 1 ELSE 0 END ELSE 1 END As EntryCount,     
EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,'' AS TransactionId,TicketTypeId INTO #ScanningDetails3     
FROM BarcodeScanLogs bcl WITH (NOLOCK)     
INNER JOIN OffLineBarcodeDetails obd WITH (NOLOCK) ON bcl.EventTicketDetailId=obd.EventTicketDetailId AND obd.BarcodeNumber=bcl.BarcodeNumber     
where obd.EventTicketDetailId IS NOT NULL     
AND  obd.EventTicketDetailId NOT IN(451722,451715)     
AND obd.EntryStatus=1 AND isnull(obd.EntryCount, 0) > 0     
AND obd.EntryDateTime is not null     
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) >= CONVERT(DATE,DATEADD(MI,600,@FromDate))     
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) <= CONVERT(DATE,DATEADD(MI,600,@ToDate))        
            
CREATE TABLE #Matchseatticketdetails(EventTicketDetailId bigint,BarcodeNumber nvarchar(100),EntryCount int,EntryStatus bit,EntryDateTime datetime,EntryGateName nvarchar(200), TransactionId bigint, TicketTypeId smallint)            
INSERT INTO #Matchseatticketdetails            
SELECT * FROM #ScanningDetails1            
UNION          
SELECT * FROM #ScanningDetails2            
UNION          
SELECT * FROM #ScanningDetails3        
            
--Transactions Starts               
 SELECT * INTO #Transactions                 
 FROM Transactions WITH (NOLOCK) WHERE Id in (SELECT TransactionId FROM #Matchseatticketdetails)                 
--Transactions End            
            
--TransactionDetails Starts                      
 SELECT * INTO #TransactionDetails                      
 FROM TransactionDetails WITH (NOLOCK) WHERE TransactionId in (SELECT ID FROM #Transactions)               
--TransactionDetails End             
            
--ReportingColumnsUserMappings Starts                      
 SELECT * INTO #ReportingColumnsUserMappings                      
 FROM ReportingColumnsUserMappings WITH (NOLOCK) WHERE UserId IN (SELECT Id FROM #LoginUser) AND IsEnabled = 1 ORDER BY SortOrder                      
--ReportingColumnsUserMappings End                      
                      
--ReportingColumnsMenuMappings Starts                      
 SELECT * INTO #ReportingColumnsMenuMappings                      
 FROM ReportingColumnsMenuMappings WITH (NOLOCK) WHERE Id IN (SELECT ColumnsMenuMappingId FROM #ReportingColumnsUserMappings) AND MenuId = 4                      
--ReportingColumnsMenuMappings End                      
                      
--ReportingColumns Starts                      
 SELECT * INTO #ReportingColumns                 
 FROM ReportingColumns WITH (NOLOCK) WHERE Id IN (SELECT ColumnId FROM #ReportingColumnsMenuMappings)                       
--ReportingColumns End             
            
SELECT * FROM #Transactions            
SELECT * FROM #TransactionDetails             
SELECT * FROM #Matchseatticketdetails      
SELECT * FROM #EventTicketDetails       
SELECT * FROM #EventDetails             
SELECT * FROM #EventAttributes            
SELECT * FROM #Events            
SELECT * FROm #TicketCategories            
SELECT * FROM #ReportingColumnsUserMappings                    
SELECT * FROM #ReportingColumnsMenuMappings                    
SELECT * FROM #ReportingColumns             
               
                     
END 