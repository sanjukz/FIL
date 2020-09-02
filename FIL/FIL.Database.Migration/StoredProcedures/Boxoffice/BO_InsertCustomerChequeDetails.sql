CREATE PROC [dbo].[BO_InsertCustomerChequeDetails]     
(                            
	@Trans_Id INT,                           
	@BankName VARCHAR(500),                            
	@ChequeNo VARCHAR(500),                            
	@ChequeDate VARCHAR(100),    
	@ReturnVal BIGINT OUTPUT                       
)                            
AS                            
BEGIN       
  INSERT INTO BoCustomerDetails(AltId,TransactionId,PaymentMode,BankName, ChequeNumber, ChequeDate,IsEnabled,CreatedUtc,CreatedBy)                            
  VALUES(NEWID(),@Trans_Id,'Cheque',@BankName,@ChequeNo,@ChequeDate,1,GetUTCDate(),NEWID())     
  SET @ReturnVal = 1            
END 
  