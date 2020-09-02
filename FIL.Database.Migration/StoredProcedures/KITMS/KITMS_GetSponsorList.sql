CREATE PROC [dbo].[KITMS_GetSponsorList] 
(
	@SponsorName VARCHAR(500)
)
AS
BEGIN
	SELECT Id AS SponsorId, SponsorName FROM Sponsors WITH(NOLOCK) WHERE UPPER(SponsorName) LIKE '%'+UPPER(@SponsorName)+'%' AND IsEnabled=1
END