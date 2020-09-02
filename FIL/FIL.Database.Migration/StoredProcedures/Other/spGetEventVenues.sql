CREATE PROCEDURE [dbo].[spGetEventVenues]
(
	@EventCatId  BIGINT,
	@UserId INT=0
)
AS
BEGIN
	IF (@UserId=0)
	BEGIN
		SELECT DISTINCT VD.Id AS VenueId ,  ISNULL(VD.Name,'') + ', '+  ISNULL(CM.Name,'')  AS EventVenue
		FROM  EventDetails ED WITH(NOLOCK)
		INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId=VD.Id
		INNER JOIN Cities CM WITH(NOLOCK) ON VD.CityId=CM.Id
		WHERE ED.EventId = @EventCatId AND ED.IsEnabled =1
	END
	ELSE
		BEGIN
			SELECT DISTINCT VD.Id AS VenueId ,  ISNULL(VD.Name,'') + ', '+  ISNULL(CM.Name,'')  AS EventVenue
			FROM  EventDetails ED WITH(NOLOCK)
			INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId=VD.Id
			INNER JOIN Cities CM WITH(NOLOCK) ON VD.CityId=CM.Id
			INNER JOIN UserVenueMappings UV WITH(NOLOCK) ON UV.VenueId=VD.Id
			WHERE ED.EventId = @EventCatId
	END
END
