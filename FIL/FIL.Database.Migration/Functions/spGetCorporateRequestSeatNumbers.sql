CREATE FUNCTION [dbo].[spGetCorporateRequestSeatNumbers]
(
	@CorpMapId BIGINT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @SeatNubmbers NVARCHAR(MAX)=''
	DECLARE @tblSetaNumbers TABLE(srno INT IDENTITY(1,1), SeatTag NVARCHAR(100))

	INSERT INTO @tblSetaNumbers
	SELECT MLSS.SeatTag
	FROM CorporateRequestDetails COM  WITH(NOLOCK) 
	INNER JOIN EventTicketAttributes C  WITH(NOLOCK) ON COM.EventTicketAttributeId=C.Id
	INNER JOIN EventTicketDetails D  WITH(NOLOCK) ON C.EventTicketDetailId=D.Id
	INNER JOIN  MatchSeatTicketDetails TND  WITH(NOLOCK) ON TND.Id IN(SELECT * FROM SPLITSTRING(COM.MatchSeatTicketDetailIds,',')) 
	AND TND.EventTicketDetailId=D.Id
	INNER JOIN MatchLayoutSectionSeats MLSS  WITH(NOLOCK) ON TND.MatchLayoutSectionSeatId=MLSS.Id
	WHERE COM.Id=@CorpMapId

	DECLARE @Count INT, @Counter INT=1, @SeatTag NVARCHAR(100)
	SELECT @Count=COUNT(*) FROM @tblSetaNumbers
	WHILE (@Counter<=@Count)
	BEGIN
		SELECT @SeatTag=SeatTag FROM @tblSetaNumbers WHERE srno=@Counter
		IF(@Counter=1)
		BEGIN
			SET @SeatNubmbers=@SeatTag
		END
		ELSE
		BEGIN
			SET @SeatNubmbers=@SeatNubmbers+','+@SeatTag
		END
		SET @Counter+=1
	END

	RETURN @SeatNubmbers
END