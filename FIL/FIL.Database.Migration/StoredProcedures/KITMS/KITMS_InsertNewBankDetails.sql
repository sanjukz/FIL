CREATE proc [dbo].[KITMS_InsertNewBankDetails]      
(      
	@CountryId int,      
	@CountryName nvarchar(100),      
	@BankDetails nvarchar(max),      
	@UserId bigint,    
	@IsBeneficiary int=0      
)      
AS      
BEGIN 
	DECLARE @AltId VARCHAR(500)
	SELECT @AltId = AltId FROm USers WITH(NOLOCK) WHERE Id  = @UserId
	INSERT INTO BankDetails(BankDetail,CountryId,IsEnabled,CreatedUtc,CreatedBy,IsIntermediaryBank)      
	VALUES(@BankDetails,@CountryId,1,GETUTCDATE(), @AltId, @IsBeneficiary)      
END