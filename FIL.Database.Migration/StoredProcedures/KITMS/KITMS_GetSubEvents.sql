CREATE PROCEDURE [dbo].[KITMS_GetSubEvents] -- 1696,7
(
	@EventCatId  BIGINT,
	@VenueId INT,
	@UserId BIGINT

)
AS
BEGIN
	SELECT DISTINCT ED.Id AS EventCatId, 
	ED.Name + ' (' + Convert(VARCHAR,StartDateTime ,106) +')' AS EventCatName
	FROM UserVenueMappings UVM WITH(NOLOCK)
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.VenueId = UVM.VenueID
	WHERE UserId = @UserId AND ED.EventId = @EventCatId AND ED.VenueId = @VenueId
END