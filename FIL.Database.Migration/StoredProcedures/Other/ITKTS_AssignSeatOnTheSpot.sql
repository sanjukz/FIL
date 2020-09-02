CREATE proc [dbo].[ITKTS_AssignSeatOnTheSpot]                                                                                                                      
(                                                                                                                                                                              
	 @EventTransID BIGINT,                                   
	 @UserId BIGINT = 0                                                                                                                                                                          
)                        
AS                        
BEGIN                        
                                      
DECLARE @totalLoopCount INT, @VMCC_Id BIGINT, @NoofTic INT, @BlockId VARCHAR(100), @TicNumId BIGINT, @AltId NVARCHAR(500)                        
DECLARE @loopCounter INT = 1, @TotalQty INT, @AssignedQty INT, @EventTicketDetailId BIGINT, @TicketTypeId INT,@TransactionDetailsId BIGINT                       
DECLARE @tblVMCCTicNumMap TABLE (Sno INT identity(1,1), VMCC_Id BIGINT, EventTicketDetailId BIGINT, TicNumId BIGINT,                         
PricePerTic DECIMAL(18,2), IsFamilyPkg INT, IsChild INT, IsSrCitizen INT, EventId BIGINT,IsPackage INT)                                       
DECLARE @tblVMCCId TABLE (Sno INT identity(1,1), VMCC_Id BIGINT)                                       
DECLARE @tblTicketType TABLE (Sno INT identity(1,1), VMCC_Id BIGINT, EventId BIGINT, PricePerTic DECIMAL(18,2),                         
IsFamilyPkg INT, IsChild INT, IsSrCitizen INT, Quantity INT)                                       
                        
SELECT @AssignedQty = COUNT(*) FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId = @EventTransID                        
SELECT @TotalQty = SUM(VMT.TotalTickets)                        
FROM Transactions ETD WITH(NOLOCK)                                     
INNER JOIN TransactionDetails VMT WITH(NOLOCK) on ETD.Id = VMT.TransactionId                                      
WHERE ETD.TransactionStatusId = 8 AND ETD.Id=@EventTransID                             
                        
 IF NOT EXISTS (SELECT * FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId)                                      
 BEGIN                        
                        
 --========================= Seat assignment starts here =================================================                        
                        
 DECLARE @tblTics TABLE (Sno INT IDENTITY(1,1), VMCC_Id BIGINT, EventTicketDetailId BIGINT, NoofTic INT,                         
 VenueCatName varchar(100), EventStartDate DATETIME, TicketTypeId INT,TransactionDetailsId Bigint)                        
                                 
 INSERT INTO @tblTics                                       
 SELECT  ETA.Id, ETD.Id,TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId ,TD.Id                       
 FROM Transactions T WITH(NOLOCK)                                           
 INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId                                       
 INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id                        
 INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id                        
 INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                                       
 INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                                      
 WHERE T.TransactionStatusId = 8 AND T.Id=@EventTransID                                      
 GROUP BY ETA.Id,ETD.Id, TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId,TD.Id                        
                        
 --SELECT * FROm @tblTics                        
                        
 SELECT @totalLoopCount = COUNT(*) FROM @tblTics                                       
 SET @loopCounter = 1                                      
 WHILE (@loopCounter <= @totalLoopCount)                                      
 BEGIN                        
  SELECT @VMCC_Id = VMCC_Id, @NoofTic = NoofTic, @EventTicketDetailId = EventTicketDetailId, @TicketTypeId = TicketTypeId,@TransactionDetailsId=TransactionDetailsId                        
  FROM @tblTics WHERE Sno=@loopCounter                        
  --SELECT @VMCC_Id, @NoofTic, @EventTicketDetailId         
--==============Added Conditions for Seated Event============================================================                     
  IF EXISTS (SELECT Id FROM TransactionSeatDetails WITH(NOLOCK) WHERE TransactionDetailId = @TransactionDetailsId)                                      
  BEGIN      
  UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  @TicketTypeId                                 
  WHERE Id IN (Select MatchSeatTicketDetailId From TransactionSeatDetails WITH(NOLOCK) where TransactionDetailId=@TransactionDetailsId )       
  END      
  ELSE      
  BEGIN                      
  UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  @TicketTypeId                                 
  WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                        
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                        
  WHERE  EventTicketDetailId=@EventTicketDetailId                        
  AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)       
  END                       
                        
  --SELECT TOP (@NoOfTic) A.* FROM MatchSeatTicketDetails A WITH(NOLOCK)                       
  --INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id                        
  --WHERE  EventTicketDetailId=@EventTicketDetailId                        
  --AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder                        
                        
 SET @loopCounter = @loopCounter + 1                                      
 END                        
