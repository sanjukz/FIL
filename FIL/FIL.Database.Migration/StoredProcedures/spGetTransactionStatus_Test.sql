CREATE PROCEDURE spGetTransactionStatus_Test-- '210190490','0','0','0','0','0','0','0'                                                      
(                                                                      
@TransactionId VARCHAR(500),               
@FirstName VARCHAR(500),              
@LastName VARCHAR(500),                                                                         
@EmailId VARCHAR(500),                                                                          
@UserMobileNo VARCHAR(500),                                                                         
@DateOfTrans VARCHAR(500),                               
@IPAddress varchar(1000),        
@BarcodeNumber varchar(500)                                       
)                                                                        
AS                                                                        
BEGIN         
       
If(@BarcodeNumber='0')        
BEGIN         
      
SELECT ROW_NUMBER() over (order by T.Id DESC) as SNo, T.Id AS ConfNumber,                            
CONVERT(varchar,T.CreatedUtc,100) as CreatedDate,                             
ISNULL(T.EmailId,'N/A') as UserEmailId,                     
ISNULL(T.PhoneNumber,U.PhoneNumber) as UserMobileNo,                           
ISNULL((T.FirstName+' '+T.LastName),'N/A') as BuyerName,                                                  
ISNULL(ED.Name,'N/A') as EventName,                               
CONVERT(varchar,ED.StartDateTime,100) as EventDate,          
ISNULL(TC.NAME, 'N/A') as VenueCatName,                             
ISNULL(TD.TotalTickets,'0') as TotalTicket,                            
T.GrossTicketAmount,                            
ISNULL(TPD.PayConfNumber,'N/A') as PayConfNumber,          
ISNULL(PG.PaymentGateway,'N/A') as PaymentGateway,         
(Select Channels From Channels WITH(NOLOCK) Where Id=T.ChannelId) as Channel,          
TS.TransactionStatus AS BoughtStatus,          
T.CountryName as CountryName                      
FROM Transactions T WITH (NOLOCK)                                                                        
 INNER JOIN TransactionDetails TD WITH (NOLOCK) ON  T.Id =TD.TransactionId                
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId                    
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId          
 INNER JOIN TicketCategories TC WITH (NOLOCK) ON ETD.TicketCategoryId=TC.ID          
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.Id                  
 Left Outer JOin EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID             
 Left Outer JOin TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId     
 --INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId           
 --Left Outer JOin PaymentGateways PG WITH(NOLOCK) ON PG.Id=PaymentGatewayId      
 INNER JOin PaymentGateways PG WITH(NOLOCK) ON PG.Id=PaymentGatewayId      
 INNER JOIN TransactionStatuses TS WITH(NOLOCK) ON TS.Id=T.TransactionStatusId       
 INNER JOIN Users U WITH(NOLOCK) ON U.AltId=T.CreatedBy                       
 WHERE  T.Id =CASE @TransactionId WHEN 0 THEN T.Id ELSE @TransactionId END          
 AND T.FirstName =CASE @FirstName WHEN '0' THEN T.FirstName ELSE @FirstName END           
 AND T.LastName = CASE @LastName WHEN '0' THEN T.LastName ELSE @LastName END           
 AND T.EmailId =  CASE @EmailId WHEN '0' THEN T.EmailId ELSE @EmailId END           
 AND (ISNULL(T.PhoneNumber,0) = CASE @UserMobileNo WHEN '0' THEN ISNULL(T.PhoneNumber,0) ELSE @UserMobileNo END     
 OR ISNULL(U.PhoneNumber,0) = CASE @UserMobileNo WHEN '0' THEN ISNULL(U.PhoneNumber,0) ELSE @UserMobileNo END)             
 AND T.CreatedUtc = CASE @DateOfTrans WHEN 0 THEN T.CreatedUtc ELSE @DateOfTrans END          
 AND T.ChannelId In(1,4)         
 Order By T.Id DESC        
         
 END        
 ELSE        
 BEGIN        
 SELECT  ROW_NUMBER() over (order by T.Id DESC) as SNo, T.Id AS ConfNumber,                            
CONVERT(varchar,T.CreatedUtc,100) as CreatedDate,                             
ISNULL(T.EmailId,'N/A') as UserEmailId,                     
ISNULL(T.PhoneNumber,U.PhoneNumber) as UserMobileNo,                            
ISNULL((T.FirstName+''+T.LastName),'N/A') as BuyerName,                                                  
ISNULL(ED.Name,'N/A') as EventName,                               
CONVERT(varchar,ED.StartDateTime,100) as EventDate,          
ISNULL(TC.NAME, 'N/A') as VenueCatName,                             
ISNULL(TD.TotalTickets,'0') as TotalTicket,                            
T.GrossTicketAmount,                            
ISNULL(TPD.PayConfNumber,'N/A') as PayConfNumber,          
ISNULL(PG.PaymentGateway,'N/A') as PaymentGateway,        
(Select Channels From Channels WITH(NOLOCK) Where Id=T.ChannelId) as Channel,            
TS.TransactionStatus AS BoughtStatus,          
T.CountryName as CountryName ,MTD.BarcodeNumber                     
FROM Transactions T WITH (NOLOCK)                                                                        
 INNER JOIN TransactionDetails TD WITH (NOLOCK) ON  T.Id =TD.TransactionId                
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId                    
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId          
 INNER JOIN TicketCategories TC WITH (NOLOCK) ON ETD.TicketCategoryId=TC.ID          
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.Id                  
 Left Outer JOin EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID             
 Left Outer JOin TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id=TPD.TransactionId          
 Left Outer JOin PaymentGateways PG WITH(NOLOCK) ON PG.Id=PaymentGatewayId           
 INNER JOIN TransactionStatuses TS WITH(NOLOCK) ON TS.Id=T.TransactionStatusId     
 INNER JOIN Users U WITH(NOLOCK) ON U.AltId=T.CreatedBy           
 INNER JOIN MatchSeatTicketDetails MTD WITH(NOLOCK) ON ETD.Id=MTD.EventTicketDetailId And MTD.TransactionId=TD.TransactionId                        
 WHERE MTD.BarcodeNumber=@BarcodeNumber  AND T.ChannelId In(1,4)         
 Order By T.Id DESC        
 END         
 END 