CREATE PROC [dbo].[BO_TransctionRevertdetails]  --4685          
(            
	@Retailer_id BIGINT            
)            
AS            
BEGIN   
	SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY T.Id  DESC) AS Sno, T.Id AS EventTransId, U.FirstName+' '+U.LastName AS RequestedBy,         
	DATEADD(MINUTE, CONVERT(INT,EA.TimeZone), TR.ReqDateTime) AS ReqDateTime,  --ReqDateTime,          
	Comments, ED.Name AS EventName,TC.Name AS VenueCatName, SUM(TD.TotalTickets) AS NoOfTic          
	FROM BO_TransactionRevertRequest TR WITH(NOLOCK)   
	INNER JOIN USers U  WITH(NOLOCK) on U.Id=TR.Retailer_Id       
	LEFT OUTER JOIN BoxofficeUserAdditionalDetails UAD WITH(NOLOCK) ON U.ID=UAD.UserId            
	INNER JOIN Transactions T WITH(NOLOCK) ON TR.TransId = T.Id           
	INNER JOIN TransactionDetails TD WITH(NOLOCK) ON TD.TransactionId = T.Id          
	INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.ID=TD.EventTicketAttributeId  
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETD.ID=ETA.EventTicketDetailId            
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON TC.Id = ETD.TicketCategoryId          
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.ID = ETD.EventDetailId   
	left outer JOIn EventAttributes  EA WITH(NOLOCK) ON EA.EventDetailId=ED.ID         
	INNER JOIN Venues V WITH(NOLOCK) ON V.ID = ED.VenueId          
	WHERE UAD.ParentId=@Retailer_id AND IsRevert = 0          
	GROUP BY T.Id, U.FirstName, U.LastName ,EA.TimeZone, ReqDateTime,          
	Comments, ED.Name, TC.Name    
END 