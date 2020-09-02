CREATE PROC [dbo].[BO_RASVAllCountry]          
AS                  
BEGIN   
	SELECT Id AS CountryId, Name AS CountryName, Name+'('+CONVERT(VARCHAR(500),Phonecode)+')' AS CountryCode, 
	Phonecode AS Country_Code,'' AS AreaCode from Countries  WITH(NOLOCK) WHERE Id=13           
	UNION    
	SELECT Id AS CountryId, Name AS CountryName, Name+'('+CONVERT(VARCHAR(500),Phonecode)+')' AS CountryCode, 
	Phonecode AS Country_Code,'' AS AreaCode FROM Countries  WITH(NOLOCK) WHERE ID<>13                    
END   