CREATE proc [dbo].[ITKTS_AssignSeatOnTheSpot_18]  --3453473 ,231344                                                                                                           
(                                                                                                                                                      
	@EventTransID BIGINT,           
	@UserId BIGINT                                                                                                                                                     
)
AS
BEGIN
              
DECLARE @totalLoopCount INT, @VMCC_Id BIGINT, @NoofTic INT, @BlockId VARCHAR(100), @TicNumId BIGINT, @AltId NVARCHAR(500)
DECLARE @loopCounter INT = 1, @TotalQty INT, @AssignedQty INT, @EventTicketDetailId BIGINT, @TicketTypeId INT
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
	VenueCatName varchar(100), EventStartDate DATETIME, TicketTypeId INT)
         
	INSERT INTO @tblTics               
	SELECT  ETA.Id, ETD.Id,TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId
	FROM Transactions T  WITH(NOLOCK)                   
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId               
	INNER JOIN EventTicketAttributes ETA  WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id               
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id              
	WHERE T.TransactionStatusId = 8 AND T.Id=@EventTransID              
	GROUP BY ETA.Id,ETD.Id, TD.TotalTickets, TC.Name, ED.StartDateTime, TD.TicketTypeId

	--SELECT * FROm @tblTics

	SELECT @totalLoopCount = COUNT(*) FROM @tblTics               
	SET @loopCounter = 1              
	WHILE (@loopCounter <= @totalLoopCount)              
	BEGIN
		SELECT @VMCC_Id = VMCC_Id, @NoofTic = NoofTic, @EventTicketDetailId = EventTicketDetailId, @TicketTypeId = TicketTypeId
		FROM @tblTics WHERE Sno=@loopCounter
		--SELECT @VMCC_Id, @NoofTic, @EventTicketDetailId

		UPDATE MatchSeatTicketDetails SET SeatStatusId = 2, TransactionId = @EventTransID, TicketTypeId =  @TicketTypeId         
		WHERE Id IN (SELECT TOP (@NoOfTic) A.Id FROM MatchSeatTicketDetails A WITH(NOLOCK)
		INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id AND BarcodeNUmber IS NULL
		WHERE  EventTicketDetailId=@EventTicketDetailId
		AND A.IsEnabled=1 AND A.SeatStatusId=1 AND B.SeatTypeId IN (1) ORDER BY B.RowOrder,B.ColumnOrder)

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
 SELECT Id FROM MatchSeatTicketDetails WHERE TransactionId = @EventTransId  
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
	FROM EventDetails A
	INNER JOIN EventTicketDetails B ON A.Id = B.EventDetailId                       
	INNER JOIN MatchSeatTicketDetails C ON B.Id = C.EventTicketDetailId               
	INNER JOIN MatchLayoutSectionSeats D ON C.MatchLayoutSectionSeatId = D.Id                                
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

  SET @loopCounter = @loopCounter + 1                
 END
 --========================= Bracode assignment Ends here =================================================

SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, TD.PricePerTicket AS LocalPricePerTic,
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime, 
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,
SUBSTRING( ISNULL(SeatTag,''),0,PATINDEX('%-%',SeatTag))AS Row_No, SUBSTRING( ISNULL(SeatTag,''),PATINDEX('%-%',SeatTag)+1,LEN(SeatTag))
 AS Tic_No, TD.PricePerTicket AS PricePerTic, 
CASE WHEN MSTD.TicketTypeId =2 THEN 1 ELSE 0 END AS IsChild, CASE WHEN MSTD.TicketTypeId =3 THEN 1 ELSE 0 END AS IsSRCitizen, TC.Id AS VenueCatId,
0 AS IsWheelChair, 0 AS  IsFamilyPkg, ED.Id AS EventId, 0 AS IsLayoutAvail, EA.TicketHtml AS TicketHtml, 
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
INNER JOIN CurrencyTypes  CT WITH(NOLOCK) ON T.CurrencyId = CT.Id
INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId
INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id
INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id
INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id
WHERE T.Id = @EventTransId AND T.TransactionStatusId =8 AND PrintStatusId <>2
ORDER BY ED.StartDateTime, ETA.Id

SELECT TOP 1 @AltId = Altid FROM USers WHERE Id = @UserId
UPDATE MatchSeatTicketDetails                                                        
SET PrintStatusId=2, PrintCount = PrintCount + 1, PrintDateTime = GETUTCDATE(), PrintedBy =@AltId
WHERE TransactionId=@EventTransID AND BarcodeNumber IS NOT NULL

SELECT FirstName +' '+ LastName AS SponsorOrCustomerName,FirstName +' '+ LastName AS SponsorName
FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransID
END
END
