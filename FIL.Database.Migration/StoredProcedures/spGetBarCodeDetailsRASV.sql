Create PROC spGetBarCodeDetailsRASV                             
(                                                        
  @Barcode VARCHAR(500),                                                   
  @Gate VARCHAR(500),                                                        
  @DeviceId VARCHAR(20)                                                       
)                                                        
AS                                                        
BEGIN                                                   

DECLARE  @MatchSeatTicketDetailId BIGINT = 0, @ChannelId INT=0,@BarcodeEventId BIGINT, @OfflineBarcodeDetailId BIGINT=0                                                                                             
DECLARE @EventIdS VARCHAR(MAX), @ScanGate NVARCHAR(500) ,@OtherEventIdS VARCHAR(MAX),@ExhibitEventIdS VARCHAR(MAX)  
                                                       
SET @EventIdS='23860,23859,23858,23857,23856,23855';  
SET @OtherEventIdS='23860,23859,23858,23857,23856';  
SET @ExhibitEventIdS='23855';  
  
SELECT Distinct @ChannelId=B.ChannelId from MatchSeatTicketDetails A  WITH(NOLOCK)                                                                                    
Inner Join Transactions B WITH(NOLOCK) on  A.TransactionId = B.Id   
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))     
  
  
SELECT @BarcodeEventId=ISNULL(C.Id,0) from MatchSeatTicketDetails A  WITH(NOLOCK)   
Inner Join EventTicketDetails B WITH(NOLOCK)  ON A.EventticketDetailid =B.Id                                                                                   
Inner Join EventDetails C WITH(NOLOCK) on  B.EventDetailId = C.ID   
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))   
  
DECLARE @currentAustralianTime time, @TimeZone NvARCHAR(50)  
SELECT @TimeZone=IsNull(TimeZone,0) FROM EventAttributes where EventDetailId=@BarcodeEventId  
SET @currentAustralianTime= CONVERT(time,DATEADD(MI,CONVERT(Int,@TimeZone),GETUTCDATE()))  
  
SELECT @OfflineBarcodeDetailId=Id FROM  OfflineBarcodeDetails  WITH(NOLOCK) WHERE BarcodeNumber= @Barcode AND EventTicketDetailId IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@OtherEventIdS,',')))         
  
If(@ChannelId=4)  --Boxoffice Check  
BEGIN  
--Print 'Boxoffice 1'   
If(@BarcodeEventId=23859)  
BEGIN  
--Print 'Boxoffice After 5'   
If(@currentAustralianTime>'17:00:00')  
BEGIN  
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0)                                                      
FROM MatchSeatTicketDetails A WITH(NOLOCK)                                                        
INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                            
INNER JOIN  MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                        
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON A.EventTicketDetailId = ETD.Id                                                        
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id = EA.EventTicketDetailId                                                        
INNER JOIN Transactions T WITH(NOLOCK) ON A.TransactionId = T.id                                                        
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@OtherEventIdS,',')))  
AND CONVERT(Date,DATEADD(MI,CONVERT(Int,@TimeZone),T.UpdatedUtc)) = CONVERT(Date,DATEADD(MI,CONVERT(Int,@TimeZone),GETUTCDATE()))  
END  
END  
ELSE  
BEGIN 
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0)                                                      
FROM MatchSeatTicketDetails A WITH(NOLOCK)                                                        
INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                            
INNER JOIN  MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                        
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON A.EventTicketDetailId = ETD.Id                                                        
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id = EA.EventTicketDetailId                                                        
INNER JOIN Transactions T WITH(NOLOCK) ON A.TransactionId = T.id                                                        
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@OtherEventIdS,',')))  
AND CONVERT(Date,DATEADD(MI,CONVERT(Int,@TimeZone),T.UpdatedUtc)) = CONVERT(Date,DATEADD(MI,CONVERT(Int,@TimeZone),GETUTCDATE()))  
END
END  
ELSE -- Other Channel Conditions  
BEGIN  
If(@BarcodeEventId=23859)  
BEGIN   
If(@currentAustralianTime>'17:00:00')  
BEGIN  
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0) from MatchSeatTicketDetails A  WITH(NOLOCK)                                                                                    
Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id   
Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                        
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                         
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@OtherEventIdS,',')))  
END  
END  
ELSE  
BEGIN  
if(@Gate='Exhibit Entry')
BEGIN
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0) from MatchSeatTicketDetails A  WITH(NOLOCK)                                                                                    
Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id   
Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                        
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                         
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@ExhibitEventIdS,','))) 
END
ELSE
BEGIN
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0) from MatchSeatTicketDetails A  WITH(NOLOCK)                                                                                    
Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id   
Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                        
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                         
Where A.BarcodeNumber=@Barcode AND A.EventticketDetailid IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@OtherEventIdS,',')))
END  
END  
END  
  
