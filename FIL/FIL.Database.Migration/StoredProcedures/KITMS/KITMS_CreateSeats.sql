CREATE PROC [dbo].[KITMS_CreateSeats] --210000375    
(
  @EventTransId BIGINT
)
AS
BEGIN
                      
DECLARE @tblEventIds TABLE (Sno INT IDENTITY(1,1), EventId INT)    
INSERT INTO @tblEventIds                                
SELECT EventDetailId FROM TransactionDetails A WITH(NOLOCK)    
INNER JOIN EventTicketAttributes B WITH(NOLOCK) ON A.EventTicketAttributeId = B.Id    
INNER JOIN EventTicketDetails C WITH(NOLOCK) ON B.EventTicketDetailId = C.Id    
WHERE A.Id=@EventTransId    
    
DECLARE @VMCC_Id BIGINT, @NoofTic INT, @PrintCount INT, @Barcode varchar(50), @CounterTic INT, @Counter INT = 1,    
@EventTicketDetailId BIGINT, @SponsorId BIGINT    
DECLARE @tblTics TABLE (Sno INT IDENTITY(1,1), VMCC_Id BIGINT, EventTicketDetailId BIGINT, NoofTic INT, VenueCatName varchar(100), EventStartDate DATETIME)                                    
                            
INSERT INTO @tblTics                   
SELECT  ETA.Id, ETD.Id,SUM(TD.TotalTickets), TC.Name, ED.StartDateTime    
FROM Transactions T WITH(NOLOCK)                        
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId                   
INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id    
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id    
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                   
INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id                  
WHERE T.TransactionStatusId = 8 AND T.Id=@EventTransID                  
GROUP BY ETA.Id,ETD.Id, TD.TotalTickets, TC.Name, ED.StartDateTime                     
                  
SELECT @CounterTic = COUNT(*) FROM @tblTics    
WHILE (@Counter <= @CounterTic)                   
BEGIN                    
              
 SELECT @VMCC_Id = VMCC_Id, @NoofTic = NoofTic, @EventTicketDetailId = EventTicketDetailId FROM @tblTics WHERE Sno=@counter    
 IF NOT EXISTS(SELECT Id FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE TransactionId=@EventTransID AND EventTicketDetailId=@EventTicketDetailId    
 AND IsEnabled=1 AND SeatStatusId=2)                   
 BEGIN                               
   SELECT @SponsorId= SponsorId FROm CorporateTransactionDetails WITH(NOLOCK) WHERE TransactionId= @EventTransId    
   DECLARE @BlockedSeatCount INT = 0, @UnBlockedSeatCount INT = 0    
       
    SELECT @BlockedSeatCount = COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)    
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
 WHERE A.SponsorId = @SponsorId AND B.SeatTypeId =3 AND EventTicketDetailId=@EventTicketDetailId    
    AND A.IsEnabled=1 AND A.SeatStatusId=1    
                  
   IF(@BlockedSeatCount >= @NoOfTic)                  
   BEGIN                  
  UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId                  
  WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)    
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
  WHERE A.SponsorId = @SponsorId AND EventTicketDetailId=@EventTicketDetailId    
  AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (3) ORDER BY B.RowOrder,B.ColumnOrder)    
   END                  
   ELSE                  
   BEGIN                   
    IF(@BlockedSeatCount > 0)                  
    BEGIN                  
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId                  
  WHERE Id IN (SELECT TOP (@BlockedSeatCount) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)    
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
  WHERE A.SponsorId = @SponsorId AND EventTicketDetailId=@EventTicketDetailId    
  AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (3) ORDER BY B.RowOrder,B.ColumnOrder)               
            
        SET @UnBlockedSeatCount = @NoOfTic - @BlockedSeatCount                  
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId                  
  WHERE Id IN (SELECT TOP (@UnBlockedSeatCount) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)    
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
  WHERE EventTicketDetailId=@EventTicketDetailId    
  AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)    
    END                  
    ELSE                  
    BEGIN                  
        UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, SponsorId= @SponsorId                  
  WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)    
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id    
  WHERE  EventTicketDetailId=@EventTicketDetailId    
  AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)                 
    END                  
   END               
 END                  
                   
