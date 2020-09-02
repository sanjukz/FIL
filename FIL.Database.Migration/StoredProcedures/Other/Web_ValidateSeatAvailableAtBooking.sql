CREATE PROC [dbo].[Web_ValidateSeatAvailableAtBooking]      
(      
	@TicketNumberId VARCHAR(5000)       
)      
AS      
BEGIN      
 IF EXISTS(SELECT * FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ',')) and SeatStatusId Not In(4,5,8,2))      
 BEGIN        
  SELECT 1 --7304625,7304626      
 END      
 ELSE      
 BEGIN      
  IF EXISTS(SELECT * FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ',')) AND SponsorId IS NOT NULL )      
  BEGIN      
   SELECT 1      
  END      
  ELSE      
  BEGIN      
   UPDATE MatchSeatTicketDetails SET SeatStatusId =5,  UpdatedUtc = GETUTCDATE() WHERE Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ','))      
   SELECT 0      
  END      
        
 END      
    
END
