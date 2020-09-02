CREATE PROC [dbo].[KITMS_GetAllIntermediaryBankDetails]    
AS    
BEGIN    
     
	SELECT A.Id AS Id, Name + ' - ' + BankDetail AS BankDetails       
	FROM BankDetails A WITH(NOLOCK) 
	INNER JOIN Countries C WITH(NOLOCK) ON A.CountryId = C.Id
	WHERE A.IsEnabled=1 AND IsIntermediaryBank =1
	ORDER BY Name
     
END 