SELECT @PrintCount =SUM(ISNULL(PrintStatusId, 1))         
 FROM MatchSeatTicketDetails WITH(NOLOCK)                         
 WHERE TransactionId=@EventTransID AND EventTicketDetailId=@EventTicketDetailId AND IsEnabled=1                   
     
 IF(@PrintCount=@NoOfTic)    
 BEGIN                  
   DECLARE @ISSUERCODE VARCHAR(02) = '08', @IDFORLOCATION VARCHAR(02) = '07', @BARCODEEVENTID VARCHAR(04), @GATEID VARCHAR(02)=NULL,                                             
   @TicketType VARCHAR(2) = '01', @DiscountType VARCHAR(2) = '02' , @ChkPricePerTic DECIMAL(18,2) , @EventType INT, @EventID_1 BIGINT                                           
                                
  SELECT @BarCodeEventId = CASE LEN(DATEPART(DD,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(DD,StartDateTime))                   
  ELSE CONVERT(VARCHAR,DATEPART(DD,StartDateTime)) END +                   
  CASE LEN(DATEPART(MM,StartDateTime)) WHEN 1 THEN '0' + CONVERT(VARCHAR,DATEPART(MM,StartDateTime))                   
  ELSE CONVERT(VARCHAR,DATEPART(MM,StartDateTime)) END,                  
  @EventType = EventId, @EventID_1 = A.EventId                                  
  FROM EventDetails A WITH(NOLOCK) 
  INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.Id = B.EventDetailId    
  INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.Id = C.EventTicketDetailId    
  WHERE C.Id = @VMCC_Id    
                    
  DECLARE @tblBarcode TABLE (SrNo INT IDENTITY(1,1), SeatId BIGINT)                   
  INSERT INTO @tblBarcode                  
  SELECT MatchLayoutSectionSeatId FROM MatchSeatTicketDetails WITH(NOLOCK) 
  WHERE EventTicketDetailId= @EventTicketDetailId AND TransactionId = @EventTransID AND SeatStatusId=2    
                    
  DECLARE @CounterBarcode INT, @Counter_Barcode INT = 1, @SeatId BIGINT                  
  SELECT @CounterBarcode = COUNT(*) FROM @tblBarcode                  
  SET @Counter_Barcode = 1                  
                    
  WHILE (@Counter_Barcode <= @CounterBarcode)    
  BEGIN                    
    SELECT @SeatId = SeatId FROM @tblBarcode WHERE SrNo = @Counter_Barcode    
    SET @Barcode = ISNULL(@BarcodeEventId,'01') +  STUFF(convert(varchar(500),(@SeatId)), 1, 0, REPLICATE('0', 6 - LEN(Convert(varchar(500),(@SeatId)))))    
                  
   DECLARE @chkBarcode INT                  
   SELECT @chkBarcode = COUNT(*) FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE MatchLayoutSectionSeatId=@SeatId     
   AND EventTicketDetailId= @EventTicketDetailId  AND TransactionId=@EventTransID AND SeatStatusId=2 AND BarcodeNumber IS NULL                                    
   IF(@chkBarcode > 0)                                    
   BEGIN                  
    UPDATE MatchSeatTicketDetails                          
    SET BarcodeNumber = CONVERT(VARCHAR(30),@Barcode)                                                                                                                       
    WHERE MatchLayoutSectionSeatId=@SeatId     
    AND EventTicketDetailId= @EventTicketDetailId  AND TransactionId=@EventTransID AND SeatStatusId=2 AND BarcodeNumber IS NULL                 
   END                  
                  
   SET @Counter_Barcode = @Counter_Barcode + 1                  
  END                  
 END                   
                   
 SET @Counter = @Counter + 1                  
END    
SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,    
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, TD.PricePerTicket AS LocalPricePerTic,    
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime,     
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,    
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,    
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,    
RowNumber AS Row_No, SeatTag AS Tic_No, TD.PricePerTicket AS PricePerTic, 0 AS IsChild, 0 AS IsSRCitizen, TC.Id AS VenueCatId,    
0 AS IsWheelChair, 0 AS  IsFamilyPkg, ED.Id AS EventId,Convert(INT,Isnull(IsSeatSelection,0)) AS IsLayoutAvail, EA.TicketHtml AS TicketHtml,     
ISNULL(EG.StreetInformation,'') AS RoadName,    
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo    
FROM Transactions T WITH(NOLOCK) 
INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId    
INNER JOIN EventTicketAttributes  ETA WITH(NOLOCK)  ON TD.EventTicketAttributeId = ETA.Id    
INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id    
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id    
INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id    
INNER JOIN EventAttributes  EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId    
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id    
INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id    
INNER JOIN Cities C WITH(NOLOCK) ON VD.CityId = C.Id    
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id    
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId    
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id    
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id    
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id    
WHERE T.Id = @EventTransID AND T.TransactionStatusId =8 AND PrintStatusId <>2    
ORDER BY ED.StartDateTime, ETA.Id    
                                            
 --SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                        
 --INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                           
 --INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId    
 --WHERE  CTD.TransactionId=  @EventTransID    
   
  SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName ,      
 '' AS CustomerMobile,'' AS PaymentType ,'' AS IdTypeNumber,'' AS UserName,T.CreatedUtc,T.Id      
 FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                            
 INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                               
 INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId        
 WHERE  CTD.TransactionId= @EventTransID   
   
 SELECT Name,ISNULL(PhoneCode,'91')+'-'+MobileNumber as MobileNumber,Email,IdType+' '+IdNumber AS 
 IdTypeNumber FROM VisitorDetails WITH(NOLOCK)  WHERE  TransactionId=@EventTransID                                                   
              
                                                                       
UPDATE MatchSeatTicketDetails                                                               
SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETDATE()    
WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL    
    
END     
    