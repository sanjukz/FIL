CREATE PROCEDURE [dbo].[KITMS_GetAvailableCategorySponsors_New]
(
	@EventId BIGINT,
	@CategoryId BIGINT
)
AS
BEGIN

	SELECT MS.Id AS SponsorId, MS.SponsorName FROM Sponsors MS WITH(NOLOCK)                                                     
	INNER JOIN EventSponsorMappings S WITH(NOLOCK) ON MS.Id = S.SponsorId                                                    
	WHERE S.EventDetailId = @EventId and MS.IsEnabled=1                                                    
	AND  MS.Id NOT IN (SELECT ST.SponsorId FROM CorporateTicketAllocationDetails ST WITH(NOLOCK) WHERE 
	EventTicketAttributeId = @CategoryId and ST.IsEnabled = 1) ORDER BY MS.SponsorName 

END 