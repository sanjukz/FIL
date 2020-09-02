CREATE Proc [dbo].[KITMS_GetSeatAvailability] --9997           
(            
	@KITMSMasterId bigint       
)            
AS             
BEGIN   
	DECLARE @SponsorId BIGINT, @Vmcc_Id BIGINT, @EventTicketDetailId BIGINT
	SELECT @SponsorId=SponsorId, @Vmcc_Id=EventTicketAttributeId FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId
	SELECT @EventTicketDetailId = EventTicketDetailId FROM EventTicketAttributes WITH(NOLOCK) WHERE  Id = @Vmcc_Id
	SELECT RemainingTickets FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KITMSMasterId

	SELECT COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)
	INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) ON A.MatchLayoutSectionSeatId = B.Id
	WHERE A.SponsorId = @SponsorId AND A.EventTicketDetailId = @EventTicketDetailId AND B.SeatTypeId =3
	AND A.SeatStatusId=1
END