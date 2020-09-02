CREATE PROCEDURE [dbo].[GetStandWiseEntryCount]  --22423                                                                           
 (               
--DECLARE                                      
	 @EventId BIGINT  = 3450                                                                                                                    
)                                                                                                                    
AS               
BEGIN                                       
BEGIN                
DECLARE @tblValues TABLE  (VenueCatName VARCHAR(1000), EntryCount INT)                                                                          

If(@EventId=22425 OR @EventId=22426 )   
BEGIN                
INSERT INTO @tblValues                                              
SELECT           
CASE WHEN TC.Name Like'%South Box%' THEN 'Boxes'             
WHEN TC.Name Like'%Mound - Specially Abled%' THEN 'West Mound'              
--WHEN TC.Name Like'%North Stand Level 1 - Block%' THEN 'North Stand'            
--WHEN TC.Name Like'%North Stand Level 3 - Block%' THEN 'North Stand'            
--WHEN TC.Name Like'%KCC Pavilion%' THEN 'KCC Pavilion'            
--WHEN TC.Name Like'%North Level%' THEN 'North Stand Boxes'            
--WHEN TC.Name Like'%George Headley Level 3 - Box%' THEN 'George Headley Boxes'            
--WHEN TC.Name Like'%George Headley Level 4 - Box%' THEN 'George Headley Boxes'         
--WHEN TC.Name Like'%South Box%' THEN 'Boxes'          
ELSE                                  
                                   
CASE WHEN CHARINDEX('-',TC.Name) > 0 THEN SUBSTRING(TC.Name, 0, CHARINDEX('-',TC.Name)) ELSE TC.Name  END END --END END                   
AS VenueCatName,                                                        
(                                              
ISNULL(                                              
(SELECT COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                        
INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id              
WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId AND T.TransactionStatusId=8                                
AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null                                              
),0)                                              
) AS EntryCount                                              
                                            
FROM EventTicketAttributes ETA  WITH (NOLOCK)                
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETA.EventTicketDetailId=ETD.ID                                           
INNER JOIN TicketCategories TC WITH (NOLOCK) ON ETD.TicketCategoryId = TC.ID               
WHERE ETA.EventTicketDetailId In(Select ID From EventTicketDetails  where EventDetailId=@EventId)               
--And ETA.IsEnabled=1                                 
ORDER BY  ETA.Price DESC, ETA.IsEnabled DESC                                                 
END
ELSE
BEGIN
INSERT INTO @tblValues                                              
SELECT           
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
--CASE WHEN TC.Name Like'%Suite%' THEN 'Suite' ELSE            
CASE WHEN CHARINDEX('-',TC.Name) > 0 THEN SUBSTRING(TC.Name, 0, CHARINDEX('-',TC.Name)) ELSE TC.Name END END END       
AS VenueCatName,                                              
(                                              
ISNULL(                                              
(SELECT COUNT(*) from MatchSeatTicketDetails  MSTD WITH (NOLOCK)                        
INNER JOIN Transactions T WITH (NOLOCK) ON MSTD.TransactionId = T.Id              
WHERE MSTD.EventTicketDetailId = ETA.EventTicketDetailId AND T.TransactionStatusId=8                                
AND BarcodeNumber IS NOT NULL AND MSTD.EntryStatus = 1 AND EntryCount is not null AND EntryDateTime is not null                                              
),0)                                              
) AS EntryCount                                              
                                            
FROM EventTicketAttributes ETA  WITH (NOLOCK)                
Inner Join EventTicketDetails ETD WITH (NOLOCK) ON ETA.EventTicketDetailId=ETD.ID                                           
INNER JOIN TicketCategories TC WITH (NOLOCK) ON ETD.TicketCategoryId = TC.ID               
WHERE ETA.EventTicketDetailId In(Select ID From EventTicketDetails  where EventDetailId=@EventId)               
--And ETA.IsEnabled=1                                 
ORDER BY  ETA.Price DESC, ETA.IsEnabled DESC        
END             
END                              
BEGIN                                     
SELECT VenueCatName AS TicketCategory, SUM(EntryCount) AS EntryCount                                              
FROM @tblValues                                                 
GROUP BY  VenueCatName                                                
ORDER BY SUM(EntryCount) DESC                                              
END                              
END 
