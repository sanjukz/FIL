CREATE PROC [dbo].[KITMS_CheckPostalCode]
(
	@PostalCode VARCHAR(100)
)
AS
BEGIN
	SELECT * FROM Zipcodes WITH(NOLOCK) WHERE Postalcode = @PostalCode
END