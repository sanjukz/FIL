CREATE PROC [dbo].[BO_CheckTicketType]          
(          
	@TransId BIGINT          
)          
AS          
BEGIN
   SELECT  FirstName +' ' + LastName AS  SponsorOrCustomerName, 0 AS TransactionBy FROM Transactions WITH(NOLOCK)                    
   WHERE Id=@TransID 
END 