If(@MatchSeatTicketDetailId<>0)  
BEGIN  
 DECLARE @tmpcount BIGINT=0                                                                                                             
 SET @tmpcount=ISNULL((SELECT COUNT(*) FROM  MatchSeatTicketDetails  A WITH(NOLOCK)                                                          
 WHERE A.Id  = @MatchSeatTicketDetailId),0)                                                        
                                                                                                                                        
  IF (@tmpcount>0)                                                                                     
  BEGIN                                                         
  UPDATE MatchSeatTicketDetails  SET EntryCount=ISNULL(EntryCount,0)+1,EntryStatus=1,EntryDateTime=GETUTCDATE(),EntryGateName=@Gate  WHERE Id  = @MatchSeatTicketDetailId                                                               
  END                                                           
                                     
 SELECT CONVERT(INT,ISNULL(A.EntryStatus,0))[EntryStatus], ISNULL(A.EntryCount,0)[EntryCount], A.EntryDateTime AS EntryDate,                                                        
 Isnull(EntryCountAllowed,1) AS BarcodeCountAllowed, A.BarcodeNumber AS Barcode, A.TransactionId AS TransId,                                                         
 A.CreatedUtc As CreatedDate,                                                        
 T.ChannelId AS TransactionBy,                                                        
 'AllGate' AS Gate                                                        
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
ELSE IF(@OfflineBarcodeDetailId<>0)  
BEGIN  
  
DECLARE @tmpofflinecount BIGINT=0                                                                                                             
 SET @tmpofflinecount=ISNULL((SELECT COUNT(*) FROM  OfflineBarcodeDetails  A WITH(NOLOCK)                                                          
 WHERE Id= @OfflineBarcodeDetailId),0)                                                        
                                                                                                                                        
  IF (@tmpofflinecount>0)                                                                                     
  BEGIN                                                         
  UPDATE OfflineBarcodeDetails SET EntryCount=ISNULL(EntryCount,0)+1,EntryStatus=1,EntryDateTime=GETUTCDATE(),EntryGateName=@Gate  WHERE Id = @OfflineBarcodeDetailId                                                               
 END                                                           
                                                        
SELECT CONVERT(INT,ISNULL(A.EntryStatus,0))[EntryStatus], ISNULL(A.EntryCount,0)[EntryCount], A.EntryDateTime AS EntryDate,                                                        
Isnull(EntryCountAllowed,1) AS BarcodeCountAllowed, A.BarcodeNumber AS Barcode, -1 AS TransId,A.CreatedUtc As CreatedDate,0 AS TransactionBy,                                                        
'AllGate' AS Gate                                                        
FROM OfflineBarcodeDetails A WITH(NOLOCK)                                                   
Where A.Id=@OfflineBarcodeDetailId AND A.EventTicketDetailId IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))                                                      
                                                        
SELECT COUNT(*)[Count] FROM  OfflineBarcodeDetails  A WITH(NOLOCK)                                                      
WHERE BarcodeNumber=@Barcode AND A.EventTicketDetailId IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))    

END  
ELSE  
BEGIN  
SELECT 0 [Count]  
SELECT 0 [Count]   
END  
END 