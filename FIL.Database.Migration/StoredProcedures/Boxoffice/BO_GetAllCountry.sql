CREATE proc [dbo].[BO_GetAllCountry]      
AS              
BEGIN
	SELECT Id AS CountryId, Name AS CountryName, Name+'('+CONVERT(VARCHAR(500),Phonecode)+')' AS CountryCode, 
	Phonecode AS Country_Code,'' AS AreaCode FROM Countries WITH(NOLOCK) 
END 