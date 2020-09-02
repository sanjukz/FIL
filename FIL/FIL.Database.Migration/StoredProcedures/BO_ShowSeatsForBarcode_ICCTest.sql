  
Create proc BO_ShowSeatsForBarcode_ICCTest                                               
(                                                                                            
 @TicNumId varchar(max)                                                                             
)                                                                                            
AS                                                                                           
BEGIN               
              
DECLARE @EventTransID BIGINT               
SELECT @EventTransID = TransactionId FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE Id IN (SELECT keyword FROM SplitString(@TicNumId,','))              
                                                            
 DECLARE @tblTransactionDetails TABLE(Id BIGINT, PricePerTicket DECIMAL(18,2), EventTicketAttributeId BIGINT, TicketTypeId INt)              
INSERT INTO @tblTransactionDetails              
SELECT Id, PricePerTicket, EventTicketAttributeId, TicketTypeId              
FROM TransactionDetails WITH(NOLOCK) WHERE TransactionId = @EventTransID              
              
              
DECLARE @tblTransaction TABLE(VenueId INT, VMCC_Id BIGINT,EventCatName NVARCHAR(500),EventCatID BIGINT,              
EventName NVARCHAR(500), EventId BIGINT, GateOpenTime NVARCHAR(100),Match_No INT, MatchDay INT,LocalPricePerTic DECIMAL(18,2),              
Stadium_Name NVARCHAR(500),Stadium_city NVARCHAR(500), EventStarttime NVARCHAR(10), EventEndtime NVARCHAR(10),               
VenueCatName NVARCHAR(500), ISPrintX INT, Barcode NVARCHAR(500), EventStartDate NVARCHAR(100),              
CurrencyName NVARCHAR(10), Stand NVARCHAR(500), Entrance NVARCHAR(500), Gate_No NVARCHAR(100), RampNo NVARCHAR(100),              
Row_No NVARCHAR(50), Tic_No NVARCHAR(50), IsChild INT, IsSRCitizen INT, VenueCatId INT,              
IsWheelChair INT, IsFamilyPkg INT,IsLayoutAvail INT, TicketHtml NVARCHAR(100), RoadName NVARCHAR(500),StaircaseNo NVARCHAR(100),              
PricePerTic DECIMAL(18,2),PrintCount INT,SectionName varchar(500),StandName varchar(500),LevelName varchar(500),BlockName varchar(500),ShortDate varchar(500))              
              
INSERT INTO @tblTransaction              
SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,              
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, 0 AS LocalPricePerTic,              
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime,               
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,              
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,              
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,              
SUBSTRING( ISNULL(SeatTag,''),0,PATINDEX('%-%',SeatTag))AS Row_No, SUBSTRING( ISNULL(SeatTag,''),PATINDEX('%-%',SeatTag)+1,LEN(SeatTag))              
 AS Tic_No,               
CASE WHEN MSTD.TicketTypeId =2 THEN 1 ELSE 0 END AS IsChild, CASE WHEN MSTD.TicketTypeId =3 THEN 1 ELSE 0 END AS IsSRCitizen, TC.Id AS VenueCatId,              
Isnull(WCS.IsWheelChair,0) AS IsWheelChair, 0 AS  IsFamilyPkg, Convert(INT,Isnull(IsSeatSelection,0)) AS IsLayoutAvail, EA.TicketHtml AS TicketHtml,               
ISNULL(EG.StreetInformation,'') AS RoadName,              
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo,              
0 AS PricePerTic, PrintCount,  
MLS.SectionName,  
(SELECT Isnull(SectionName,'') from MatchLayoutSections WITH(NOLOCK) where Id In(Select  StandIdTemp from udfGetMatchSectionHierarchy(MLS.Id)))                  
AS StandName,                 
(SELECT Isnull(SectionName,'') from  MatchLayoutSections WITH(NOLOCK) where Id In(Select  LevelIdTemp from udfGetMatchSectionHierarchy (MLS.Id)))              
AS LevelName,                  
(SELECT Isnull(SectionName,'') from  MatchLayoutSections WITH(NOLOCK) where Id In(Select  BlockIdTemp from udfGetMatchSectionHierarchy (MLS.Id)))           
AS BlockName,  
convert(varchar(7), ED.StartDateTime, 100) as ShortDate              
FROM Transactions T WITH(NOLOCK)              
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId              
INNER JOIN EventTicketAttributes  ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id              
INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id              
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id              
INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id              
INNER JOIN EventAttributes  EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId              
INNER JOIN Events  E WITH(NOLOCK) ON ED.EventId = E.Id              
INNER JOIN Venues  VD WITH(NOLOCK) ON ED.VenueId = VD.Id              
INNER JOIN Cities  C WITH(NOLOCK) ON VD.CityId = C.Id              
INNER JOIN CurrencyTypes  CT WITH(NOLOCK) ON T.CurrencyId = CT.Id              
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId              
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id              
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id              
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id           
LEFT Outer Join WheelchairSeatMappings WCS WITH(NOLOCK) ON WCS.MatchSeatTicketDetailId=MSTD.ID                    
WHERE T.Id = @EventTransID AND T.TransactionStatusId =8 --AND PrintStatusId <>2              
AND MSTD.Id IN(SELECT keyword FROM SplitString(@TicNumId,','))              
ORDER BY ED.StartDateTime, ETA.Id              
              
UPDATE @tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket              
FROM @tblTransaction A              
INNER JOIN @tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId              
WHERE TicketTypeId = 1              
              
UPDATE @tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket              
FROM @tblTransaction A              
INNER JOIN @tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId              
WHERE IsChild  = 1 AND TicketTypeId = 2              
              
              
UPDATE @tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket              
FROM @tblTransaction A              
INNER JOIN @tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId              
WHERE IsSRCitizen  = 1 AND TicketTypeId = 3              
              
SELECT * FROM @tblTransaction              
ORDER BY EventStartDate, VMCC_Id              
              
UPDATE MatchSeatTicketDetails                                                                         
SET PrintCount = PrintCount + 1, PrintDateTime = GETUTCDATE()              
WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL              
              
--SELECT FirstName +' '+ LastName AS SponsorOrCustomerName,FirstName +' '+ LastName AS SponsorName              
--FROM Transactions WHERE Id = @EventTransID               
            
SELECT FirstName +' '+ LastName AS SponsorOrCustomerName,FirstName +' '+ LastName AS SponsorName,  PhoneCode +' - '+ PhoneNumber as CustomerMobile,              
(Select PaymentType From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID ) as PaymentType ,Isnull((Select Cust_IdType +' ' + Cust_IdTypeNumber From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID ),'') As IdTypeNumber,            
  
(Select FirstName +' '+ LastName from  Users WITH(NOLOCK) where Id In(Select Retailer_Id From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID )) as UserName,          
CreatedUtc,Id FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransID                
            
Select Name,IsNull(PhoneCode,'91')+'-'+MobileNumber as MobileNumber,Email,IdType+' '+IdNumber as IdTypeNumber  from VisitorDetails WITH(NOLOCK) where  TransactionId=@EventTransID            
  
SELECT TA.Name as TeamA, TB.Name AS TeamB, MA.MatchNo, MA.MatchStartTime, MA.EventDetailId  FROM MatchAttributes MA WITH(NOLOCK)       
INNER JOIN Teams TA WITH(NOLOCK) ON TA.Id = MA.TeamA      
INNER JOIN Teams TB WITH(NOLOCK)  ON TB.Id = MA.TeamB      
WHERE MA.EventDetailId IN (SELECT EventId FROM @tblTransaction) ORDER BY MA.MatchNo                                  
            
END 