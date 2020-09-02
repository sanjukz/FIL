CREATE PROC [dbo].[ITKTS_SyncOfflineBarcode]                                                                                         
(                                                                                         
	@Barcode varchar(500),                                                                                       
	@DateString varchar(500),                                                                                       
	@ReturnVal BIGINT OUTPUT                                                           
)                                                                                          
AS                                                                                          
BEGIN                        
DECLARE @EventIDS VARCHAR(MAX), @MatchSeatTicketDetailId BIGINT = 0                                                  
                                                    
SET @EventIDS='6688';                                                  
                                                    
SELECT @MatchSeatTicketDetailId=ISNULL(A.Id,0) FROM MatchSeatTicketDetails A WITH(NOLOCK)                                                                               
Inner Join MatchLayoutSectionSeats B WITH(NOLOCK) on  A.MatchLayoutSectionSeatId = B.Id                                                                                                                                                                                
Inner Join MatchLayoutSections C WITH(NOLOCK) on B.MatchLayoutSectionId=C.Id                                                  
INNER JOIN EntryGates D WITH(NOLOCK) ON C.EntryGateId = D.Id                                                   
Where A.BarcodeNumber=@Barcode AND A.EventTicketDetailId IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) 
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIDS,',')))                                                  
                                                  
IF(@MatchSeatTicketDetailId <> '' AND @MatchSeatTicketDetailId <> 0)                                                  
 BEGIN                                                  
  INSERT INTO BarcodeScanLogs(EventTicketDetailId,BarcodeNumber,DeviceName,ScanGateName,IsEnabled,CreatedUtc,CreatedBy)                                                  
  SELECT EventTicketDetailId, BarcodeNumber,NULL,NULL,1,GETUTCDATE(),NEWID() FROM                                                  
  MatchSeatTicketDetails WITH(NOLOCK) WHERE  Id = @MatchSeatTicketDetailId                                                                           
END                                                  
ELSE                                                  
BEGIN                                                                                                                                                            
  INSERT INTO BarcodeScanLogs(EventTicketDetailId,BarcodeNumber,DeviceName,ScanGateName,IsEnabled,CreatedUtc,CreatedBy)                                                  
  VALUES(NULL, @Barcode,NULL,NULL,1,GETUTCDATE(),NEWID())                                                                                                                       
END                                                   
                                                                                                                                                   
IF(@MatchSeatTicketDetailId <> '' AND @MatchSeatTicketDetailId <> 0)                                                                                     
BEGIN                                                                                                                                                                 
 UPDATE MatchSeatTicketDetails SET CheckedDateTime=GETDATE() WHERE Id = @MatchSeatTicketDetailId                                                  
                                                           
DECLARE @Tempcount bigint=0                            
SET @Tempcount=ISNULL((SELECT COUNT(*) FROM  MatchSeatTicketDetails A WITH(NOLOCK)                                
WHERE A.Id  = @MatchSeatTicketDetailId),0)                             
                                       
if @Tempcount>0                                      
BEGIN                                                  
 UPDATE MatchSeatTicketDetails  SET EntryCount=ISNULL(EntryCount,0)+1,EntryStatus=1,                                                  
 EntryDateTime=@DateString WHERE Id  = @MatchSeatTicketDetailId                                                  
 SET @ReturnVal=1                                                                             
END                                                                             
ELSE                                               
BEGIN                                                                   
SET @ReturnVal=0                                                                 
END                                                                                     
  END                                                                                 
END     