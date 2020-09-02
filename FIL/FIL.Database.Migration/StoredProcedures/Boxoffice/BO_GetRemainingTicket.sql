CREATE PROCEDURE [dbo].[BO_GetRemainingTicket]      
(          
	@VMCC_ID BIGINT          
)          
AS          
BEGIN          
 SELECT RemainingTicketForSale as TotalTic FROM EventTicketAttributes ETA WITH(NOLOCK)        
 WHERE Id = @VMCC_ID AND IsEnabled = 1          
END     

   
        
 