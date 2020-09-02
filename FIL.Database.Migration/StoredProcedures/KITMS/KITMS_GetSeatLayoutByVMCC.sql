CREATE PROCEDURE [dbo].[KITMS_GetSeatLayoutByVMCC] --90455
(
	@VmccId INT
)
AS
BEGIN

	SELECT  A.MatchLayoutSectionSeatId AS SeatId,B.SeatTag,C.Id AS SectionId, B.RowNumber,B.ColumnNumber, 
	ISNULL(B.SeatTypeId,1) AS SeatType, A.IsEnabled AS Status,
	CASE WHEN A.SeatStatusId =2 THEN 1 ELSE 0 END AS SoldStatus, CASE WHEN ISNULL(A.PrintStatusId,0)=2 THEN 1 ELSE 0 END AS IsPrint, 
	CASE WHEN B.SeatTypeId = 3 THEN 1 ELSE 0 END AS IsBlock, A.Id AS TicNUmId, ISNULL(A.SponsorId,0) AS SponsorId,
	 0 AS IsWheelChair, C.SectionName
	FROM MatchSeatTicketDetails A WITH (NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id                        
	INNER JOIN MatchLayoutSections C WITH(NOLOCK) ON B.MatchLayoutSectionId=C.Id                        
	INNER JOIN EventTicketDetails D WITH(NOLOCK) ON A.EventTicketDetailId = D.Id
	INNER JOIN EventTicketAttributes G WITH(NOLOCK) ON D.Id = G.EventTicketDetailId
	INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id
	WHERE G.Id=@VmccId
	ORDER BY A.MatchLayoutSectionSeatId ASC
END