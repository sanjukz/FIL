CREATE PROC [dbo].[KITMS_CheckSeatAvailbility] --'4657021,4657022,4657023,4657024,4657025',0,0
(             
	@Seats TEXT,                      
	@EventId BIGINT, 
	@retValue INT OUTPUT
)              
AS              
BEGIN
DECLARE @SeatCount INT = 0, @SeatCountTmp INT =0
SELECT @SeatCount = COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)
INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
WHERE A.SponsorId IS NULL AND B.Id IN(SELECT * FROM dbo.SplitString(@Seats,',')) AND B.SeatTypeId =1
AND A.SeatStatusId<>2

SELECT @SeatCountTmp =COUNT(*) FROM dbo.SplitString(@Seats,',')
IF(@SeatCount=@SeatCountTmp)
  BEGIN
    SET @retValue = 1
  END         
  ELSE
  BEGIN
	SET @RetValue =0
  END
 SELECT @RetValue   
END 