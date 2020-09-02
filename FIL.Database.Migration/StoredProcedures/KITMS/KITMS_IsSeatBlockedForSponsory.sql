CREATE PROC  [dbo].[KITMS_IsSeatBlockedForSponsory] --2875,0
(                     
	@SponsorId BIGINT, 
	@retValue INT OUTPUT
)              
AS              
BEGIN
SEt @retValue =0
IF EXISTS(SELECT COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK) WHERE SponsorId = @SponsorId AND SeatStatusId<>2)
BEGIN
	SEt @retValue =1
END
SELECT @retValue
END