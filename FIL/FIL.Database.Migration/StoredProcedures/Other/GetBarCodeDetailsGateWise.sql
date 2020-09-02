CREATE PROC [dbo].[GetBarCodeDetailsGateWise]                                                    
(                                                    
	 @Barcode VARCHAR(500),                                                    
	 @Gate VARCHAR(500),                                                    
	 @DeviceId VARCHAR(20)                                                    
)                                                    
AS                                                    
BEGIN                                                    
                                                    
DECLARE @flgGate INT, @MatchSeatTicketDetailId BIGINT = 0                                                    
SET @flgGate=0                                                                               
                                                                                       
DECLARE @EventIdS VARCHAR(MAX), @ScanGate NVARCHAR(500)                                                    
SET @EventIdS='6688';                                                  
SET @ScanGate = @Gate;                                                  
IF(@Gate='AllGate')                                                                                                                                                                                                                                          
BEGIN                                                    
 SELECT @Gate=ISNULL(C.ScanGateName,D.Name) from MatchSeatTicketDetails A WITH(NOLOCK)                                                                                 
 Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                                                                                                       
 Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                    
 INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                     
 Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(                                                    
 SELECT * FROM SplitString(@EventIDS,',')))                                                    
 PRINT @Gate                                                                                          
 SET @flgGate=1                                                                                                                                 
END                                                      
                                                    
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0) from MatchSeatTicketDetails A  WITH(NOLOCK)                                                                                
Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                                                                                                        
Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                    
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                     
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(                                                    
SELECT * FROM SplitString(@EventIDS,','))) AND ISNULL(C.ScanGateName,D.Name)=@Gate                                                    
                                                    
--SELECT @MatchSeatTicketDetailId                                                    
                                                    
IF(@MatchSeatTicketDetailId <> '' AND @MatchSeatTicketDetailId <> 0)               
 BEGIN                                                    
  INSERT INTO BarcodeScanLogs(EventTicketDetailId,BarcodeNumber,DeviceName,ScanGateName,IsEnabled,CreatedUtc,CreatedBy)                                                    
  SELECT EventTicketDetailId, BarcodeNumber,@DeviceId,@ScanGate,1,GETUTCDATE(),NEWID() FROM                                  
  MatchSeatTicketDetails WITH(NOLOCK) WHERE  Id = @MatchSeatTicketDetailId                                                      
  Update MatchSeatTicketDetails SET CheckedDateTime=GETUTCDATE() WHERE Id = @MatchSeatTicketDetailId                                                                        
END                                
ELSE                                                    
BEGIN                                  
  INSERT INTO BarcodeScanLogs(EventTicketDetailId,BarcodeNumber,DeviceName,ScanGateName,IsEnabled,CreatedUtc,CreatedBy)                                                    
  VALUES(NULL, @Barcode,@DeviceId,@ScanGate,1,GETUTCDATE(),NEWID())                                                    
                                                                                      
END                                                     
                                      
IF(@MatchSeatTicketDetailId <> '' AND @MatchSeatTicketDetailId <> 0)                                                  
BEGIN                                                     
 DECLARE @tmpcount BIGINT=0                                                                                                         
 SET @tmpcount=ISNULL((SELECT COUNT(*) FROM  MatchSeatTicketDetails  A WITH(NOLOCK)                                                      
 WHERE A.Id  = @MatchSeatTicketDetailId),0)                                                    
                                                                                                                                    
  IF (@tmpcount>0)                                                                                 
  BEGIN                                                     
  UPDATE MatchSeatTicketDetails  SET EntryCount=ISNULL(EntryCount,0)+1,EntryStatus=1,EntryDateTime=GETUTCDATE()                                                    
  WHERE Id  = @MatchSeatTicketDetailId                                                           
 END      END                                                    
                                                    
SELECT CONVERT(INT,ISNULL(A.EntryStatus,0))[EntryStatus], ISNULL(A.EntryCount,0)[EntryCount], A.EntryDateTime AS EntryDate,                                                    
D.Name As GateID, C.SectionName,                                                     
(SUBSTRING( ISNULL(B.SeatTag,''),0,PATINDEX('%-%',B.SeatTag))) +'-'+ SUBSTRING( ISNULL(B.SeatTag,''),                                                    
PATINDEX('%-%',B.SeatTag)+1,Len(B.SeatTag)) AS RowTic_No, D.Name AS Entrance, EA.Price AS PricePerTic,                                                     
Isnull(EntryCountAllowed,1) AS BarcodeCountAllowed, A.BarcodeNumber AS Barcode, A.TransactionId AS TransId, '' AS TktSerialNo,                                                    
A.CreatedUtc As CreatedDate,                                                    
CASE T.ChannelId WHEN 1 THEN 'Website' WHEN 2 THEN 'Retail' WHEN 4 THEN 'BO' WHEN 8 THEN 'KITMS' ELSE 'NA' END AS TransactionBy,                                                    
CASE WHEN @flgGate=1 THEN 'AllGate' ELSE ISNULL(C.ScanGateName,D.Name) END AS Gate                                                    
 FROM MatchSeatTicketDetails A WITH(NOLOCK)                                                    
INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                                                                                                        
INNER JOIN  MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                            
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                    
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON A.EventTicketDetailId = ETD.Id                                                    
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id = EA.EventTicketDetailId                                                    
INNER JOIN Transactions T WITH(NOLOCK) ON A.TransactionId = T.id                                                    
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(                                           
SELECT * FROM SplitString(@EventIDS,',')))                                                  
                                                    
SELECT COUNT(*)[Count] FROM  MatchSeatTicketDetails  A WITH(NOLOCK)                                                  
WHERE BarcodeNumber=@Barcode AND A.EventTicketDetailId IN                                                     
(SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))                                                     
 END 
