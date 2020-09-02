CREATE PROC [dbo].[KITMS_ReleaseSeats]          
(          
	@Seats VARCHAR(MAX),          
	@EventId BIGINT,    
	@SponsorId INT        
)          
AS           
BEGIN 
	UPDATE MatchSeatTicketDetails SET SponsorId= @SponsorId WHERE MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1
	AND SponsorId = @SponsorId
	UPDATE MatchLayoutSectionSeats SET SeatTypeId= 1 WHERE Id IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatTypeId = 3  
END