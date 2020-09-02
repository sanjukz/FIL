CREATE FUNCTION [dbo].[SeatTagString] --18065  
(  
	@TransactionDetailsId BigINt  
)  
RETURNS varchar(500)  
AS  
BEGIN  
	Declare @SeatTagString AS Nvarchar(MAX)  
	Select  @SeatTagString= COALESCE(@SeatTagString+',','')+SeatTag from MatchLayoutSectionSeats WITH(NOLOCK) 
	where ID In(Select MatchLayoutSectionSeatId From MatchSEatTicketDetails  WITH(NOLOCK) where ID In(  
	Select Distinct MatchSeatTicketDetailId From TransactionSeatDetails  WITH(NOLOCK) where TransactionDetailId=@TransactionDetailsId))  
	return  @SeatTagString  
END