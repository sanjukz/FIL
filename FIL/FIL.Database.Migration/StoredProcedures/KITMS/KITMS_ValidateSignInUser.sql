CREATE PROC [dbo].[KITMS_ValidateSignInUser]    
(    
	@UserEmail nvarchar(250),    
	@Password nvarchar(250)    
)    
AS    
BEGIN    
 IF EXISTS(SELECT * FROM Users WITH(NOLOCK) WHERE UserName=@UserEmail AND Password = @Password AND IsEnabled = 1)      
 BEGIN    
   SELECT TOP 1 Id AS KT_UserId,FirstName AS UserFName,LastName AS UserLName, PhoneCode +'-'+ PhoneNumber AS Usermobile,
   UserName AS UserEmail,Password,IsEnabled AS Status,'KyazoongaUser' AS RoleId
   FROM Users WITH(NOLOCK)  WHERE UserName=@UserEmail AND Password = @Password AND IsEnabled = 1 
   ORDER BY Id ASC 
 END    
END 
