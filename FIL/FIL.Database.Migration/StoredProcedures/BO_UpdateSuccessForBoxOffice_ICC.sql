CREATE PROC [dbo].[BO_UpdateSuccessForBoxOffice_ICC]                                                        
(                                                        
 @TransId BIGINT,                                                                  
 @PayConfNumber VARCHAR(1000),                                                                  
 @ErrMsg VARCHAR(500) = NULL ,                          
 @Flag VARCHAR(50) = 'Default'                                                                 
)                                                        
AS                                                                  
BEGIN                                                          
 DECLARE @ReturnVal int,@AltId uniqueidentifier;                         
 UPDATE Transactions SET TransactionStatusId = 8 WHERE Id = @TransId                                                                                   
   
 --DECLARE @ReturnVal int,@AltId uniqueidentifier;       
 SET @ReturnVal =1   
 SET @AltId =(SELECT AltId from   Transactions WITH(NOLOCK) WHERE Id =@TransId)  
 SELECT  @ReturnVal as RetVal,@AltId as TranscationAltId                                        
  
END 