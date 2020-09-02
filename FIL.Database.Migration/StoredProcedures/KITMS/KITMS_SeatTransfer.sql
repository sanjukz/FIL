CREATE PROC [dbo].[KITMS_SeatTransfer]          
(          
	@Seats VARCHAR(MAX),          
	@EventId BIGINT,    
	@TransferSponsorId INT        
)          
AS           
BEGIN
	UPDATE MatchSeatTicketDetails SET SponsorId= @TransferSponsorId WHERE 
	MatchLayoutSectionSeatId IN (SELECT * FROM dbo.SplitString(@Seats,',')) AND SeatStatusId=1 
END