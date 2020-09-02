CREATE PROCEDURE [dbo].[GetStandWiseEntryReportTemp]  --22413                                                                                                              
(                                                  
--Declare                                                
	@EventId BIGINT  = 22423                                                                                                                                    
)                                                
AS                                                
BEGIN                                                
BEGIN                                                
DECLARE @tblValues TABLE  (VenueCatName VARCHAR(1000), EntryCount INT, PaidCount INT, CompCount INT,SRCitiZenCount INT)                                                
                                                
INSERT INTO @tblValues                                                
SELECT                                   
CASE WHEN TC.Name Like'%Gros Islet Block%' THEN 'Gros Islet' ELSE                        
CASE WHEN TC.Name Like'%Greenidge & Haynes%' THEN 'Greenidge and Haynes'  WHEN TC.Name Like'%Worrell Weekes and Walcott%' THEN 'WWW'  WHEN TC.Name Like'%Hall & Griffith%' THEN 'Hall and Griffith' WHEN TC.Name Like'%Hewitt & Innis%' THEN 'Hewitt and Innis'     
 ELSE        
CASE WHEN TC.Name Like'%George Headley Level 1 - Block%' THEN 'George Headley Stand'         
WHEN TC.Name Like'%George Headley Level 2 - Block%' THEN 'George Headley Stand'          
WHEN TC.Name Like'%North Stand Level 1 - Block%' THEN 'North Stand'        
WHEN TC.Name Like'%North Stand Level 3 - Block%' THEN 'North Stand'        
WHEN TC.Name Like'%KCC Pavilion%' THEN 'KCC Pavilion'        
WHEN TC.Name Like'%North Level%' THEN 'North Stand Boxes'        
WHEN TC.Name Like'%George Headley Level 3 - Box%' THEN 'George Headley Boxes'        
WHEN TC.Name Like'%George Headley Level 4 - Box%' THEN 'George Headley Boxes'     
WHEN TC.Name Like'%Pavilion%' THEN 'Boxes'      
ELSE                              
--West  Pavilion Upper                                  
--CASE WHEN TC.Name Like'%Suite%' THEN 'Suite' ELSE                                    
CASE WHEN CHARINDEX('-',TC.Name) > 0 THEN SUBSTRING(TC.Name, 0, CHARINDEX('-',TC.Name)) ELSE TC.Name  END END END END               
AS VenueCatName,        
--TC.Name ,                                               
(                                                
ISNULL(                                                
(select COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                                
 INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id                                                 
 WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId AND T.TransactionStatusId=8                                                
 AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null),0)                                                
 ) AS EntryCount,                                                
(                                                 
ISNULL((select COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                                           
INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id                                                 
WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId AND T.TransactionStatusId=8                                                
AND T.Id NOT In(Select TransactionId from TransactionPaymentDetails WITH(NOLOCK) where  RequestType='Complimentary')                                                               
AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null  And TicketTypeId<>3                                                
)                                                
,0))                                                
 AS PaidCount,                          
 ( ISNULL(                                                 
 (select COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                      
 INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id                                                 
 WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId                                                 
 AND T.TransactionStatusId=8  And T.ChannelId=8                                                 
 and T.Id In(Select TransactionId from TransactionPaymentDetails WITH(NOLOCK) where  RequestType='Complimentary')                                                                  
 AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null  ),0)                                                
 ) AS CompCount,                                                
                                                 
 (                                                 
ISNULL((select COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                                                           
INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id                                                 
WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId AND T.TransactionStatusId=8                              
AND T.Id NOT In(Select TransactionId from TransactionPaymentDetails WITH(NOLOCK) where  RequestType='Complimentary')                                                               
AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null  And TicketTypeId=3                                          
)                                                
,0))                                                
 AS SRCitiZenCount                                                        
                                                 
 FROM EventTicketAttributes ETA  WITH (NOLOCK)                                                   
 Inner Join EventTicketDetails ETD WITH (NOLOCK) ON ETA.EventTicketDetailId=ETD.ID                                                        INNER JOIN TicketCategories TC WITH                                                
 (NOLOCK) ON ETD.TicketCategoryId = TC.ID                                                  
 WHERE ETA.EventTicketDetailId In(Select ID From EventTicketDetails  WITH(NOLOCK) where EventDetailId=@EventId)                                                
 --And ETA.IsEnabled=1                                          
 --AND ETA.ID In(16532,16427,16428,16429,16430,16431,16432,16433,16434,16435,16436,16437,16438,16439,16440,16441,16475,16476,16477,16478)                                           
  --AND ETA.ID In(16638,16533,16534,16535,16536,16537,16538,16539,16540,16541,16542,16543,16544,16545,16546,16547,16581,16582,16583,16584)                                          
   -- AND ETA.ID In(16744,16639,16640,16641,16642,16643,16644,16645,16646,16647,16648,16649,16650,16651,16652,16653,16687,16688,16689,16690)                                         
                                           
 ORDER BY  ETA.Price DESC, ETA.IsEnabled DESC                                                                               
 END                                                
 BEGIN                                                
 SELECT VenueCatName AS TicketCategory, SUM(EntryCount) AS EntryCount,SUM(CompCount+SRCitiZenCount) as  CompCount,SUM(PaidCount) as PaidCount                                                             
 FROM @tblValues Where EntryCount>0                                                                            
 GROUP BY  VenueCatName  ORDER BY SUM(EntryCount) DESC     END  END 
GO
