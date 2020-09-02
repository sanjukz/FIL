CREATE PROC [dbo].[Retail_GetTransactionReport_2017]  --4977,415,0                                             
(                            
	 @Retailer_Id BIGINT =201369 ,                      
	 @EventCatId BigINt=2180,                                                      
	 @EventId BIGINT = 0 ,                                                      
	 @FromDate varchar(100) = '',                                                       
	 @ToDate varchar(100) = ''                                                       
)                                                      
AS                                                      
BEGIN                      
                      
IF(@EventCatId IS NULL OR @EventCatId = '')                                                      
BEGIN                                                      
 SET @EventCatId = 0                                                      
END                                                         
                                                      
IF(@EventId IS NULL OR @EventId = '')                                                      
BEGIN                                                      
 SET @EventId = 0                                                      
END          
        
IF(@Retailer_Id=151958)                                                      
BEGIN                                                      
 SET @EventCatId = 2238                                                      
END                                           
                                                      
IF(@FromDate IS NULL OR @FromDate = '')                                                      
                                                      
BEGIN                                                      
                                                      
 SET @FromDate = '09-28-2015'                                                      
                                                      
END                                                      
IF(@ToDate IS NULL OR @ToDate = '')                                                      
BEGIN                                                      
 SET @ToDate = '09-28-2037'                                                      
END                                                      
DECLARE @TblTransTickets TABLE (TotalNoOfTic INT, EventTransId BIGINT)                                                       
DECLARE @Level INT = 0 --3 for Corporate, 2 for BM, 1 for User                                                      
IF EXISTS (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE UserId = @Retailer_Id AND UserType = 3)                                                      
                                                      
BEGIN                                                      
                                                      
SET @Level = 1                                                      
END                                                      
                                                      
ELSE IF EXISTS (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE UserId = @Retailer_Id AND UserType = 2)                                                      
                                                      
BEGIN                                                      
 SET @Level = 2                                                      
END                                                      
                                                      
ELSE                                                      
                                                      
BEGIN                                                      
 SET @Level = 3                                                      
                                                      
END                                                      
IF(@Level = 1)                                                      
BEGIN                                                       
 INSERT INTO @TblTransTickets                                                          
 SELECT SUM(TD.TotalTickets),T.ID as EventTransId                                                          
 FROM Transactions T WITH (NOLOCK)                                                            
INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId = T.Id     
INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId        
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId                                                       
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.Id      
 Left Outer Join EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID                                                    
 INNER JOIN BO_RetailCustomer RU  WITH (NOLOCK) ON RU.Trans_Id = T.Id              
 WHERE                                                          
  T.ChannelId in (4)                             
   AND ED.EventId = CASE @EventCatId WHEN 0 THEN ED.EventId ELSE @EventCatId END                      
   AND RU.Retailer_Id IN (SELECT ParentId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE 
   ParentId IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId = @Retailer_Id))                                                       
   AND ED.Id = CASE @EventId WHEN 0 THEN ED.Id ELSE @EventId END    
   AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) >= CONVERT(DATE,@FromDate)                                     
   AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) <= CONVERT(DATE,@ToDate)                             
 --  AND VMCCM.VMCC_Id = CASE @VMCC_Id WHEN 0 THEN VMCCM.VMCC_Id ELSE @VMCC_Id END                                      
GROUP BY T.Id                                                      
                                                      
END                                                       
                                                      
ELSE IF(@Level = 2)                                                      
BEGIN                                                       
INSERT INTO @TblTransTickets                                                          
SELECT SUM(TD.TotalTickets),T.ID as EventTransId                                                          
 FROM Transactions T WITH (NOLOCK)                                                            
INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId = T.Id     
INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId        
INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId                                                       
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.Id      
 Left Outer Join EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID                                                    
 INNER JOIN BO_RetailCustomer RU  WITH (NOLOCK) ON RU.Trans_Id = T.Id              
 WHERE                                                          
  T.ChannelId in (4)    
  AND ED.EventId = CASE @EventCatId WHEN 0 THEN ED.EventId ELSE @EventCatId END                                                      
  AND RU.Retailer_Id IN (SELECT UserId FROM BoxofficeUserAdditionalDetails WITH (NOLOCK) WHERE ParentId = @Retailer_Id)                                                      
  AND ED.Id = CASE @EventId WHEN 0 THEN ED.Id ELSE @EventId END    
  AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) >= CONVERT(DATE,@FromDate)                                                          
  AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) <= CONVERT(DATE,@ToDate)                                                      
 --- AND VMCCM.VMCC_Id = CASE @VMCC_Id WHEN 0 THEN VMCCM.VMCC_Id ELSE @VMCC_Id END                                                      
                                           
  GROUP BY T.Id                                                   
                                                      
