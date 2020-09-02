CREATE PROCEDURE [dbo].[spGetMasterVenueLayoutSectionSeatMap] --305
(
	@MasterVenueLayoutSectionId INT
)
AS
BEGIN
	SELECT B.Id AS SeatId,B.SeatTag,A.Id AS SectionId, B.RowNumber,B.ColumnNumber, B.RowOrder,B.ColumnOrder,ISNULL(B.SeatTypeId,1) AS SeatType,
	A.IsEnabled AS Status, 0 AS SoldStatus, 0 AS IsPrint, 0 AS IsBlock, B.Id AS TicNUmId, 0 As SponsorId,
	 0 AS IsWheelChair, A.SectionName
	FROM MasterVenueLayoutSections A WITH (NOLOCK)
	INNER JOIN MasterVenueLayoutSectionSeats B WITH (NOLOCK) ON A.Id=B.MasterVenueLayoutSectionId
	WHERE A.Id IN(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterDynamicStadiumSectionDetails WITH(NOLOCK)  
	WHERE MasterDynamicStadiumCoordinateId IN(SELECT DISTINCT MasterDynamicStadiumCoordinateId FROM 
	MasterDynamicStadiumSectionDetails  WITH(NOLOCK) WHERE MasterVenueLayoutSectionId = @MasterVenueLayoutSectionId))
	ORDER BY A.SectionName,B.RowOrder DESC,B.ColumnOrder ASC
END
