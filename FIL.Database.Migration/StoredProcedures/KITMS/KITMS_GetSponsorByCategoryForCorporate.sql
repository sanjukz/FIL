CREATE PROCEDURE [dbo].[KITMS_GetSponsorByCategoryForCorporate] --143
(
	@CorpMapId BIGINT
)
AS
BEGIN
	DECLARE @EventId BIGINT, @VMCC_Id BIGINT

	SELECT @EventId=ETD.EventdetailId, @VMCC_Id=CRD.EventTicketAttributeId
	FROM CorporateRequests CR  WITH(NOLOCK)
	INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id
	INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
	WHERE CRD.Id = @CorpMapId

	SELECT KM.Id AS KM_Id,SM.SponsorName+' ( '+CONVERT(NVARCHAR(10),RemainingTickets)+' )' AS SponsorName, KM.SponsorId
	FROM Sponsors SM WITH(NOLOCK) 
	INNER JOIN  CorporateTicketAllocationDetails KM WITH(NOLOCK) ON KM.SponsorId=SM.Id
	WHERE EventTicketAttributeId=@VMCC_Id AND KM.isEnabled=1 AND KM.RemainingTickets>0
	ORDER BY SponsorName

END