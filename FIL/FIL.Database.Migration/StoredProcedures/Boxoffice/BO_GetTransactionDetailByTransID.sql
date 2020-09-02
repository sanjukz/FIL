CREATE proc [dbo].[BO_GetTransactionDetailByTransID] --- 210000150,34242                                                                                      
(                                                                                                                                
  @TransId BIGINT,                                                                                                         
  @Uid BIGINT                                                                                                                               
)                                                                                                                                
AS                                                                                                                                
BEGIN                        
                        
DECLARE @Address VARCHAR(500)                        
DECLARE @UserMobileNumber VARCHAR(500)                      
DECLARE @userEmailId VARCHAR(500)        
DECLARE @nameOnCard VARCHAR(500)                      
                           
SELECT @Address=UserName  FROM  Users WITH(NOLOCk) WHERE Id=@Uid                      
SELECT @UserMobileNumber=PhoneNumber   FROM  Transactions WITH(NOLOCk) WHERE ID=@TransId                       
SELECT @userEmailId=EmailId  FROM  Transactions  WITH(NOLOCk) WHERE ID=@TransId     
                                            
DECLARE @EventId AS BIGINT                          
                      
IF EXISTS(SELECT Id FROM Transactions WITH(NOLOCk) WHERE Id = @TransId)                        
                        
SELECT DISTINCT T.ID AS TransId, ED.Id AS EventId, ED.Name AS EventName, Ed.StartDateTime AS EventStartDate, 
Ed.StartDateTime AS EventStartTime,--CONVERT(VARCHAR,T.CreatedUtc,100) AS CreatedDate,
DATEADD(MI,CONVERT(INT, ISNULL(EA.TimeZone,0)),T.CreatedUtc) AS CreatedDate,
c.Name AS City,V.Name AS Venue,    
'' AS CardNumber,'' AS CCVNum, @nameOnCard AS NameOnCard, PO.PaymentOptions AS CCtypeText,PO.Id AS CardType,                  
'' AS PayTypetext,'' AS PayType,'' AS CardExpire,T.GrossTicketAmount AS TotalCharge,ETA.ID AS VMCC_ID, TD.TotalTickets AS NoOfTic,TC.Name AS VenueCatName,
IsNull(TD.DiscountAmount,0) AS DiscountAmt,T.NetTicketAmount AS TotalticAmt,TD.PricePerTicket AS PricePertic,CT.code AS CurrencyName,
TDD.DeliveryTypeId AS DeliveryType,'' AS CardNumber,IsNull(TDD.PickupBy,0) AS PickedBy,'' AS Representative1,'' AS Representative2,
IsNUll(TDD.DeliveryStatus,0) AS DeliveryStatus,'false' AS IsDispatchReady,'false' AS IsComplete,'false' AS IsPrinted,
--TDD.DeliveryDate AS DeliveryDate,
DATEADD(MI,CONVERT(INT, isnull(EA.TimeZone,0)),TDD.DeliveryDate) AS DeliveryDate,
'self - '+TDD.DeliverdTo AS DeliverdTo,T.FirstName+' '+T.LastName AS BuyerName,'' AS OriginalTicketSerialNos,'' AS AuthLetter,'' AS IDType, '' AS IDNumber,T.ConvenienceCharges AS ConvenienceCharge,T.ServiceCharge AS ServiceTax,'' AS PickUpAt, '' AS DocateNumber,0 AS CourierDeliveryStatus,'' AS OriginalTicketSerialNos,'N/A' AS SeatNumber, '' AS BOUserName,'' AS [Address],T.PhoneNumber AS MobileNumber,T.EmailId AS EmailId,TD.Id AS VMCCMT_Id,2 AS PickUpType   
FROM TransactionDetails TD WITH(NOLOCK)     
INNER JOIN Transactions T WITH(NOLOCK)  ON TD.TransactionId = T.ID                                                            
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id =  T.CurrencyId                                                                                    
INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.ID = TD.EventTicketAttributeId     
INNER JOIN EventTicketDetails  ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id                              
INNER JOIN TicketCategories  TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                                         
INNER JOIN EventDetails  ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id  
LEFT OUTER JOIN EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID    
INNER JOIN Venues  v WITH(NOLOCK) ON v.Id = ED.VenueId     
INNER JOIN Cities c WITH(NOLOCK) ON c.Id = V.CityId     
LEFT OUTER JOIN TransactionDeliveryDetails TDD WITH(NOLOCK) ON TDD.TransactionDetailId=TD.ID    
LEFT OUTER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) On TPD.TransactionId=T.ID       
LEFT OUTER JOIN PaymentOptions PO WITH(NOLOCK) ON PO.ID=TPD.PaymentOptionId     
WHERE T.ID = @TransId                         
--AND ED.EventId in (SELECT EventId BoUserVenue FROM  WHERE UserId = @Uid and IsEnabled =1)                  
AND T.TransactionStatusId=8 and T.ChannelId in (1,2)    
--  Group By T.ID , ED.Id , ED.Name, Ed.StartDateTime  , Ed.StartDateTime ,CONVERT(VARCHAR,T.CreatedUtc,100) ,c.Name ,V.Name ,    
--'' ,'' , @nameOnCard, PO.PaymentOptions ,PO.Id,                  
--'','' ,'' ,T.GrossTicketAmount ,ETA.ID , TD.TotalTickets ,TC.Name ,IsNull(TD.DiscountAmount,0) ,T.NetTicketAmount ,TD.PricePerTicket ,CT.code ,TDD.DeliveryTypeId ,'' ,IsNull(TDD.PickupBy,0) ,'' ,'' ,IsNUll(TDD.DeliveryStatus,0) ,'false' ,'false' ,'false' ,TDD.DeliveryDate ,TDD.DeliverdTo ,T.FirstName+' '+T.LastName,'' ,'' ,'' , '' ,T.ConvenienceCharges ,T.ServiceCharge ,'' , '' ,0,'' ,'N/A' , '' ,'' ,T.PhoneNumber ,T.EmailId ,TD.Id ,2                   
ORDER BY ED.StartDateTime     
              
END 

