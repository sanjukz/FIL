CREATE PROCEDURE [dbo].[BO_GetPaymentMethod]  --210341      
(      
	@RetailerId BIGINT      
)      
AS      
BEGIN      
	SELECT 1 AS Id,'Cash' AS PaymentMethod
	UNION
	SELECT 2 AS Id,'Card' AS PaymentMethod
	UNION
	SELECT 3 AS Id,'Cheque' AS PaymentMethod      
END    