END                                                    
ELSE                     
BEGIN                                                       
 INSERT INTO @TblTransTickets                                                
 SELECT SUM(TD.TotalTickets),T.ID as EventTransId                                                          
 FROM Transactions T WITH (NOLOCK)                                                            
 INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId = T.Id     
 INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId        
 INNER JOIN EventTicketDetails ETD WITH (NOLOCK) ON ETD.Id=ETA.EventTicketDetailId                                                       
 INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.Id      
 Left Outer Join EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID                                                    
 INNER JOIN BO_RetailCustomer RU  WITH (NOLOCK) ON RU.Trans_Id = T.Id              
 WHERE                                                          
  T.ChannelId in (4)    
  AND ED.EventId = CASE @EventCatId WHEN 0 THEN ED.EventId ELSE @EventCatId END        
 AND RU.Retailer_Id IN (@Retailer_Id)                                                      
 AND ED.Id = CASE @EventId WHEN 0 THEN ED.Id ELSE @EventId END                                                     
-- AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(ED.EventTimeZone,0)),ETD.CreatedDate)) >= CONVERT(DATE,@FromDate)                                                          
-- AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(ED.EventTimeZone,0)),ETD.CreatedDate)) <= CONVERT(DATE,@ToDate)                       
 -- AND VMCCM.VMCC_Id = CASE @VMCC_Id WHEN 0 THEN VMCCM.VMCC_Id ELSE @VMCC_Id END                                                      
 GROUP BY T.Id                                                    
                                                      
END                                                       
--SELECT * FROM @TblTransTickets                                                       
DECLARE @TransactionReport TABLE (                                                      
                                                      
--SrNo INT IDENTITY(1,1),                   
SerialNo INT IDENTITY(1,1),                                               
EventTransId BIGINT,                                  
CreatedDateTime VARCHAR(1000),                                                        
Username VARCHAR(500),
RetailerName VARCHAR(500),                                                       
CustomerName VARCHAR(500),                                                       
CustomerMobileNumber VARCHAR(500),                                                      
CustomerEmail VARCHAR(500),                                               
CityName VARCHAR(500),                                        
EventId BIGINT,                                         
EventName VARCHAR(500),        
VenueName VARCHAR(500),                                                       
EventDate VARCHAR(500),                                                      
Category VARCHAR(500),                                            
NumberOfTickets INT,                                                       
TicketPrice DECIMAL(18,2),                                                         
TotalTicketAmount DECIMAL(18,2),                                                         
GrossTicketAmount DECIMAL(18,2),                                                       
DiscountAmount DECIMAL(18,2),                                               
CourierCharge DECIMAL(18,2),                                                         
DBGrossTicketAmount DECIMAL(18,2),                    
ServiceTax DECIMAL(18,2),                                                      
ConvenienceCharges DECIMAL(18,2),                        
TicketType VARCHAR(500),                                              
PaymentType VARCHAR(500),                                     
VoidRequest VARCHAR(500),                                                       
IsChild INT,                                                       
IsRevert BIT,                                                      
CurrencyName VARCHAR(500),                                                       
Retailer_Id BIGINT,                                                         
Parent_Id BIGINT,                       
SeatNumber VARCHAR(MAX)                                                      
)                                                      
                                                      
INSERT INTO @TransactionReport                                                      
                                                      
