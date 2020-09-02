Alter PROCEDURE [dbo].[spGetBoxofficeSeasonSeatLayout] 
(                       
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
 CREATE TABLE #TableEventTicketAttributeIds (EventTicketAttributeId BIGINT)         
        
 CREATE TABLE #TableSeats (Sno INT IDENTITY(1,1), MatchLayoutSectionSeatId BIGINT, SeatTag VARCHAR(50), MatchLayoutSectionId INT, RowNumber VARCHAR(50), ColumnNumber VARCHAR(50),         
 RowOrder INT, ColumnOrder INT, SeatTypeId smallint, IsEnabled BIT, EventTicketAttributeId BIGINT, SeatStatusId INT, PrintStatusId INT,MatchSeatTicketDetailId BIGINT,         
 SponsorId INT, SectionName VARCHAR(500), IsWheelChair INT)         
         
 CREATE TABLE #TableSeasonSeats (Sno INT IDENTITY(1,1), MatchLayoutSectionSeatId BIGINT, SeatTag VARCHAR(50), MatchLayoutSectionId INT, RowNumber VARCHAR(50), ColumnNumber VARCHAR(50),         
 RowOrder INT, ColumnOrder INT, SeatTypeId smallint, IsEnabled BIT, EventTicketAttributeId BIGINT, SeatStatusId INT,PrintStatusId INT,MatchSeatTicketDetailId BIGINT,         
 SponsorId INT, SectionName VARCHAR(500), IsWheelChair INT)          
        
 CREATE TABLE #TableSeatsFilter (Sno INT IDENTITY(1,1), MatchSeatTicketDetailId BIGINT, SeatTag VARCHAR(50))          
        
 INSERT INTO #TableEventTicketAttributeIds        
 SELECT DISTINCT A.Id FROM EventTicketAttributes A WITH (NOLOCK)        
 INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId = B.Id        
 INNER JOIN EventDetails C WITH(NOLOCK) ON B.EventDetailId = C.Id        
 WHERE SeasonPackage = 1 AND C.EventId = @EventId AND C.VenueId = @VenueId AND B.TicketCategoryId = @TicketCategoryId        
         
 SELECT @CountSeasonVmcc = COUNT(EventTicketAttributeId) FROM #TableEventTicketAttributeIds        
             
 INSERT INTO #TableSeats              
 SELECT  A.MatchLayoutSectionSeatId ,B.SeatTag,C.Id, B.RowNumber,B.ColumnNumber, ISNULL(B.RowOrder,B.RowNumber) As RowOrder,          
 ISNULL(B.ColumnOrder,B.ColumnNumber) AS ColumnOrder,B.SeatTypeId,A.IsEnabled,G.Id AS EventTicketAttributeId,A.SeatStatusId,A.PrintStatusId,A.Id, ISNULL(A.SponsorId,0),C.SectionName, 0 AS IsWheelChair           
 FROM MatchSeatTicketDetails A WITH (NOLOCK)          
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id                                  
 INNER JOIN MatchLayoutSections C WITH(NOLOCK) ON B.MatchLayoutSectionId=C.Id                                  
 INNER JOIN EventTicketDetails D WITH(NOLOCK) ON A.EventTicketDetailId = D.Id          
 INNER JOIN EventTicketAttributes G WITH(NOLOCK) ON D.Id = G.EventTicketDetailId          
 INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id          
 WHERE G.Id=@EventTicketAttributeId        
 ORDER BY ISNULL(B.RowOrder,B.RowNumber) DESC,          
 ISNULL(B.ColumnOrder,B.ColumnNumber) ASC         
         
 INSERT INTO #TableSeasonSeats              
 SELECT  A.MatchLayoutSectionSeatId ,B.SeatTag,C.Id, B.RowNumber,B.ColumnNumber, ISNULL(B.RowOrder,B.RowNumber) As RowOrder,          
 ISNULL(B.ColumnOrder,B.ColumnNumber) AS ColumnOrder,B.SeatTypeId,A.IsEnabled,G.Id AS EventTicketAttributeId,A.SeatStatusId,A.PrintStatusId,A.Id,ISNULL(A.SponsorId,0),C.SectionName, 0 AS IsWheelChair    
 FROM MatchSeatTicketDetails A WITH (NOLOCK)          
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id               
 INNER JOIN MatchLayoutSections C WITH(NOLOCK) ON B.MatchLayoutSectionId=C.Id                                  
 INNER JOIN EventTicketDetails D WITH(NOLOCK) ON A.EventTicketDetailId = D.Id          
 INNER JOIN EventTicketAttributes G WITH(NOLOCK) ON D.Id = G.EventTicketDetailId          
 INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id          
 WHERE G.Id In((SELECT EventTicketAttributeId FROM #TableEventTicketAttributeIds))        
 ORDER BY G.Id ASC, ISNULL(B.RowOrder,B.RowNumber) DESC,          
 ISNULL(B.ColumnOrder,B.ColumnNumber) ASC             
         
 CREATE INDEX IDX_TblSeats_TicNumId ON #TableSeats(MatchSeatTicketDetailId)             
 CREATE INDEX IDX_TblSeasonSeats_TicNumId ON #TableSeasonSeats(MatchSeatTicketDetailId)        
 CREATE NONCLUSTERED INDEX IDX_TblSeasonSeats_SeatTag ON #TableSeasonSeats(SeatTag)        
             
 DECLARE @LoopCount INT = 0, @Counter INT = 1        
           
 INSERT INTO #TableSeatsFilter    
 SELECT MatchSeatTicketDetailId, SeatTag FROM #TableSeats WHERE SeatTypeId = 1  AND IsEnabled = 1 AND SeatStatusId NOT IN(2,4)      
         
 SELECT @LoopCount = COUNT(MatchSeatTicketDetailId) FROM #TableSeatsFilter         
     
 WHILE @Counter <= @LoopCount            
 BEGIN        
      
  DECLARE      
  @SoldSeatStatus INT = 0,     
  @BlockCustomerSeatStatus INT = 0,    
  @BlockSponsorSeatStatus INT = 0,    
  @AvailableSeat INT=0,    
  @KilledSeat INT=0,    
  @BlockedSeat INT=0,    
  @SeatTag VARCHAR(200),     
  @MatchSeatTicketDetailId BIGINT;          
      
  SELECT @SeatTag = SeatTag, @MatchSeatTicketDetailId = MatchSeatTicketDetailId FROM #TableSeatsFilter WHERE Sno= @Counter     
         
  SELECT @SoldSeatStatus = ISNULL(COUNT(*),0)        
  FROM #TableSeasonSeats WHERE SeatTag = @SeatTag AND SeatStatusId = 2      
    
  SELECT @BlockCustomerSeatStatus = ISNULL(COUNT(*),0)        
  FROM #TableSeasonSeats WHERE SeatTag = @SeatTag AND SeatStatusId = 4      
    
  SELECT @BlockSponsorSeatStatus = ISNULL(COUNT(*),0)        
  FROM #TableSeasonSeats WHERE SeatTag = @SeatTag AND IsNUll(SponsorId,0) > 0  And SeatTypeId=3     
      
  SELECT @KilledSeat = ISNULL(COUNT(*),0)        
  FROM #TableSeasonSeats WHERE SeatTag = @SeatTag AND SeatTypeId=2        
    
  SELECT @BlockedSeat = ISNULL(COUNT(*),0)        
  FROM #TableSeasonSeats WHERE SeatTag = @SeatTag AND SeatTypeId=3      
          
 UPDATE #TableSeats     
 SET SeatStatusId =(CASE WHEN @SoldSeatStatus> 0 THEN 2  ELSE CASE WHEN @BlockCustomerSeatStatus>0 THEN 4 ELSE 1 END END),     
 SponsorId =(CASE WHEN @BlockSponsorSeatStatus>0 THEN 1 ELSE 0 END),    
 SeatTypeId=(CASE WHEN @KilledSeat>0 THEN 2 ELSE CASE WHEN @BlockedSeat>0 THEN 3 ELSE 1 END END)      
 WHERE MatchSeatTicketDetailId = @MatchSeatTicketDetailId     
    
 SET @Counter = @Counter + 1     
    
 END     
             
 SELECT * FROM #TableSeats            
 DROP TABLE #TableSeats        
 DROP TABLE #TableEventTicketAttributeIds          
 DROP TABLE #TableSeatsFilter        
 DROP TABLE #TableSeasonSeats      
        
END 