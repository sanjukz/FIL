CREATE PROC spScanningCount_Test    
AS                      
BEGIN          
CREATE TABLE #BarcodeLogTable(RowNum INT, EventTicketDetailId BIGINT,BarcodeNumber Nvarchar(200),CreatedUtc DateTime)  
INSERT INTO #BarcodeLogTable  
Select ROW_NUMBER() OVER(PARTITION BY BarcodeNumber ORDER BY BarcodeNumber),  
 EventTicketDetailId,BarcodeNumber,bsl.CreatedUtc From BarcodeScanLogs bsl WITH (NOLOCK)  
INNER JOIN EventTicketDetails etd WITH (NOLOCK) ON etd.Id=bsl.EventTicketDetailId  
where CONVERT(DATE,DATEADD(MI,600,bsl.CreatedUtc)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))   
AND etd.EventDetailId In(23860,182362,182363,23855,23856,23857,23858,23859,182868)  
Group By EventTicketDetailId,BarcodeNumber,bsl.CreatedUtc  
  
DELETE FROM #BarcodeLogTable WHERE EventTicketDetailId = 98811 AND RowNum > 2  
DELETE FROM #BarcodeLogTable WHERE EventTicketDetailId <> 98811 AND BarcodeNumber LIKE '2209%' AND BarcodeNumber NOT IN ('0078151kw1q6gi3de5t','00781527kromy9ct9b8','0081983ashrt2lpyqdn','00819843a00mjtpwyxv') AND RowNum > 1  
  
Select Distinct ROW_NUMBER() OVER(PARTITION BY mstd.BarcodeNumber, CONVERT(DATE, DATEADD(MI,600,bcl.CreatedUtc))  ORDER BY mstd.BarcodeNumber) Rownum, mstd.EventTicketDetailId,mstd.BarcodeNumber,  
1 As EntryCount,  
EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,TransactionId,TicketTypeId,mstd.EntryCountAllowed INTO #ScanningDetails1  
FROM #BarcodeLogTable bcl WITH (NOLOCK)  
INNER JOIN MatchSeatTicketDetails mstd WITH (NOLOCK) ON bcl.EventTicketDetailId=mstd.EventTicketDetailId AND mstd.BarcodeNumber=bcl.BarcodeNumber  
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id = mstd.EventTicketDetailId  
INNER JOIN EventDetails ED WITH (NOLOCK) ON ED.Id = ETD.EventDetailId  
INNER JOIN Transactions t WITH (NOLOCK) ON t.id = mstd.TransactionId  
where t.TransactionStatusId = 8  
AND mstd.EventTicketDetailId IS NOT NULL  
AND mstd.EntryStatus=1  
AND isnull(mstd.EntryCount, 0) <> 0  
AND mstd.EntryDateTime is not null  
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))   
  
Select Distinct ROW_NUMBER() OVER(PARTITION BY obd.BarcodeNumber ORDER BY obd.BarcodeNumber) Rownum, obd.EventTicketDetailId,obd.BarcodeNumber,  
EntryCount, EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,'' AS TransactionId,TicketTypeId, obd.EntryCountAllowed INTO #ScanningDetails2  
FROM #BarcodeLogTable bcl WITH (NOLOCK)
INNER JOIN OffLineBarcodeDetails obd WITH (NOLOCK) ON bcl.EventTicketDetailId=obd.EventTicketDetailId AND obd.BarcodeNumber=bcl.BarcodeNumber  
where obd.EventTicketDetailId IS NOT NULL  
AND obd.EventTicketDetailId IN(451722,451715)  
AND obd.EntryStatus=1  
AND isnull(obd.EntryCount, 0) <> 0  
AND obd.EntryDateTime is not null  
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))   
  
Select Distinct ROW_NUMBER() OVER(PARTITION BY obd.BarcodeNumber, CONVERT(DATE, DATEADD(MI,600,bcl.CreatedUtc)) ORDER BY obd.BarcodeNumber) Rownum, obd.EventTicketDetailId,obd.BarcodeNumber,  
1 AS EntryCount,  
EntryStatus,CONVERT(DATETIME,DATEADD(MI,600,bcl.CreatedUtc)) As EntryDateTime,EntryGateName,'' AS TransactionId,TicketTypeId, obd.EntryCountAllowed INTO #ScanningDetails3  
FROM #BarcodeLogTable bcl WITH (NOLOCK)  
INNER JOIN OffLineBarcodeDetails obd WITH (NOLOCK) ON bcl.EventTicketDetailId=obd.EventTicketDetailId AND obd.BarcodeNumber=bcl.BarcodeNumber  
where obd.EventTicketDetailId IS NOT NULL  
AND  obd.EventTicketDetailId NOT IN(451722,451715)  
AND obd.EntryStatus=1 AND isnull(obd.EntryCount, 0) > 0  
AND obd.EntryDateTime is not null  
AND bcl.RowNum <= obd.EntryCountAllowed  
AND CONVERT(DATE,DATEADD(MI,600,bcl.CreatedUtc)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))   
  
--SELECT * FROM #ScanningDetails1 where TransactionId=210088339  
  
DELETE FROM #ScanningDetails1 WHERE Rownum > 1  
DELETE FROM #ScanningDetails3 WHERE Rownum > 1  
  
CREATE TABLE #Matchseatticketdetails(Rownum int, EventTicketDetailId bigint,BarcodeNumber nvarchar(100),EntryCount int,EntryStatus bit,EntryDateTime datetime,EntryGateName nvarchar(200), TransactionId bigint, TicketTypeId smallint, EntryCountAllowed int) 
  
   
INSERT INTO #Matchseatticketdetails  
SELECT * FROM #ScanningDetails1   
UNION  
SELECT * FROM #ScanningDetails2  
UNION  
SELECT * FROM #ScanningDetails3  
  
--Select TransactionId, Ch.Channels, T.EmailId, C.Name AS [TicketType],BarcodeNumber,  
--1 AS EntryCount,CONVERT(DATE,EntryDateTime) As EntryDate ,CONVERT(time,EntryDateTime) as EntryTime,EntryGateName From   #Matchseatticketdetails A  
--Inner Join EventTicketDetails ED With (NoLOCK) On A.EventTicketDetailId=ED.ID  
--INNER JOIN TicketCategories C On ED.TicketCategoryId=C.Id  
--LEFT OUTER JOIN Transactions T ON T.Id = TransactionId  
--LEFT OUTER JOIN Channels Ch ON Ch.Id = T.ChannelId  
--Order by EntryDateTime  
  
 SELECT ISNULL(EntryGateName,1) AS EntryGateName, Count(*) AS EntryCount                                                                                    
 FROM #Matchseatticketdetails                                                                                                 
 GROUP BY  ISNULL(EntryGateName,1)  ORDER BY Count(*) DESC   
  
 END   
  