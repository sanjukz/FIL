CREATE PROC [dbo].[ITKTS_ShowSeatsForReprintKITMSUser] --210011580,'4168516'                
(                
	 @Transid BIGINT,                
	 @TicNumIds VARCHAR(MAX)                
)                 
AS                 
BEGIN       
      
SELECT DISTINCT VD.Id As VenueId, ETA.Id AS VMCC_Id, E.Name AS EventCatName, E.Id AS EventCatID, ED.Name AS EventName, ED.Id AS EventId,      
EA.GateOpenTime AS GateOpenTime,EA.MatchNo AS Match_No, EA.MatchDay AS MatchDay, 
CONVERT(DECIMAL(18,2),TD.PricePerTicket) AS LocalPricePerTic,      
VD.Name AS Stadium_Name, C.Name AS Stadium_city, CONVERT(VARCHAR(5),CAST(StartDateTime AS TIME)) AS EventStarttime,       
CONVERT(VARCHAR(5),CAST(EndDateTime AS TIME))  AS EventEndtime, TC.Name AS VenueCatName, ISNULL(MLS.IsPrintBigX,0) AS ISPrintX,      
MSTD.BarcodeNumber AS Barcode, ED.StartDateTime AS EventStartDate,CT.Code As CurrencyName, TC.Name AS Stand,      
ISNULL(EG.StreetInformation,'') AS Entrance, EG.name AS Gate_No,MLS.RampNumber AS RampNo,      
RowNumber AS Row_No, SeatTag AS Tic_No, CONVERT(DECIMAL(18,2),TD.PricePerTicket) AS PricePerTic, 0 AS IsChild, 0 AS IsSRCitizen, TC.Id AS VenueCatId,      
0 AS IsWheelChair, 0 AS  IsFamilyPkg, ED.Id AS EventId, Convert(INT,Isnull(IsSeatSelection,0)) AS IsLayoutAvail, EA.TicketHtml AS TicketHtml,       
ISNULL(EG.StreetInformation,'') AS RoadName,      
ISNULL(MLS.StaircaseNumber,'') AS StaircaseNo      
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
WHERE T.Id = @Transid AND T.TransactionStatusId =8 --AND PrintStatusId <>2               
AND MSTD.Id  IN (SELECT Keyword from dbo.SplitString(@TicNumIds,','))      
ORDER BY ED.StartDateTime, ETA.Id      
                
 --SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                          
 --INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                             
 --INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId      
 --WHERE  CTD.TransactionId=  @Transid      
     
  SELECT  SM.SponsorName AS SponsorOrCustomerName, SM.SponsorName AS SponsorName ,      
 '' as CustomerMobile,'' as PaymentType ,'' As IdTypeNumber,'' as UserName,T.CreatedUtc,T.Id      
 FROM  CorporateTransactionDetails CTD  WITH(NOLOCK)                                                                                            
 INNER JOIN Transactions T WITH(NOLOCK) ON CTD.TransactionId = T.Id                                                                                               
 INNER JOIN  Sponsors SM WITH(NOLOCK) ON  SM.Id =  CTD.SponsorId        
 WHERE  CTD.TransactionId=  @Transid       
         
SELECT Name,ISNULL(PhoneCode,'91')+'-'+MobileNumber as MobileNumber,Email,IdType+' '+IdNumber as IdTypeNumber
FROM VisitorDetails WITH(NOLOCK) WHERE  TransactionId=@Transid       
                
END 
