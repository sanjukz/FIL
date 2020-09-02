CREATE PROC [dbo].[KITMS__GetHandOverSheetSerialDetails_Temp]    
(                          
	@TransId BIGINT                        
)
AS                                  
BEGIN                      
	SELECT DISTINCT A.TransactionId AS TransId, B.TotalTickets AS QuantityPrinted, SerialStart AS SerialFrom,
	SerialEnd AS SerialTo, TicketHandedBy AS HandedBy, TicketHandedTo AS SubmittedTo
	FROM HandoverSheets A WITH(NOLOCK)
 	INNER JOIN Transactions B WITH(NOLOCK) ON A.TransactionId =B.Id AND A.TransactionId = @TransId
END 