SELECT                                                       
T.id as EventTransId,                                    
DATEADD(MI,CONVERT(INT, isnull(EA.TimeZone,0)),T.CreatedUtc) as DateOfTransaction,                                                   
U.UserName AS Username,                                                      
U.FirstName +' '+ U.LastName AS RetailerName,                                                      
T.FirstName + ' ' + T.LastName AS CustomerName, T.PhoneNumber as Cust_MobileNumber, T.EmailId as Cust_Email, C.Name as  CityName, ED.Id as EventId, ED.Name as EventName,       
V.Name as VenueAddress,                                                    
CONVERT(VARCHAR(20), ED.StartDateTime, 101) As EventDate, TC.Name AS Category, TD.TotalTickets AS NumberOfTickets, TD.PricePerTicket AS TicketPrice,                                                      
TD.TotalTickets * TD.PricePerTicket AS TotalTicketAmount, (TD.TotalTickets * TD.PricePerTicket) AS GrossTicketAmount,                                 
--(ETD.DiscountAmt / TotalNoOfTic) * VMCCM.NoOfTic AS DiscountAmount,                                 
TD.DiscountAmount AS DiscountAmount,                                       
0 as CourierCharge, T.GrossTicketAmount  AS DBGrossTicketAmount,                                                      
CONVERT(DECIMAL(18,2), ISNULL((T.ServiceCharge / TotalNoOfTic) * TD.TotalTickets,0)) AS ServiceTax,                         
CONVERT(DECIMAL(18,2), ISNULL((T.ConvenienceCharges / TotalNoOfTic) * TD.TotalTickets,0)) AS ConvenienceCharges,                                                  
CASE ISNULL(TD.TicketTypeId,0) WHEN 2 THEN 'Child' ELSE CASE ISNULL(TD.TicketTypeId,0) WHEN 3 THEN 'Senior Citizen' ELSE 'Adult' END END AS TicketType,                                           
RC.PaymentType,                                                  
--Case RC.Cust_PaymentMode when 5 Then 'Cash' Else RC.PaymentType END as [PaymentType],                                                  
                                                  
CASE TRR.IsRevert when '1' Then 'Approved' when '0' then 'Pending' else '' end as VoidRequest,                                   
--ISNULL(VMCCM.IsChild,0),    
0,    
 ISNULL(TRR.IsRevert,0),     
CT.Code AS CurrencyName,                                                      
RU.UserId as Retailer_Id, RU.ParentId as Parent_Id,  'N/A'  AS SeatNumber                                                      
FROM                                                       
Transactions T WITH (NOLOCK)                                                               
INNER JOIN @TblTransTickets TT ON TT.EventTransId = T.Id                                                       
INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId = T.Id                                                              
INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON TD.EventTicketAttributeId = ETA.Id      
Inner Join EventTicketDetails ETD  WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId                                                              
INNER JOIN TicketCategories TC WITH (NOLOCK) ON TC.Id = ETD.TicketCategoryId                      
INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.ID       
Left Outer Join EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID                                                        
--INNER JOIN ITKTS_CurrencyMapping CMAP WITH (NOLOCK) ON CMAP.Vmcc_id=VMCC.VMCC_Id                                                                 
INNER JOIN CurrencyTypes CT WITH (NOLOCK) ON CT.Id = T.CurrencyId    
INNER JOIN BO_RetailCustomer RC WITH (NOLOCK) ON RC.Trans_Id = T.Id    
INNER JOIN BoxofficeUserAdditionalDetails RU WITH (NOLOCK) ON RU.UserId = RC.Retailer_Id    
Inner JOin Users U WITH(NOLOCK) ON U.Id=RU.UserId                                                         
INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId                                    
INNER JOIN Cities C WITH (NOLOCK) ON C.id = V.CityId                                                      
LEFT OUTER JOIN BO_TransactionRevertRequest TRR ON TRR.TransId = T.Id                                                       
WHERE T.TransactionStatusId =8 and  TD.TotalTickets >0                                                     
                                                  
--Hem                                                  
 AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) >= CONVERT(DATE,@FromDate)                                                          
 AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) <= CONVERT(DATE,@ToDate)                                          
                                            
                                                     
UNION                                                    
                                                    
--SELECT                                                       
--ETD.EventTransId,                                                      
--DATEADD(MI,CONVERT(INT, ISNULL(ED.EventTimeZone,0)),ETD.CreatedDate) as DateOfTransaction,                                                      
--RU.UserName AS Username,                                                      
--RU.FirstName +' '+ RU.LastName AS RetailerName,                                                      
--RC.Cust_FirstName + ' ' + RC.Cust_LastName AS CustomerName,                                                      
--RC.Cust_MobileNumber,                                                      
--RC.Cust_Email,                                                      
--CM.City_Name AS CityName,                                                      
                                                      
