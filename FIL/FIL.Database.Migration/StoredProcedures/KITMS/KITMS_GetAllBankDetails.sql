CREATE proc [dbo].[KITMS_GetAllBankDetails]      
AS      
BEGIN 
	SELECT  A.Id AS Id, Name + ' - ' + BankDetail AS BankDetails       
	FROM BankDetails A WITH(NOLOCK) 
	INNER JOIN Countries C WITH(NOLOCK) ON A.CountryId = C.Id
	WHERE A.IsEnabled=1 AND IsIntermediaryBank =0
	ORDER BY Name
END