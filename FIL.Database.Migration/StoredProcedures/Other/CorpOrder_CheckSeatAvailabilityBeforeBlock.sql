CREATE PROC [dbo].[CorpOrder_CheckSeatAvailabilityBeforeBlock]
(
--DECLARE 
	@TicketNumberId VARCHAR(5000),
	@retValue INT OUTPUT
)
AS
BEGIN
	IF EXISTS(SELECT A.Id FROM MatchSeatTicketDetails A  WITH(NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B  WITH(NOLOCK)On A.MatchLayoutSectionSeatId = B.Id
	WHERE A.Id IN (SELECT * FROM dbo.SplitString(@TicketNumberId, ',')) AND B.SeatTypeId = 1
	AND SeatStatusId=1)
	BEGIN
		SET @retValue=1 --7304625,7304626
	END
	ELSE
	BEGIN
		SET @retValue=0
	END

	--SET @retValue=0
END