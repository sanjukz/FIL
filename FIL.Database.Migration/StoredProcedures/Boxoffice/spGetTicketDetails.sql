create proc spGetTicketDetails  
(  
 @TransactionId BIGINT = 0,   
 @TransactionAltId uniqueidentifier = null,    
 @ModifiedBy  uniqueidentifier = null          
)  
AS  
BEGIN  
IF(@TransactionId=0)                          
BEGIN                          
SELECT @TransactionId = Id FROM Transactions WITH(NOLOCK) WHERE AltId = @TransactionAltId                          
END   
  
SELECT VD.Id As VenueId,VD.NAME AS VenueName,C.Name AS CityName,E.Id AS EventId,E.Name AS EventName,ED.Id AS EventDeatilId,ED.NAME AS EventDetailsName,  
ED.StartDateTime As EventStartTime,EA.TicketHtml,EA.GateOpenTime,EA.MatchNo,EA.MatchDay,EA.MatchAdditionalInfo,T.Firstname +' ' + T.Lastname AS SponsorOrCustomerName,  
TC.Id AS TicketCategoryId,TC.NAME AS TicketCategoryName,ISNULL(MSTD.Price,TD.PricePerTicket) AS Price,MSTD.BarcodeNumber,CT.Code AS CurrencyName,  
(SELECT Isnull(SectionName,'') from MatchLayoutSections WITH(NOLOCK) where Id In(Select  StandIdTemp from udfGetMatchSectionHierarchy(MLS.Id)))                                  
AS StandName,                                 
(SELECT Isnull(SectionName,'') from  MatchLayoutSections WITH(NOLOCK) where Id In(Select  LevelIdTemp from udfGetMatchSectionHierarchy (MLS.Id)))                                   
AS LevelName,                                  
(SELECT Isnull(SectionName,'') from  MatchLayoutSections WITH(NOLOCK) where Id In(Select  BlockIdTemp from udfGetMatchSectionHierarchy (MLS.Id)))                           
AS BlockName,   
MLS.SectionName,    
MLS.Id,           
EG.Name AS GateName,  
SUBSTRING( ISNULL(SeatTag,''),0,PATINDEX('%-%',SeatTag))AS RowNumber, SUBSTRING( ISNULL(SeatTag,''),PATINDEX('%-%',SeatTag)+1,LEN(SeatTag))                                                
AS TicketNumber,  
MSTD.TicketTypeId,  
Isnull(WCS.IsWheelChair,0) AS IsWheelChair,ETA.IsSeatSelection  
FROM Transactions T WITH(NOLOCK)                                                
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId                                                
INNER JOIN EventTicketAttributes ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id                                                
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id                                                
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                                                
INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                                              
INNER JOIN EventAttributes EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId                                                
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id                                                
INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id                         
INNER JOIN Cities C WITH(NOLOCK) ON VD.CityId = C.Id                                                
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id                                                
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId                                                
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id                                                
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id                                                
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id                                      
LEFT Outer Join WheelchairSeatMappings WCS WITH(NOLOCK) ON WCS.MatchSeatTicketDetailId=MSTD.ID                                              
WHERE T.Id = @TransactionId AND T.TransactionStatusId =8 AND PrintStatusId <>2                                                
ORDER BY ED.StartDateTime, ETA.Id                                      
  
IF(@ModifiedBy=null)                                                
BEGIN                                                
  SELECT  @ModifiedBy = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id = @TransactionId                                                
END                                                
ELSE                                                
BEGIN                                                
 SELECT TOP 1 @ModifiedBy = Altid FROM USers WITH(NOLOCK) WHERE Altid = @ModifiedBy                                                
END                                                
                                                
UPDATE MatchSeatTicketDetails                                                                                                           
SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETUTCDATE(), PrintedBy =@ModifiedBy                                                
WHERE TransactionId=@TransactionId AND BarcodeNumber IS NOT NULL                                                                                      
  
SELECT TA.Name as TeamA, TB.Name AS TeamB, MA.MatchNo, MA.MatchStartTime, MA.EventDetailId  FROM MatchAttributes MA WITH(NOLOCK)                       
INNER JOIN Teams TA WITH(NOLOCK) ON TA.Id = MA.TeamA                      
INNER JOIN Teams TB WITH(NOLOCK)  ON TB.Id = MA.TeamB                      
WHERE MA.EventDetailId IN (  
Select ETD.EventDetailId FROM Transactions T WITH(NOLOCK)                                                
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId                                                
INNER JOIN EventTicketAttributes ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id                                                
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id   
WHERE T.Id =@TransactionId AND T.TransactionStatusId =8)   
ORDER BY MA.MatchNo    
  
END  