--ED.EventId,                                                      
--ED.EventName,      
--VD.VenueAddress,                                               
--CONVERT(VARCHAR(20), ED.EventStartDate, 101) As EventDate,                                
--VCD.VenueCatName AS Category,                                                      
--VMCCM.NoOfTic AS NumberOfTickets,                                                      
--VMCCM.PricePerTic AS TicketPrice,                                                      
--VMCCM.NoOfTic * VMCCM.PricePerTic AS TotalTicketAmount,                                                      
--(VMCCM.NoOfTic * VMCCM.PricePerTic) AS GrossTicketAmount,                               
----(ETD.DiscountAmt / TotalNoOfTic) * VMCCM.NoOfTic AS DiscountAmount,                                              
--VMCCM.DiscountAmount AS DiscountAmount,                                
--CONVERT(DECIMAL(18,2), ISNULL((ETD.Othercharge / TotalNoOfTic) * VMCCM.NoOfTic, 0)) AS CourierCharge,                                                      
--ETD.TotalCharge  AS DBGrossTicketAmount,                                                      
--CONVERT(DECIMAL(18,2), ISNULL((ETD.ServiceTax / TotalNoOfTic) * VMCCM.NoOfTic,0)) AS ServiceTax,                                                      
--CONVERT(DECIMAL(18,2), ISNULL((ETD.ConvenienceCharge / TotalNoOfTic) * VMCCM.NoOfTic,0)) AS ConvenienceCharges,                                                      
     
--CASE ISNULL(VMCCM.IsChild,0) WHEN 1 THEN 'Child' ELSE CASE ISNULL(VMCCM.IsSRCitizen,0) WHEN 1 THEN 'Senior Citizen' ELSE 'Adult' END END AS TicketType,                    
--RC.PaymentType,                                                      
--CASE TRR.IsRevert when '1' Then 'Approved' when '0' then 'Pending' else '' end as VoidRequest,                                                      
--ISNULL(VMCCM.IsChild,0),                                                      
--ISNULL(TRR.IsRevert,0),                                                      
                                          
