CREATE PROC [dbo].[spGetCountries]
AS
BEGIN
	SELECT * FROm Countries WITH(NOLOCK) ORDER By Name ASC
END
