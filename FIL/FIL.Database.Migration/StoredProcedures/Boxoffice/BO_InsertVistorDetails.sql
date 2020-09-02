CREATE PROC [dbo].[BO_InsertVistorDetails]                  
(                                        
	@TransId BIGINT,
	@Name NVARCHAR(200),
	@Email NVARCHAR(200), 
	@PhoneCode NVARCHAR(50), 
	@MobileNumber NVARCHAR(100),
	@IdType NVARCHAR(100), 
	@IdNo NVARCHAR(200),  
	@ReturnVal BIGINT OUTPUT
)                                        
AS                                        
BEGIN TRANSACTION   
INSERT INTO VisitorDetails(TransactionId,Name,Email,PhoneCode,MobileNumber,IdType,IdNumber,IsEnabled,
CreatedUtc,CreatedBy)                                        
VALUES(@TransId, @Name,LOWER(@Email), @PhoneCode,@MobileNumber,@IdType,@IdNo,1,GETUTCDATE(),NEWID())                                        
IF (@@ERROR <> 0)                                               
BEGIN                                              
	ROLLBACK TRAN                                                    
	SET @ReturnVal = 0                                            
END                                              
Else                                              
BEGIN                                              
	COMMIT TRANSACTION                                              
	SET @ReturnVal = SCOPE_IDENTITY()                                              
END          