--ISNULL(RU.CurrencyName, CCM.CurrencyName) AS CurrencyName,                             
--RU.Retailer_Id,                                                          
--RU.Parent_Id,                             
--CASE ISNULL(VMCC.IsLayoutAvail,0) WHEN 1 THEN ISNULL(VMCCM.SeatNumber, 'N/A') ELSE 'N/A' END AS SeatNumber                                                     
--FROM                                                       
--ITKTSEVNT_EventTransDetails ETD WITH (NOLOCK)                                                               
--INNER JOIN @TblTransTickets TT ON TT.EventTransId = ETD.EventTransId                                                       
--INNER JOIN ITKTSEVNT_VMCC_MapTrans VMCCM WITH (NOLOCK) ON VMCCM.EventTransId = ETD.EventTransId                                                              
--INNER JOIN ITKTSEVNT_VenueMapCategoryClass VMCC WITH (NOLOCK) ON VMCCM.VMCC_Id = VMCC.VMCC_Id                                                               
--INNER JOIN ITKTSEVNT_VenueCategoryDetails VCD WITH (NOLOCK) ON VCD.VenueCatId = VMCC.VenueCatId                                                              
--INNER JOIN ITKTSEVNT_EventDetails ED WITH (NOLOCK) ON VMCCM.EventId = Ed.EventId                                                         
--INNER JOIN ITKTS_CurrencyMapping CMAP WITH (NOLOCK) ON CMAP.Vmcc_id=VMCC.VMCC_Id                                                                 
--INNER JOIN ITKTS_CurrencyMaster CCM WITH (NOLOCK) ON CCM.Currencyid = ETD.CurrencyId                                                      
--INNER JOIN ITKTS_RetailUser_Mst RU WITH (NOLOCK) ON RU.Retailer_Id = ETD.UserId                                                       
--INNER JOIN ITKTSEvnt_RetailCustomer RC WITH (NOLOCK) ON RC.Trans_Id = ETD.EventTransId                                                       
--INNER JOIN ITKTSEVNT_VenueDetails VD WITH (NOLOCK) ON VD.VenueId = ED.EventVenue                                                               
--INNER JOIN ITKTS_City_Mst CM WITH (NOLOCK) ON VD.VenueCity = CM.City_id                                                      
--INNER JOIN ITKTS_TransactionRevertRequest TRR ON TRR.TransId = ETD.EventTransId                                                         
--WHERE --ETD.BoughtStatus in (0)  and                          
--VMCCM.NoOfTic >0   and   TRR.IsRevert=1       
SELECT                                                       
T.id as EventTransId,                                    
DATEADD(MI,CONVERT(INT, isnull(EA.TimeZone,0)),T.CreatedUtc) as DateOfTransaction,                                                   
U.UserName AS Username,                                                      
U.FirstName +' '+ U.LastName AS RetailerName,                                                      
T.FirstName + ' ' + T.LastName AS CustomerName, T.PhoneNumber as Cust_MobileNumber, T.EmailId as Cust_Email, C.Name as  CityName, ED.Id as EventId, ED.Name as EventName,       
V.Name as VenueAddress,                                                    
CONVERT(VARCHAR(20), ED.StartDateTime, 101) As EventDate, TC.Name AS Category, TD.TotalTickets AS NumberOfTickets, TD.PricePerTicket AS TicketPrice,                                                      
TD.TotalTickets * TD.PricePerTicket AS TotalTicketAmount, (TD.TotalTickets * TD.PricePerTicket) AS GrossTicketAmount,                                 
--(ETD.DiscountAmt / TotalNoOfTic) * VMCCM.NoOfTic AS DiscountAmount,                             
TD.DiscountAmount AS DiscountAmount,                                       
0 as CourierCharge, T.GrossTicketAmount  AS DBGrossTicketAmount,                                                      
CONVERT(DECIMAL(18,2), ISNULL((T.ServiceCharge / TotalNoOfTic) * TD.TotalTickets,0)) AS ServiceTax,                         
CONVERT(DECIMAL(18,2), ISNULL((T.ConvenienceCharges / TotalNoOfTic) * TD.TotalTickets,0)) AS ConvenienceCharges,                                                  
CASE ISNULL(TD.TicketTypeId,0) WHEN 2 THEN 'Child' ELSE CASE ISNULL(TD.TicketTypeId,0) WHEN 3 THEN 'Senior Citizen' ELSE 'Adult' END END AS TicketType,                                           
RC.PaymentType,                                                  
--Case RC.Cust_PaymentMode when 5 Then 'Cash' Else RC.PaymentType END as [PaymentType],                                                  
                                                  
CASE TRR.IsRevert when '1' Then 'Approved' when '0' then 'Pending' else '' end as VoidRequest,                                   
--ISNULL(VMCCM.IsChild,0),    
0,    
 ISNULL(TRR.IsRevert,0),     
CT.Code AS CurrencyName,                                                      
RU.UserId as Retailer_Id, RU.ParentId as Parent_Id,  'N/A'  AS SeatNumber                                                      
FROM                                                       
Transactions T WITH (NOLOCK)                                                               
INNER JOIN @TblTransTickets TT ON TT.EventTransId = T.Id                                                       
INNER JOIN TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId = T.Id                                                              
INNER JOIN EventTicketAttributes ETA WITH (NOLOCK) ON TD.EventTicketAttributeId = ETA.Id      
Inner Join EventTicketDetails ETD  WITH (NOLOCK) ON ETD.Id = ETA.EventTicketDetailId                                                              
INNER JOIN TicketCategories TC WITH (NOLOCK) ON TC.Id = ETD.TicketCategoryId                                
INNER JOIN EventDetails ED WITH (NOLOCK) ON ETD.EventDetailId = Ed.ID       
Left Outer Join EventAttributes EA WITH (NOLOCK) On EA.EventDetailId=ED.ID                                                        
--INNER JOIN ITKTS_CurrencyMapping CMAP WITH (NOLOCK) ON CMAP.Vmcc_id=VMCC.VMCC_Id                                                                 
INNER JOIN CurrencyTypes CT WITH (NOLOCK) ON CT.Id = T.CurrencyId    
INNER JOIN BO_RetailCustomer RC WITH (NOLOCK) ON RC.Trans_Id = T.Id    
INNER JOIN BoxofficeUserAdditionalDetails RU WITH (NOLOCK) ON RU.UserId = RC.Retailer_Id    
Inner JOin Users U WITH(NOLOCK) ON U.Id=RU.UserId                                                         
INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId                                    
INNER JOIN Cities C WITH (NOLOCK) ON C.id = V.CityId                                                      
LEFT OUTER JOIN BO_TransactionRevertRequest TRR ON TRR.TransId = T.Id                                                       
WHERE T.TransactionStatusId =8 and  TD.TotalTickets >0  and TRR.IsRevert=1                                                      
                                                  