--========================= Seat assignment Ends here =================================================                        
END                         
IF EXISTS (SELECT TransactionId FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId AND PrintStatusId <>2)                                      
BEGIN                        
--========================= Bracode assignment starts here =================================================                        
 DECLARE @tblTicNumId table (Sno INT identity(1,1), TicNumId BIGINT)                                        
 INSERT INTO @tblTicNumId                                      
 SELECT Id FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId                          
 --SELECT * FROM @tblTicNumId                        
                        
 SELECT @totalLoopCount = COUNT(*) FROM @tblTicNumId                                      
 SET @loopCounter = 1                                      
 WHILE (@loopCounter <= @totalLoopCount)                                      
 BEGIN                                      
 DECLARE @BarcodeEventId VARCHAR(04), @TicketType VARCHAR(02), @ChkPrintCount INT = 0, @SeatId VARCHAR(100) = 0, @SeatTag VARCHAR(100)                                      
 SELECT @TicNumId = TicNumId FROM @tblTicNumId WHERE Sno = @loopCounter                                      
                          
 INSERT INTO @tblVMCCTicNumMap                        
 SELECT C.Id, B.Id, A.Id, C.Price, 0, 0, 0, D.Id,0                        
 FROM MatchSeatTicketDetails  A WITH(NOLOCK)                       
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id                        
 INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.Id= C.EventTicketDetailId                        
 INNER JOIN EventDetails D WITH(NOLOCK) ON B.EventDetailId= D.Id                        
 WHERE A.Id = @TicNumId                                    
  --SELECT * FROM @tblVMCCTicNumMap                        
 SELECT                                       
 @BarcodeEventId = CASE LEN(DATEPART(DD,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(DD,StartDateTime))                         
 ELSE CONVERT(VARCHAR,DATEPART(DD,StartDateTime)) END +                                       
 CASE LEN(DATEPART(MM,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MM,StartDateTime))                         
 ELSE CONVERT(VARCHAR,DATEPART(MM,StartDateTime)) END,                                      
 @ChkPrintCount = ISNULL(PrintCount,0),                        
 @SeatId = CASE LEN(D.Id) % 2 WHEN 0 THEN CONVERT(VARCHAR, D.Id) ELSE '0' + CONVERT(VARCHAR, D.Id) END,                                      
 @SeatTag = D.SeatTag                                      
 FROM EventDetails A WITH(NOLOCK)                       
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.Id = B.EventDetailId                                               
 INNER JOIN MatchSeatTicketDetails C WITH(NOLOCK) ON B.Id = C.EventTicketDetailId                                       
 INNER JOIN MatchLayoutSectionSeats D WITH(NOLOCK) ON C.MatchLayoutSectionSeatId = D.Id                                                        
 WHERE C.Id = @TicNumId           
                        
 SELECT  @TicketType = CASE PayConfNumber WHEN 'Complimentary' THEN '04' ELSE '01' END                                      
 FROM TransactionPaymentDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId                               
 --SELECT @ChkPrintCount                        
 IF(@ChkPrintCount > 0)                                      
 BEGIN                        
  UPDATE MatchSeatTicketDetails  SET                                       
  BarcodeNumber = ISNULL(@BarcodeEventId,'01') + ISNULL(@SeatId, '01') +                                       
  CASE WHEN @ChkPrintCount > 9 THEN CONVERT(VARCHAR, @ChkPrintCount) ELSE '0' + CONVERT(VARCHAR, @ChkPrintCount) END,                                       
  PrintCount = ISNULL(PrintCount, 0) + 1 WHERE Id = @TicNumId                         
 END                                      
 ELSE                                      
 BEGIN                                      
  UPDATE MatchSeatTicketDetails SET                                       
  BarcodeNumber = ISNULL(@BarcodeEventId,'01') + ISNULL(@SeatId, '01'),                                       
  PrintCount = 1 WHERE Id = @TicNumId                         
 END               
 --========================WC Companion BEGINS HERE========================================================              
 IF EXISTS(SELECT MatchSeatTicketDetailId FROM  WheelchairSeatMappings WITH(NOLOCK) WHERE MatchSeatTicketDetailId = @TicNumId and IsWheelChair=1)                            
 BEGIN              
 Print 'In to CM Ticket '              
 SELECT @TicNumId =  MatchSeatTicketDetailId                            
 FROM WheelchairSeatMappings WITH(NOLOCK)             
 WHERE IsWheelChair = (SELECT TOP 1 MatchSeatTicketDetailId FROM  WheelchairSeatMappings WITH(NOLOCK) WHERE MatchSeatTicketDetailId = @TicNumId and IsWheelChair=1)               
                     
 INSERT INTO @tblVMCCTicNumMap                        
 SELECT C.Id, B.Id, A.Id, 0, 0, 0, 0, D.Id,0                        
 FROM MatchSeatTicketDetails  A WITH(NOLOCK)                        
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id                        
 INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.Id= C.EventTicketDetailId                        
 INNER JOIN EventDetails D WITH(NOLOCK) ON B.EventDetailId= D.Id                        
 WHERE A.Id = @TicNumId                                    
 --SELECT * FROM @tblVMCCTicNumMap                        
 SELECT                                       
 @BarcodeEventId = CASE LEN(DATEPART(DD,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(DD,StartDateTime))                         
 ELSE CONVERT(VARCHAR,DATEPART(DD,StartDateTime)) END +                                       
 CASE LEN(DATEPART(MM,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MM,StartDateTime))                         
 ELSE CONVERT(VARCHAR,DATEPART(MM,StartDateTime)) END,                                      
 @ChkPrintCount = ISNULL(PrintCount,0),                        
 @SeatId = CASE LEN(D.Id) % 2 WHEN 0 THEN CONVERT(VARCHAR, D.Id) ELSE '0' + CONVERT(VARCHAR, D.Id) END,                                      
 @SeatTag = D.SeatTag                                      
 FROM EventDetails A WITH(NOLOCK)                        
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.Id = B.EventDetailId                                               
 INNER JOIN MatchSeatTicketDetails C WITH(NOLOCK) ON B.Id = C.EventTicketDetailId                                       
 INNER JOIN MatchLayoutSectionSeats D WITH(NOLOCK) ON C.MatchLayoutSectionSeatId = D.Id                                                        
 WHERE C.Id = @TicNumId                 
               
 SELECT  @TicketType = CASE PayConfNumber WHEN 'Complimentary' THEN '04' ELSE '01' END                                      
 FROM TransactionPaymentDetails WITH(NOLOCK) WHERE TransactionId = @EventTransId                                      
 --SELECT @ChkPrintCount                        
 IF(@ChkPrintCount > 0)                                      
 BEGIN                        
 UPDATE MatchSeatTicketDetails  SET                                       
 BarcodeNumber = ISNULL(@BarcodeEventId,'01') + ISNULL(@SeatId, '01') +                                       
 CASE WHEN @ChkPrintCount > 9 THEN CONVERT(VARCHAR, @ChkPrintCount) ELSE '0' + CONVERT(VARCHAR, @ChkPrintCount) END,                                       
 PrintCount = ISNULL(PrintCount, 0) + 1 WHERE Id = @TicNumId                         
 END                                      
 ELSE                                      
 BEGIN                                      
 UPDATE MatchSeatTicketDetails SET                                       
 BarcodeNumber = ISNULL(@BarcodeEventId,'01') + ISNULL(@SeatId, '01'),                   
 PrintCount = 1 WHERE Id = @TicNumId                         
 END                        
                 
   UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId = 1 WHERE Id = @TicNumId               
                  
 END               
--========================WC Companion ends here========================================================                            
  SET @loopCounter = @loopCounter + 1                                     
 END                        
--========================= Bracode assignment Ends here =================================================               
                                         
DECLARE @tblTransactionDetails TABLE(Id BIGINT, PricePerTicket DECIMAL(18,2), EventTicketAttributeId BIGINT, TicketTypeId INt)                        
INSERT INTO @tblTransactionDetails                        
SELECT Id, PricePerTicket, EventTicketAttributeId, TicketTypeId                        
FROM TransactionDetails WITH(NOLOCK) WHERE TransactionId = @EventTransID                        
                        
                        
DECLARE @tblTransaction TABLE(VenueId INT, VMCC_Id BIGINT,EventCatName NVARCHAR(500),EventCatID BIGINT,                        
EventName NVARCHAR(500), EventId BIGINT, GateOpenTime NVARCHAR(100),Match_No INT, MatchDay INT,LocalPricePerTic DECIMAL(18,2),                        
SponsorOrCustomerName NVARCHAR(500),                        
Stadium_Name NVARCHAR(500),Stadium_city NVARCHAR(500), EventStarttime NVARCHAR(10), EventEndtime NVARCHAR(10),                         
VenueCatName NVARCHAR(500), ISPrintX INT, Barcode NVARCHAR(500), EventStartDate NVARCHAR(100),                        
CurrencyName NVARCHAR(10), Stand NVARCHAR(500), Entrance NVARCHAR(500), Gate_No NVARCHAR(100), RampNo NVARCHAR(100),                        
Row_No NVARCHAR(50), Tic_No NVARCHAR(50), IsChild INT, IsSRCitizen INT, VenueCatId INT,                        
IsWheelChair INT, IsFamilyPkg INT,IsLayoutAvail INT, TicketHtml NVARCHAR(100), RoadName NVARCHAR(500),StaircaseNo NVARCHAR(100),                        
PricePerTic DECIMAL(18,2), PrintCount INt)                        
                        
INSERT INTO @tblTransaction                        
SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,               
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, 0 AS LocalPricePerTic, T.Firstname +' ' + T.Lastname AS SponsorOrCustomerName,                        
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime,                         
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,                        
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,                        
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,                        
SUBSTRING( ISNULL(SeatTag,''),0,PATINDEX('%-%',SeatTag))AS Row_No, SUBSTRING( ISNULL(SeatTag,''),PATINDEX('%-%',SeatTag)+1,LEN(SeatTag))                        
 AS Tic_No,                         
CASE WHEN MSTD.TicketTypeId =2 THEN 1 ELSE 0 END AS IsChild, CASE WHEN MSTD.TicketTypeId =3 THEN 1 ELSE 0 END AS IsSRCitizen, TC.Id AS VenueCatId,                        
Isnull(WCS.IsWheelChair,0) AS IsWheelChair, 0 AS  IsFamilyPkg, Convert(INT,IsNull(IsSeatSelection,0)) AS IsLayoutAvail, EA.TicketHtml AS TicketHtml,                         
ISNULL(EG.StreetInformation,'') AS RoadName,                        
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo,                        
0 AS PricePerTic, PrintCount                        
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
WHERE T.Id = @EventTransID AND T.TransactionStatusId =8 AND PrintStatusId <>2                        
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
                        
IF(@UserId=0)                        
BEGIN                        
  SELECT  @AltId = CreatedBy FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransID                        
END                        
ELSE                        
BEGIN                        
 SELECT TOP 1 @AltId = Altid FROM USers WITH(NOLOCK) WHERE Id = @UserId                        
END                        
                        
UPDATE MatchSeatTicketDetails                                                                                   
SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETUTCDATE(), PrintedBy =@AltId                        
WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL                        
                      
--Declare @EventTransID BigInt=210001284                      
                      
SELECT FirstName +' '+ LastName AS SponsorOrCustomerName,FirstName +' '+ LastName AS SponsorName,  PhoneCode +' - '+ PhoneNumber as CustomerMobile,                      
(Select PaymentType From BO_RetailCustomer where Trans_Id=@EventTransID ) as PaymentType ,Isnull((Select Cust_IdType +' ' + Cust_IdTypeNumber From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID ),'') As IdTypeNumber,                   
(Select FirstName +' '+ LastName from  Users WITH(NOLOCK) where Id In(Select Retailer_Id From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID )) as UserName,                      
CreatedUtc,Id FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransID                        
                    
Select Name,IsNull(PhoneCode,'91')+'-'+MobileNumber as MobileNumber,Email,IdType+' '+IdNumber as IdTypeNumber  from VisitorDetails WITH(NOLOCK) where  TransactionId=@EventTransID                    
END                                                                  
END             
