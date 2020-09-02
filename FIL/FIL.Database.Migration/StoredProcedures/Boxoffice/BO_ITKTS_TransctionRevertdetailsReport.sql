CREATE Proc [dbo].[BO_ITKTS_TransctionRevertdetailsReport]          
(          
	@Retailer_id BIGINT          
)          
AS    
BEGIN   
SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY T.ID DESC) AS Sno, T.Id as EventTransId, U.FirstName+' '+U.LastName AS RequestedBy,       
DATEADD(MINUTE, CONVERT(INT,EA.TimeZone), TRR.ReqDateTime) AS ReqDateTime, --ReqDateTime,        
Comments,ED.Name as EventName,TC.Name as VenueCatName, SUM(TD.TotalTickets) AS NoOfTic, CASE IsRevert WHEN 1 THEN 'Yes' ELSE 'No' END AS IsRevert      
FROM BO_TransactionRevertRequest TRR  WITH(NOlock)        
Inner Join USers U WITH(NOlock)  on TRR.Retailer_id=U.Id
Left Outer join BoxofficeUserAdditionalDetails UAD WITH(NOlock) on U.ID=UAD.UserId          
INNER JOIN Transactions T WITH(NOlock) ON TRR.TransId = T.Id         
INNER JOIN TransactionDetails TD WITH(NOlock) ON TD.TransactionId = T.Id        
INNER JOIN EventTicketAttributes ETA WITH(NOlock) ON ETA.ID=TD.EventTicketAttributeId
INNER JOIN EventTicketDetails ETD WITH(NOlock) ON ETD.ID=ETA.EventTicketDetailId          
INNER JOIN TicketCategories TC WITH(NOlock) ON TC.Id = ETD.TicketCategoryId  
INNER JOIN EventDetails ED WITH(NOlock) ON ED.ID = ETD.EventDetailId 
Inner Join EventAttributes EA WITH(NOlock) ON EA.EventDetailId=ED.ID       
INNER JOIN Venues V WITH(NOlock) ON V.ID = ED.VenueId   
where UAD.ParentId=@Retailer_id       
GROUP BY T.ID, U.FirstName, U.LastName ,EA.TimeZone, ReqDateTime ,      
Comments,  ED.Name, TC.Name , IsRevert        
      
End        