--Hem                                                  
 AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) >= CONVERT(DATE,@FromDate)                                                          
 AND CONVERT(DATE,DATEADD(MI,CONVERT(Int,ISNULL(EA.TimeZone,0)),T.CreatedUtc)) <= CONVERT(DATE,@ToDate)        
 Order By   T.Id                             
                                                   
SELECT                                                       
--SrNo,                  
ROW_NUMBER() Over (Order by SerialNo desc) As SrNo,                     
SerialNo,                                            
EventTransId,                                                       
CreatedDateTime,                         
Username,           
RetailerName,                                                       
CustomerName,
CustomerMobileNumber,                                                      
CustomerEmail,                                                      
CityName,                                                       
EventId,                                                       
EventName,                                                     
EventDate,                                                       
Category,                                                       
NumberOfTickets,                                                       
TicketPrice,                                                       
TotalTicketAmount,                                                       
--GrossTicketAmount,                                 
(GrossTicketAmount - DiscountAmount) as GrossTicketAmount,                                                      
DiscountAmount,                                 
CourierCharge,                                                       
DBGrossTicketAmount,                                                       
ServiceTax,                                                       
ConvenienceCharges,                                                       
(GrossTicketAmount + ConvenienceCharges + ServiceTax + CourierCharge) - DiscountAmount AS TotalTransactedAmount,                                                       
TicketType,                                                       
PaymentType,                                                       
VoidRequest,                                                       
IsChild,                                                      
CurrencyName,                             
Retailer_Id,                                                       
Parent_Id,                                                       
SeatNumber                                                       
FROM @TransactionReport  order by SerialNo desc                                          
                                                   
SELECT                                                       
ISNULL(SUM(NumberOfTickets), 0) AS ValidTickets,                                                       
ISNULL(SUM(GrossTicketAmount), 0)  AS ValidGrossAmount,                                              
ISNULL(SUM(DiscountAmount), 0) AS ValidDiscountAmount,                                                      
ISNULL(SUM((GrossTicketAmount + ConvenienceCharges + ServiceTax + CourierCharge) - DiscountAmount), 0) AS ValidNetTicketAmount,PaymentType,VenueName,CityName                                                           
FROM @TransactionReport                                                      
WHERE IsRevert = 0  group by PaymentType,VenueName,CityName       
Order By VenueName,CityName                                      
                                                    
SELECT                                                       
ISNULL(SUM(NumberOfTickets), 0) AS VoidedTickets,                  
ISNULL(SUM(GrossTicketAmount), 0)  AS VoidedGrossAmount                                                      
FROM @TransactionReport                                                      
WHERE IsRevert = 1                                             
                                                   
SELECT                                                       
--RetailerName,                                                      
--ISNULL(SUM(NumberOfTickets), 0) AS NoOfTickets,           
--ISNULL(SUM(GrossTicketAmount), 0)  AS TotalTicketAmount,                              
RetailerName,                                                      
ISNULL(SUM(NumberOfTickets), 0) AS NoOfTickets,                             
ISNULL(SUM(GrossTicketAmount), 0)  AS ValidGrossAmount,                               
ISNULL(SUM(DiscountAmount), 0) AS ValidDiscountAmount,                            
ISNULL(SUM((GrossTicketAmount + ConvenienceCharges + ServiceTax + CourierCharge) - DiscountAmount), 0) AS TotalTicketAmount ,                        
CurrencyName                                                       
FROM @TransactionReport                                                  
WHERE IsRevert = 0                                         
GROUP BY RetailerName, CurrencyName                       
ORDER BY RetailerName                                                      
END