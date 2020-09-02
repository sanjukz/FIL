CREATE PROCEDURE [dbo].[BO_GetCardCharges]      
(      
	@RetailerId Bigint,      
	@EventId BIGINT      
)      
AS      
BEGIN  
	SELECT 0 AS CardCharges
END 