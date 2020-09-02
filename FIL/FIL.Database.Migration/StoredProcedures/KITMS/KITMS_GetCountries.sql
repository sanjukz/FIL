CREATE PROC [dbo].[KITMS_GetCountries]  
AS  
BEGIN 
	SELECT ID AS CountryId, Name AS CountryName, IsoAlphaTwoCode AS CountryCode, Phonecode AS Country_Code,
	Numcode AS AreaCode
	FROM Countries WITH(NOLOCK) WHERE IsEnabled =1
END