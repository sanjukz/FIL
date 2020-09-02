CREATE PROCEDURE [dbo].[KITMS_GetTournamentVenues] -- 1696,7
(
	@EventCatId  BIGINT=1903,
	@UserId BIGINT=3
)
AS
BEGIN
	SELECT DISTINCT V.Id AS VenueId, V.Name +', '+ C.Name AS EventVenue
	FROM Events E WITH (NOLOCK)
	INNER JOIN EventDetails ED WITH (NOLOCK) ON E.Id=ED.EventId
	INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId
	INNER JOIN UserVenueMappings UVM WITH (NOLOCK) ON ED.VenueId = UVM.VenueId
	INNER JOIN Cities C WITH (NOLOCK) ON C.Id = V.CityId
	WHERE E.Id = @EventCatId AND UVM.UserId = @UserId --AND ED.IsEnabled = 1  AND V.IsEnabled = 1
	AND UVM.IsEnabled = 1

END

