CREATE proc [dbo].[KITMS_GetallCountries]        
AS        
BEGIN        
	SELECT Id AS CountryId,Name AS CountryName 
	FROM Countries WITH(NOLOCK) WHERE IsEnabled=1
	ORDER BY CountryName       
END