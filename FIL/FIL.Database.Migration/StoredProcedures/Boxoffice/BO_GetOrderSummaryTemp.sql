CREATE PROCEDURE  [dbo].[BO_GetOrderSummaryTemp] --210000253                       
(                                    
	@EventTransId BIGINT
)                                    
AS                                        
BEGIN               
	SELECT A.Id AS EventTransID, ED.Name AS EventName,  D.Name AS VenueCatName, B.TotalTickets AS NoOfTic,    
	B.PricePerTicket AS PricePerTic, B.DiscountAmount AS DiscountAmt, A.ConvenienceCharges AS ConvenienceCharge, 
	A.ServiceCharge AS ServiceTax, A.ConvenienceCharges  + A.ServiceCharge AS TotalConvFee, 
	A.NetTicketAmount AS TotalCharge,A.NetTicketAmount AS TotalTicAmt,  ED.ID AS EventId , CM.Code AS CurrencyName,
	ISNULL(dbo.SeatTagString(b.id),'N/A')  AS SeatNumber,   --'N/A' AS SeatNumber                    
	ED.StartDateTime AS EventStartDate,ED.StartDateTime AS EventStartTime, VD.Name AS VenueAddress, 
	C.Name AS City_Name,1 AS PickedBy, 2 AS [DeliveryType], '' AS PickUpnotes, '' AS CourierNotes, '' AS PAHNotes, 
	'' AS MTicketNotes, E.Name AS EventCatName, E.  Id AS EventCatId, A.CreatedUtc AS CreatedDate,                    
	0 AS IsFamilyPkg,A.PhoneNumber AS [Cust_MobileNumber], 0 AS Othercharge,'' AS ShiftName,'' AS VisitDate,
	'' AS VisitorTypeDesc,'' AS VisitorName, '' AS VisitorDOB, 'Male' AS VisitorGender,'' AS VisitorTypeAges,             
	B.Id AS VMCCMT_Id,B.TicketTypeId                     
	FROM Transactions A  WITH(NOLOCK)                  
	INNER JOIN TransactionDetails B WITH(NOLOCK) ON A.Id = B.TransactionId                    
	INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.Id=B.EventTicketAttributeId     
	Inner Join EventTicketDetails ETD WITH(NOLOCK) On ETD.Id=ETA.EventTicketDetailId                   
	INNER JOIN TicketCategories D WITH(NOLOCK) ON D.Id =ETD.TicketCategoryId                    
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.ID = ETD.EventDetailId                    
	INNER JOIN Venues VD WITH(NOLOCK) ON Vd.Id = ED.VenueId                    
	INNER JOIN Cities C WITH(NOLOCK) ON C.Id = VD.CityId       
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON CM.Id = A.CurrencyId      
	Left Outer JOIN BO_RetailCustomer RC WITH(NOLOCK) ON RC.Trans_Id = A.Id                    
	INNER JOIN Events E WITH(NOLOCK) ON E.Id = ED.EventId      
	WHERE A.Id=@EventTransId          
	AND B.TotalTickets > 0        
	ORDER BY ED.Id       
END   