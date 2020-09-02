CREATE PROC [dbo].[BO_UpdateSuccessForBoxOffice]                                                    
(                                                    
	@TransId BIGINT,                                                              
	@PayConfNumber VARCHAR(1000),                                                              
	@ErrMsg VARCHAR(500) = NULL ,                      
	@Flag VARCHAR(50) = 'Default'                                                             
)                                                    
AS                                                              
BEGIN                                                      
	DECLARE @ReturnVal int                     
	UPDATE Transactions SET TransactionStatusId = 8 WHERE Id = @TransId                                                                               
	SET @ReturnVal =1                                      
	SELECT   @ReturnVal                                                    
 END 
