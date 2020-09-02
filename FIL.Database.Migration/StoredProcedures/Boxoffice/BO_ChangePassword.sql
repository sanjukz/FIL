CREATE PROC [dbo].[BO_ChangePassword]        
(        
	@UserId BIGINT,        
	@CurrPassword NVARCHAR(250),      
	@NewPassword NVARCHAR(250),      
	@retValue INT OUTPUT       
)        
AS         
BEGIN       
    
IF EXISTS(SELECT Id FROM Users WITH(NOLOCK)  WHERE Id=@UserId AND Password=@CurrPassword AND IsEnabled = 1)    
BEGIN      
	UPDATE users SET Password=@NewPassword WHERE id=@UserId AND Password=@CurrPassword AND IsEnabled = 1     
	SET @retValue =1      
END     
ELSE    
BEGIN      
	SET @retValue =2      
END 
 END     