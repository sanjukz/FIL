CREATE PROC [dbo].[ResetTicketforPrint]    
(      
	@TransID BIGINT,      
	@UserName VARCHAR(200)      
)      
AS      
BEGIN     
--Insert into PrintathomeLog(Transid,ResetBy,CreatedDate) values (@TransID,@UserName, GETDATE())
	UPDATE MatchSeatTicketDetails set PrintStatusId=1  where TransactionId=@TransID and isnull(EntryStatus,0)=0    
	SELECT 'Your reset request has bean accepted and sent mail to user with reprint link sucessfully'      
      
END   
