CREATE PROC [dbo].[BO_GetClosingBOReport]   
(          
	@Retailer_id BIGINT           
)         
AS           
BEGIN       
SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY CB.Id DESC) AS [Sr. No.], U.FirstName+' '+U.LastName as [BO User Name], U.UserName as [Box Office ID],         
CB.CreatedUtc AS [Date/Time],TicketStockStartSrNo as [Last Ticket for the day],        
CB.WasteTickets AS [Ticket Wastage in nos.],CB.CashAmount AS [Cash Amount],CB.CardAmount AS [Card Amount]     
FROM BoClosingDetails CB  WITH(NOLOCK)   
INNER JOIN BoxofficeUserAdditionalDetails  RUM WITH(NOLOCK) ON CB.UserId=RUM.UserId  
INNER JOIN Users U WITH(NOLOCK)  ON U.Id=RUM.UserId    
WHERE RUM.ParentId=@Retailer_id --and CB.CreatedDate > '2017-03-01 00:00:00.000'    
GROUP BY  CB.ID,U.FirstName,U.LastName,U.UserName,CB.CreatedUtc,CB.TicketStockStartSrNo,CB.WasteTickets,CB.CashAmount,CB.CardAmount,CB.CardAmount         
            
END   