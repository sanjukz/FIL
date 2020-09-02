CREATE PROC [dbo].[BO_InsertCustomerDetails]                
(                                      
	@Retailer_Id BIGINT,                                      
	@Cust_FirstName VARCHAR(50),                                      
	@Cust_LastName VARCHAR(50),                                      
	@Cust_Email VARCHAR(100),                                      
	@Cust_City VARCHAR(60),                                      
	@Cust_Address VARCHAR(500),                                      
	@Cust_PinCode NCHAR(20),     
	@Cust_PhoneCode Varchar(50),                                   
	@Cust_MobileNumber VARCHAR(50),              
	@Cust_PaymentMode INT,                                   
	@Cust_IdType VARCHAR(100),                                  
	@Cust_IdTypeNumber VARCHAR(100),              
	@UpdatedBy VARCHAR(50),                                      
	@Trans_Id INT,              
	@DealerCharge decimal(8,2)=0,              
	@ReturnVal BIGINT OUTPUT,            
	@PaymentType varchar(500)=Null                   
                                   
)                                      
AS                                      
BEGIN TRANSACTION                                      
  --Insert Retail Customer table ITKTS_UserProfile_Mst first                         
	UPDATE Transactions                                 
	SET FirstName= @Cust_FirstName,LastName=@Cust_LastName,PhoneCode=@Cust_PhoneCode,PhoneNumber=@Cust_MobileNumber, EmailId=@Cust_Email                              
	WHERE ID = @Trans_Id                           
                  
  INSERT INTO BO_RetailCustomer                                      
  (                                      
   Retailer_Id, Cust_FirstName, Cust_LastName, Cust_Email, Cust_City,Cust_Address, Cust_PinCode, Cust_MobileNumber,                                      
   Cust_PaymentMode,Cust_IdType,Cust_IdTypeNumber,DealerAccessCharge,UpdateDate, Trans_Id,UpdatedBy, PaymentType                                      
  )                                      
  VALUES                                      
  (                                      
   @Retailer_Id, @Cust_FirstName,@Cust_LastName,LOWER(@Cust_Email), @Cust_City,@Cust_Address,@Cust_PinCode,@Cust_MobileNumber,                                      
   @Cust_PaymentMode, @Cust_IdType,@Cust_IdTypeNumber,@DealerCharge,GetDate(), @Trans_Id,@UpdatedBy,@PaymentType                                     
  )                                      
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