CREATE PROC [dbo].[KITMS_CheckExistingSponsor]
(
	@SponsorName VARCHAR(500)
)
AS
BEGIN
	SELECT * FROM Sponsors WITH(NOLOCK) WHERE SponsorName=@SponsorName
END