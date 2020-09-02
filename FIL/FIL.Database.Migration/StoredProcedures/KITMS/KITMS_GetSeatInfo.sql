CREATE PROCEDURE [dbo].[KITMS_GetSeatInfo]   -- 3450, 272, 2125
(                                                                
  @EventIds VARCHAR(MAX)='3450',                                            
  @VenueCatId BIGINT = 272,      
  @SponsorId INT   = 2125                                        
)                                            
AS                                            
BEGIN    
     
DECLARE  @tblEvent  TABLE                  
(        
 EventId BIGINT,       
 VmccId BIGINT    
)                    
    
INSERT INTO @tblEvent    
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A WITH(NOLOCK)     
INNER JOIN EventTicketDetails B WITH(NOLOCK) ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId      
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))       
    
DECLARE @EventId BIGINT                      
DECLARE @VmccId BIGINT    
DECLARE @BlockTickets INT    
DECLARE @AvailableTickets BIGINT    
DECLARE @KMId BIGINT, @EventName VARCHAR(500) , @AvailableTic INT                    
    
DECLARE @tblSeats TABLE    
(    
  EventName VARCHAR(500),    
  AvailableSeats INT,    
  BlockSeats INT    
)    
    
DECLARE curEvent CURSOR FOR                                       
SELECT * FROM @tblEvent    
                   
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN                    
   SELECT @KMId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId      
    AND SponsorId=@SponsorId AND IsEnabled = 1                   
  SELECT @EventName = ED.Name FROM EventDetails ED WITH(NOLOCK) WHERE ED.Id= @EventId     
  SELECT @AvailableTic=ISNULL(RemainingTickets,0) FROM  CorporateTicketAllocationDetails WITH(NOLOCK) where Id= @KMId     
       
  SELECT @BlockTickets = COUNT(A.Id) FROM MatchSeatTicketDetails A WITH(NOLOCK)
  INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK)  ON A.MatchLayoutSectionSeatId = B.Id
  INNER JOIN EventTicketDetails C WITH(NOLOCK) ON A.EventTicketDetailId = C.Id
  INNER JOIN EventTicketAttributes D WITH(NOLOCK) ON D.EventTicketDetailId = C.Id  
  WHERE A.SponsorId = @SponsorId  AND D.Id = @VmccId AND B.SeatTypeId= 3 AND A.TransactionId IS NULL
      
  INSERT INTO @tblSeats    
  SELECT @EventName, @AvailableTic, @BlockTickets    
      
FETCH NEXT FROM curEvent INTO @VmccId, @EventId     
END    
CLOSE curEvent;                     
DEALLOCATE curEvent;     
    
SELECT * FROM @tblSeats    
END 


