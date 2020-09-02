CREATE PROC spGetSeasonLayout  
(  
@EventTicketDetailId BIGINT = 452123  
)  
AS  
BEGIN  
 DECLARE @TicketCategoryId BIGINT, @EventId BIGINT   
  
 DECLARE @CountSeason INT = 0, @CountSeasonAvailableTic INT = 0      
 CREATE TABLE #TblEventTicketDetailId (EventTicketDetailId BIGINT)  
 CREATE TABLE #TblSeatsFilter (Sno INT IDENTITY(1,1), Id BIGINT, SeatTag VARCHAR(50))    
    
 SELECT @TicketCategoryId = ETD.TicketCategoryId, @EventId = E.Id   
 FROM Events E WITH (NOLOCK)  
 INNER JOIN EventDetails ED WITH (NOLOCK) ON E.Id = ED.EventId  
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.EventDetailId = ED.Id  
 WHERE ETD.Id=@EventTicketDetailId  
  
 INSERT INTO #TblEventTicketDetailId  
 SELECT ETD.Id   
 FROM EventTicketDetails ETD WITH (NOLOCK)  
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.EventTicketDetailId = ETD.Id  
 WHERE ETD.TicketCategoryId = @TicketCategoryId   
 AND ETA.SeasonPackage = 1  
 AND ETD.EventDetailId IN (SELECT Id FROM EventDetails WITH (NOLOCK) WHERE EventId  = @EventId)  
 SELECT @CountSeason = COUNT(EventTicketDetailId) FROM #TblEventTicketDetailId  
  
 SELECT A.Id,  
 A.AltId,  
 A.MatchLayoutSectionSeatId,  
 A.EventTicketDetailId,  
 A.Price,  
 A.ChannelId,  
 A.SeatStatusId,  
 A.BarcodeNumber,  
 A.PrintStatusId,  
 A.PrintCount,  
 A.PrintedBy,  
 A.PrintDateTime,  
 A.EntryCount,  
 A.EntryStatus,  
 A.EntryDateTime,  
 A.CheckedDateTime,  
 A.EntryCountAllowed,  
 A.TicketTypeId,  
 A.IPDetailId,  
 A.IsEnabled,  
 A.CreatedUtc,  
 A.UpdatedUtc,  
 A.CreatedBy,  
 A.UpdatedBy,  
 A.SponsorId,  
 A.TransactionId,  
 A.IsConsumed,  
 A.ConsumedDateTime,  
 A.EntryGateName, B.SeatTag   
 INTO #TblSeats  
 FROM MatchSeatTicketDetails A WITH (NOLOCK)   
 INNER JOIN MatchLayoutSectionSeats B WITH (NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id   
 WHERE EventTicketDetailId = @EventTicketDetailId  
  
 CREATE INDEX IDX_TblTransTickets_EventTransId ON #TblSeats(Id)    
  
 DECLARE @LoopCount INT = 0, @Counter INT = 1      
  
 INSERT INTO #TblSeatsFilter     
 SELECT Id, SeatTag FROM #TblSeats  WHERE SeatStatusId = 1   
 SELECT @LoopCount = COUNT(Id) FROM #TblSeatsFilter    
 WHILE @Counter <= @LoopCount      
 BEGIN      
 SELECT @CountSeasonAvailableTic = COUNT(A.Id)      
 FROM MatchSeatTicketDetails A WITH (NOLOCK)      
 INNER JOIN MatchLayoutSectionSeats B WITH (NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id          
 WHERE A.EventTicketDetailId in (SELECT EventTicketDetailId FROM #TblEventTicketDetailId) AND B.SeatTag IN (SELECT SeatTag FROM #TblSeatsFilter WHERE Sno= @Counter)      
 AND A.SeatStatusId = 1       
  
 IF(@CountSeason <> @CountSeasonAvailableTic)      
 BEGIN          
 UPDATE #TblSeats SET SeatStatusId = 2 WHERE Id = (SELECT Id FROM #TblSeatsFilter WHERE Sno= @Counter)      
 END      
  
 SET @Counter = @Counter + 1     
  
 END      
 SELECT * FROM #TblSeats  
 DROP TABLE #TblEventTicketDetailId  
 DROP TABLE #TblSeats  
 DROP TABLE #TblSeatsFilter  
END