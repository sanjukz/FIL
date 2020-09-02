CREATE PROC [dbo].[Web_ValidateSeatAvailable]    
(    
--DECLARE     
	@TicketNumberId VARCHAR(5000)     
)    
AS    
BEGIN    
 IF EXISTS(SELECT Id FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ',')) 
 AND SeatStatusId<>1 AND SponsorId IS NULL)    
 BEGIN    
  SELECT 1    
 END    
 ELSE    
 BEGIN     
  UPDATE MatchSeatTicketDetails SET SeatStatusId = 4, UpdatedUtc = GETUTCDATE() WHERE 
  ID IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ','))    
  SELECT 0     
 END    
 --SELECT 0     
END
