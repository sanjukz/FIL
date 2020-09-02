CREATE PROCEDURE [dbo].[spGetSeasonSeatLayout]  --16859  
(                 
 --DECLARE       
 @EventTicketAttributeId bigint= 16859  
)             
AS             
BEGIN      
 DECLARE @VenueId INT, @TicketCategoryId INT, @EventId BIGINT  
  
 SELECT @VenueId = C.VenueId, @TicketCategoryId = TicketCategoryId, @EventId = C.EventId  
 FROM EventTicketAttributes A WITH (NOLOCK)  
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id AND A.Id= @EventTicketAttributeId  
 INNER JOIN EventDetails C WITH(NOLOCK) ON B.EventDetailId = C.Id  
     
 DECLARE @CountSeasonVmcc INT = 0, @CountSeasonAvailableTic INT = 0      
 CREATE TABLE #TblVmccId (EventTicketAttributeId BIGINT)   
  
 CREATE TABLE #TblSeats (Sno INT IDENTITY(1,1), SeatId BIGINT, SeatTag VARCHAR(50), SectionId INT, RowNumber VARCHAR(50), ColumnNumber VARCHAR(50),   
 RowOrder INT, ColumnOrder INT, SeatType INT, Status INT, EventTicketAttributeId BIGINT, SoldStatus INT, IsPrint INT, IsBlock INT, TicNUmId BIGINT,   
 SponsorId INT, SectionName VARCHAR(500), IsWheelChair INT, IsOtherArea INT)   
   
 CREATE TABLE #TblSeasonSeats (Sno INT IDENTITY(1,1), SeatId BIGINT, SeatTag VARCHAR(50), SectionId INT, RowNumber VARCHAR(50), ColumnNumber VARCHAR(50),   
 RowOrder INT, ColumnOrder INT, SeatType INT, Status INT, EventTicketAttributeId BIGINT, SoldStatus INT, IsPrint INT, IsBlock INT, TicNUmId BIGINT,   
 SponsorId INT, SectionName VARCHAR(500), IsWheelChair INT)    
  
 CREATE TABLE #TblSeatsFilter (Sno INT IDENTITY(1,1), TicNumId BIGINT, SeatTag VARCHAR(50))    
  
 INSERT INTO #TblVmccId  
 SELECT DISTINCT A.Id FROM EventTicketAttributes A WITH (NOLOCK)  
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id  
 INNER JOIN EventDetails C WITH(NOLOCK) ON B.EventDetailId = C.Id  
 WHERE SeasonPackage = 1 AND C.EventId = @EventId AND C.VenueId = @VenueId AND B.TicketCategoryId = @TicketCategoryId  
   
 SELECT @CountSeasonVmcc = COUNT(EventTicketAttributeId) FROM #TblVmccId  
       
 INSERT INTO #TblSeats        
 SELECT  A.MatchLayoutSectionSeatId AS SeatId,B.SeatTag,C.Id AS SectionId, B.RowNumber,B.ColumnNumber, ISNULL(B.RowOrder,B.RowNumber) As RowOrder,    
 ISNULL(B.ColumnOrder,B.ColumnNumber) AS ColumnOrder,   
 ISNULL(B.SeatTypeId,1) AS SeatType, A.IsEnabled AS Status, G.Id AS EventTicketAttributeId,    
 CASE WHEN A.SeatStatusId =2 THEN 1 ELSE 0 END AS SoldStatus, CASE WHEN ISNULL(A.PrintStatusId,0)=2 THEN 1 ELSE 0 END AS IsPrint,     
 CASE WHEN B.SeatTypeId = 3 THEN 1 ELSE 0 END AS IsBlock, A.Id AS TicNUmId, ISNULL(A.SponsorId,0) AS SponsorId, C.SectionName, 0 AS IsWheelChair,  
 0 AS IsOtherArea    
 FROM MatchSeatTicketDetails A WITH (NOLOCK)    
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id                            
 INNER JOIN MatchLayoutSections C WITH(NOLOCK) ON B.MatchLayoutSectionId=C.Id                            
 INNER JOIN EventTicketDetails D WITH(NOLOCK) ON A.EventTicketDetailId = D.Id    
 INNER JOIN EventTicketAttributes G WITH(NOLOCK) ON D.Id = G.EventTicketDetailId    
 INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id    
 WHERE G.Id=@EventTicketAttributeId  
 ORDER BY ISNULL(B.RowOrder,B.RowNumber) DESC,    
 ISNULL(B.ColumnOrder,B.ColumnNumber) ASC   
   
  INSERT INTO #TblSeasonSeats        
 SELECT  A.MatchLayoutSectionSeatId AS SeatId,B.SeatTag,C.Id AS SectionId, B.RowNumber,B.ColumnNumber, ISNULL(B.RowOrder,B.RowNumber) As RowOrder,    
 ISNULL(B.ColumnOrder,B.ColumnNumber) AS ColumnOrder,   
 ISNULL(B.SeatTypeId,1) AS SeatType, A.IsEnabled AS Status, G.Id AS EventTicketAttributeId,    
 CASE WHEN A.SeatStatusId =2 THEN 1 ELSE 0 END AS SoldStatus, CASE WHEN ISNULL(A.PrintStatusId,0)=2 THEN 1 ELSE 0 END AS IsPrint,     
 CASE WHEN B.SeatTypeId = 3 THEN 1 ELSE 0 END AS IsBlock, A.Id AS TicNUmId, ISNULL(A.SponsorId,0) AS SponsorId, C.SectionName, 0 AS IsWheelChair    
 FROM MatchSeatTicketDetails A WITH (NOLOCK)    
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id         
 INNER JOIN MatchLayoutSections C WITH(NOLOCK) ON B.MatchLayoutSectionId=C.Id                            
 INNER JOIN EventTicketDetails D WITH(NOLOCK) ON A.EventTicketDetailId = D.Id    
 INNER JOIN EventTicketAttributes G WITH(NOLOCK) ON D.Id = G.EventTicketDetailId    
 INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id    
 WHERE G.Id In((SELECT EventTicketAttributeId FROM #TblVmccId))  
 ORDER BY G.Id ASC, ISNULL(B.RowOrder,B.RowNumber) DESC,    
 ISNULL(B.ColumnOrder,B.ColumnNumber) ASC       
   
 CREATE INDEX IDX_TblSeats_TicNumId ON #TblSeats(TicNumId)       
 CREATE INDEX IDX_TblSeasonSeats_TicNumId ON #TblSeasonSeats(TicNumId)  
 CREATE NONCLUSTERED INDEX IDX_TblSeasonSeats_SeatTag ON #TblSeasonSeats(SeatTag)  
       
 DECLARE @LoopCount INT = 0, @Counter INT = 1  
     
 INSERT INTO #TblSeatsFilter     
 SELECT TicNumId, SeatTag FROM #TblSeats  WHERE SeatType = 1 AND IsBlock = 0 AND Status = 1 AND SoldStatus = 0    
 SELECT @LoopCount = COUNT(TicNumId) FROM #TblSeatsFilter   
 --SET @LoopCount =2  
 --SET @Counter=1  
 WHILE @Counter <= @LoopCount      
 BEGIN  
  DECLARE @IsBlock INT = 0, @SoldStatus INT = 0, @IsPrint INT = 0, @SeatTag VARCHAR(200), @TicNumId BIGINT, @SeatType INT  
  SELECT @SeatTag = SeatTag, @TicNumId = TicNumId FROM #TblSeatsFilter WHERE Sno= @Counter  
  
  SELECT @IsBlock = ISNULL(COUNT(*),0)  
  FROM #TblSeasonSeats WHERE SeatTag = @SeatTag AND IsBlock = 1  
  
  SELECT @SoldStatus = ISNULL(COUNT(*),0)  
  FROM #TblSeasonSeats WHERE SeatTag = @SeatTag AND SoldStatus = 1  
  
  SELECT @SoldStatus = ISNULL(COUNT(*),0)  
  FROM #TblSeasonSeats WHERE SeatTag = @SeatTag AND SoldStatus = 1  
  
  SELECT @SeatType = ISNULL(COUNT(*),0)  
  FROM #TblSeasonSeats WHERE SeatTag = @SeatTag AND SeatType <> 1  
  --   SELECT *   
  --FROM #TblSeasonSeats WHERE SeatTag = @SeatTag  
  --SELECT @SeatType  
  
 UPDATE #TblSeats SET SoldStatus = CASE WHEN @SoldStatus> 0 THEn 1 ELSE 0 END,   
 IsBlock = CASE WHEN @IsBlock > 0 THEn 1 ELSE 0 END,  
 IsPrint = CASE WHEN @IsPrint > 0 THEn 1 ELSE 0 END,  
 IsOtherArea = CASE WHEN @SeatType > 0 THEn 1 ELSE 0 END  
 WHERE TicNumId = @TicNumId     
 --SELECT @CountSeasonVmcc, @CountSeasonAvailableTic, @Counter    
   
      
 SET @Counter = @Counter + 1     
     
 END      
 SELECT * FROM #TblSeats      
 DROP TABLE #TblSeats  
 DROP TABLE #TblVmccId    
 DROP TABLE #TblSeatsFilter  
 DROP TABLE  #TblSeasonSeats  
  
END 