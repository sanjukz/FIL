CREATE PROC [dbo].[KITMS_BlockkSeatsForSponsors]               
(             
	@Seats NVARCHAR(MAX),                      
	@EventId BIGINT,  
	@SponsorId INT  
)              
AS              
BEGIN                          
  
  DECLARE @SponsorType INT = 1
  SELECT @SponsorType = SponsorTypeId FROm Sponsors WITH(NOLOCK) WHERE Id= @SponsorId
  IF(@SponsorType=0)
  BEGIN
	  UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 2 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 1
  END
  ELSE
  BEGIN
	  UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	  UPDATE MatchLayoutSectionSeats SET SeatTypeId= 3 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 1
  END
END