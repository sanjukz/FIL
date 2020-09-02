CREATE Proc spScanningCount      
As      
BEGIN       
 DECLARE @tblCount TABLE  (EntryGateName VARCHAR(1000), EntryCount INT)      
 BEGIN

 INSERT INTO @tblCount        
 SELECT EntryGateName, COUNT(*) as EntryCount from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                                                    
 INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id        
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON MSTD.EventTicketDetailId=ETD.ID                                                                    
 WHERE T.TransactionStatusId=8  AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868) 
 AND CONVERT(DATE,DATEADD(MI,600,MSTD.EntryDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))      
 Group By EntryGateName    
 
 INSERT INTO @tblCount 
 SELECT EntryGateName, COUNT(*) as EntryCount from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                                                    
 INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id        
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON MSTD.EventTicketDetailId=ETD.ID                                                                    
 WHERE T.TransactionStatusId=8  AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868) 
 AND CONVERT(DATE,DATEADD(MI,600,MSTD.CheckedDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))    
 AND CONVERT(DATE,DATEADD(MI,600,MSTD.EntryDateTime)) < CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))          
 AND EntryCountAllowed>1
 Group By EntryGateName 
   
 
 INSERT INTO @tblCount      
 SELECT EntryGateName, COUNT(*) as EntryCount from OffLineBarcodeDetails  OBD WITH (NOLOCK)      
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON OBD.EventTicketDetailId=ETD.ID                                                                    
 WHERE  BarcodeNumber IS NOT NULL AND OBD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868,182363,182868)  AND  EventTicketDetailId NOt In(451722,451715) 
 AND CONVERT(DATE,DATEADD(MI,600,OBD.EntryDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))      
 Group By EntryGateName 
 
 INSERT INTO @tblCount 
 SELECT EntryGateName, SUM(IsNull(OBD.EntryCount,0)) as EntryCount  from OffLineBarcodeDetails  OBD WITH (NOLOCK)      
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON OBD.EventTicketDetailId=ETD.ID                                                                    
 WHERE  BarcodeNumber IS NOT NULL AND OBD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868,182363,182868)  AND  EventTicketDetailId NOt In(451722,451715)
 AND CONVERT(DATE,DATEADD(MI,600,OBD.CheckedDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))    
 AND CONVERT(DATE,DATEADD(MI,600,OBD.EntryDateTime)) < CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))          
 AND EntryCountAllowed>1
 Group By EntryGateName     
 
 INSERT INTO @tblCount      
 SELECT EntryGateName, SUM(IsNull(OBD.EntryCount,0)) as EntryCount from OffLineBarcodeDetails  OBD WITH (NOLOCK)      
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON OBD.EventTicketDetailId=ETD.ID                                                                    
 WHERE  BarcodeNumber IS NOT NULL AND OBD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868,182363,182868) AND  EventTicketDetailId In(451722,451715)  
 AND CONVERT(DATE,DATEADD(MI,600,OBD.EntryDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))                            
 Group By EntryGateName
  
 INSERT INTO @tblCount  
 SELECT EntryGateName, COUNT(*) as EntryCount  from OffLineBarcodeDetails  OBD WITH (NOLOCK)      
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON OBD.EventTicketDetailId=ETD.ID                                                                    
 WHERE  BarcodeNumber IS NOT NULL AND OBD.EntryStatus = 1 AND isnull(EntryCount, 0) > 0 AND EntryDateTime is not null      
 AND ETD.EventDetailId In(23860,23859,23858,23857,23855,182868,182363,182868) AND  EventTicketDetailId In(451722,451715)
 AND CONVERT(DATE,DATEADD(MI,600,OBD.CheckedDateTime)) = CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))    
 AND CONVERT(DATE,DATEADD(MI,600,OBD.EntryDateTime)) < CONVERT(DATE,DATEADD(MI,600,GETUTCDATE()))          
 AND EntryCountAllowed>1
 Group By EntryGateName   
 
 END  
       
 SELECT EntryGateName AS EntryGateName, SUM(EntryCount) AS EntryCount                                                                                
 FROM @tblCount                                                                                             
 GROUP BY  EntryGateName  ORDER BY SUM(EntryCount) DESC       
 
 END 