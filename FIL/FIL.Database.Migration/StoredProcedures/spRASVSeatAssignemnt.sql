--exec spRASVSeatAssignemnt 210123006  
CREATE PROCEDURE spRASVSeatAssignemnt --ITKTS_AssignSeatOnTheSpot_Test_RASV   
(                                                                                                                                                        
  @EventTransID BIGINT = 0,                                             
  @UserId BIGINT = 0,                  
  @TransactionAltId uniqueidentifier = null                                                                                                                                                                                   
)  
AS                               
BEGIN    
 IF(@EventTransID=0)                  
 BEGIN                  
  SELECT @EventTransID = Id FROM Transactions WITH(NOLOCK) WHERE AltId = @TransactionAltId                  
 END     
 UPDATE MatchSeatTicketDetails SET PrintStatusId=1 WHERE TransactionId = @EventTransID   
 UPDATE MatchSeatTicketDetails SET SeatStatusId=1, BarcodeNumber=NULL, PrintStatusId=1, PrintCount=0, PrintedBy=NULL,   
 PrintDateTime = NULL, TransactionId = NULL where TransactionId = @EventTransID  
  
 IF NOT EXISTS (SELECT * FROM MatchSeatTicketDetails WITH (NOLOCK) WHERE TransactionId = @EventTransId)       
 BEGIN  
  DECLARE @totalLoopCount INT, @loopCounter INT = 1, @VMCC_Id BIGINT, @NoofTic INT, @EventTicketDetailId BIGINT, @TicketTypeId INT  
   
  CREATE TABLE #tblTics (Sno INT IDENTITY(1,1), VMCC_Id BIGINT, EventTicketDetailId BIGINT, NoofTic INT,                                   
  VenueCatName varchar(100), EventStartDate DATETIME, TicketTypeId INT)                                  
                                           
  INSERT INTO #tblTics                                                 
  SELECT  ETA.Id, ETD.Id,TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId                                  
  FROM Transactions T WITH (NOLOCK)                                                     
  INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId                                                 
  INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id                                  
  INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id                                  
  INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                                     
  INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                                                
  WHERE T.TransactionStatusId = 8 AND T.Id=@EventTransID                                                
  GROUP BY ETA.Id,ETD.Id, TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId    
  
  SELECT @totalLoopCount = COUNT(*) FROM #tblTics                                                 
  SET @loopCounter = 1  
  
  WHILE (@loopCounter <= @totalLoopCount)   
  BEGIN     
   SELECT @VMCC_Id = VMCC_Id, @NoofTic = NoofTic, @EventTicketDetailId = EventTicketDetailId, @TicketTypeId = TicketTypeId                                  
   FROM #tblTics WHERE Sno=@loopCounter     
  
   IF(@VMCC_Id = 98719 OR @VMCC_Id = 98725)                        
   BEGIN                        
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  1                         
    WHERE Id IN (SELECT TOP (@NoOfTic * 2) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                                  
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)  
    ORDER BY A.Id DESC ,B.ColumnOrder)                         
                          
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  2                                           
    WHERE Id IN (SELECT TOP (@NoOfTic * 2) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                                  
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)  
    ORDER BY A.Id,B.ColumnOrder DESC)                                               
   END                  
   ELSE IF(@VMCC_Id =98720)                        
   BEGIN                        
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  1                                           
    WHERE Id IN (SELECT TOP (@NoOfTic * 1) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                                  
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)  
    ORDER BY A.Id DESC ,B.ColumnOrder)                                  
                          
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  2                                           
    WHERE Id IN (SELECT TOP (@NoOfTic * 3) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                                  
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)  
    ORDER BY A.Id, B.ColumnOrder DESC)                                                 
   END              
   ELSE IF(@VMCC_Id =98721)                           
   BEGIN                        
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  @TicketTypeId                                           
    WHERE Id IN (SELECT TOP (@NoOfTic * 2) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                       
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)     
    ORDER BY A.Id DESC,B.ColumnOrder)                                                 
   END                          
   ELSE                    
   BEGIN                        
    UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  @TicketTypeId                                           
    WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)                                  
    INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL                                  
    WHERE  EventTicketDetailId=@EventTicketDetailId AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1)  
    ORDER BY A.Id,B.ColumnOrder)                                                 
   END                             
   SET @loopCounter = @loopCounter + 1  
  END  
  --DROP TABLE #tblTics  
 END     
 IF EXISTS (SELECT TransactionId FROM MatchSeatTicketDetails  WITH (NOLOCK) WHERE TransactionId = @EventTransId AND PrintStatusId <>2)                                                
 BEGIN  
  DECLARE @totalLoopCount1 INT, @loopCounter1 INT = 1, @TicNumId BIGINT   
   
  CREATE TABLE #tblTicNumId(Sno INT identity(1,1), TicNumId BIGINT)                                                  
  INSERT INTO #tblTicNumId                                                
  SELECT Id FROM MatchSeatTicketDetails  WITH (NOLOCK) WHERE TransactionId = @EventTransId   
  
  SELECT @totalLoopCount1 = COUNT(*) FROM #tblTicNumId  
  SET @loopCounter1 = 1   
  WHILE (@loopCounter1 <= @totalLoopCount1)                                                
  BEGIN  
   SELECT @TicNumId = TicNumId FROM #tblTicNumId WHERE Sno = @loopCounter1  
   DECLARE @BarcodeEventId VARCHAR(04), @SeatId VARCHAR(100) = 0  
  
   SELECT @BarcodeEventId = CASE LEN(DATEPART(DD,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(DD,StartDateTime))                                   
   ELSE CONVERT(VARCHAR,DATEPART(DD,StartDateTime)) END +                                                 
   CASE LEN(DATEPART(MM,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MM,StartDateTime))                                   
   ELSE CONVERT(VARCHAR,DATEPART(MM,StartDateTime)) END,  
   @SeatId = CASE LEN(D.Id) % 2 WHEN 0 THEN CONVERT(VARCHAR, D.Id) ELSE '0' + CONVERT(VARCHAR, D.Id) END               
   FROM EventDetails A WITH (NOLOCK)                                  
   INNER JOIN EventTicketDetails B WITH (NOLOCK) ON A.Id = B.EventDetailId                                                         
   INNER JOIN MatchSeatTicketDetails C WITH (NOLOCK) ON B.Id = C.EventTicketDetailId                                                 
   INNER JOIN MatchLayoutSectionSeats D WITH (NOLOCK) ON C.MatchLayoutSectionSeatId = D.Id                                                                  
   WHERE C.Id = @TicNumId      
  
   UPDATE MatchSeatTicketDetails SET                                                 
   BarcodeNumber = ISNULL(@BarcodeEventId,'01') + ISNULL(@SeatId, '01'), PrintCount = 1   
   WHERE Id = @TicNumId    
  
   SET @loopCounter1 = @loopCounter1 + 1      
  END  
  
  CREATE TABLE #tblTransactionDetails(Id BIGINT, PricePerTicket DECIMAL(18,2), EventTicketAttributeId BIGINT, TicketTypeId INt)                    
  INSERT INTO #tblTransactionDetails                                  
  SELECT Id, PricePerTicket, EventTicketAttributeId, TicketTypeId                                  
  FROM TransactionDetails WITH (NOLOCK) WHERE TransactionId = @EventTransID                                  
                                  
                                  
  CREATE TABLE #tblTransaction(VenueId INT, VMCC_Id BIGINT,EventCatName NVARCHAR(500),EventCatID BIGINT,                                  
  EventName NVARCHAR(500), EventId BIGINT, GateOpenTime NVARCHAR(100),Match_No INT, MatchDay INT,LocalPricePerTic DECIMAL(18,2),                                  
  SponsorOrCustomerName NVARCHAR(500),       
  Stadium_Name NVARCHAR(500),Stadium_city NVARCHAR(500), EventStarttime NVARCHAR(10), EventEndtime NVARCHAR(10),                                   
  VenueCatName NVARCHAR(500), ISPrintX INT, Barcode NVARCHAR(500), EventStartDate NVARCHAR(100),                                  
  CurrencyName NVARCHAR(10), Stand NVARCHAR(500), Entrance NVARCHAR(500), Gate_No NVARCHAR(100), RampNo NVARCHAR(100),                                  
  Row_No NVARCHAR(50), Tic_No NVARCHAR(50), IsChild INT, IsSRCitizen INT, VenueCatId INT,                                  
  IsWheelChair INT, IsFamilyPkg INT,IsLayoutAvail INT, TicketHtml NVARCHAR(100), RoadName NVARCHAR(500),StaircaseNo NVARCHAR(100),                                  
  PricePerTic DECIMAL(18,2), PrintCount INt)                                  
                                  
  INSERT INTO #tblTransaction                                  
  SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,                                  
  EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, 0 AS LocalPricePerTic, T.Firstname +' ' + T.Lastname AS SponsorOrCustomerName,                                  
  VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime,                                   
  CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,                       
  MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,                                  
  ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,                                  
  SUBSTRING( ISNULL(SeatTag,''),0,PATINDEX('%-%',SeatTag))AS Row_No, SUBSTRING( ISNULL(SeatTag,''),PATINDEX('%-%',SeatTag)+1,LEN(SeatTag))                                  
  AS Tic_No,                                   
  CASE WHEN MSTD.TicketTypeId =2 THEN 1 ELSE 0 END AS IsChild, CASE WHEN MSTD.TicketTypeId =3 THEN 1 ELSE 0 END AS IsSRCitizen, TC.Id AS VenueCatId,                                  
  0 AS IsWheelChair, 0 AS  IsFamilyPkg, 0 AS IsLayoutAvail, EA.TicketHtml AS TicketHtml,                                   
  ISNULL(EG.StreetInformation,'') AS RoadName,                                  
  ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo,                                  
  0 AS PricePerTic, PrintCount                                  
  FROM Transactions T  WITH(NOLOCK)                                 
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
  WHERE T.Id = @EventTransID AND T.TransactionStatusId =8 AND PrintStatusId <>2                                  
  ORDER BY ED.StartDateTime, ED.Id DESC                      
                                  
  UPDATE #tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket                                  
  FROM #tblTransaction A                                  
  INNER JOIN #tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId                                  
  WHERE TicketTypeId = 1                                  
                                
  UPDATE #tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket                                  
  FROM #tblTransaction A                                  
  INNER JOIN #tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId                                  
  WHERE IsChild  = 1 AND TicketTypeId = 2  
                                  
  UPDATE #tblTransaction SET PricePerTic = B.PricePerTicket, LocalPricePerTic = B.PricePerTicket                                 
  FROM #tblTransaction A                                  
  INNER JOIN #tblTransactionDetails B ON A.VMCC_Id = B.EventTicketAttributeId           
  WHERE IsSRCitizen  = 1 AND TicketTypeId = 3                                  
                        
  SELECT * FROM #tblTransaction                                  
  ORDER BY EventStartDate, EventId DESC                                  
    
  DECLARE @AltId NVARCHAR(500)                                  
  IF(@UserId=0)                                  
  BEGIN                                  
   SELECT  @AltId = CreatedBy FROM Transactions  WITH(NOLOCK) WHERE Id = @EventTransID                                  
  END                                  
  ELSE                                  
  BEGIN                                  
   SELECT TOP 1 @AltId = Altid FROM USers  WITH(NOLOCK) WHERE Id = @UserId                                  
  END                                  
                                  
  UPDATE MatchSeatTicketDetails             
  SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETUTCDATE(), PrintedBy =@AltId                                  
  WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL                                  
                                
  --Declare @EventTransID BigInt=210001284                                
                                
  SELECT FirstName +' '+ LastName AS SponsorOrCustomerName,FirstName +' '+ LastName AS SponsorName,  PhoneCode +' - '+ PhoneNumber as CustomerMobile,                                
  (Select PaymentType From BO_RetailCustomer WITH(NOLOCK) where Trans_Id=@EventTransID ) as PaymentType ,      
  Isnull((Select Cust_IdType +' ' + Cust_IdTypeNumber From BO_RetailCustomer  WITH(NOLOCK) where       
  Trans_Id=@EventTransID ),'') As IdTypeNumber,                            
       
  (Select FirstName +' '+ LastName from  Users  WITH(NOLOCK)  where Id       
  In(Select Retailer_Id From BO_RetailCustomer  WITH(NOLOCK) where Trans_Id=@EventTransID )) as UserName,                                
  CreatedUtc,Id FROM Transactions  WITH(NOLOCK) WHERE Id = @EventTransID                                  
                              
  Select Name,IsNull(PhoneCode,'91')+'-'+MobileNumber as MobileNumber,Email,IdType+' '+IdNumber as IdTypeNumber       
  from VisitorDetails WITH(NOLOCK) where  TransactionId=@EventTransID         
  --DROP TABLE #tblTicNumId  
 END           
END