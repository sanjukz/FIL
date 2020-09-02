CREATE proc [dbo].[KITMS_GenerateInvoiceBKP]   --187                            
(                            
  @CorpOrderId bigint                             
)                            
AS                            
BEGIN   
---DECLARE @CorpOrderId BIGINT=187  
  
 DECLARE @TotalItemCount INT                            
 DECLARE @InvoiceGeneratedCount INT                          
 Declare @Vat1 Decimal(18,2)                          
 Declare @Vat2 Decimal(18,2)                         
                             
 SELECT @TotalItemCount=COUNT(*)                            
 FROM CorporateRequestDetails CODM WITH(NOLOCK)                              
 INNER JOIN CorporateRequests COM WITH(NOLOCK) ON COM.Id = CODM.CorporateRequestId                            
 WHERE COM.Id=@CorpOrderId                             
                            
 SELECT @InvoiceGeneratedCount=COUNT(*)                            
 FROM CorporateRequestDetails CODM WITH(NOLOCK)                             
 INNER JOIN CorporateRequests COM WITH(NOLOCK) ON COM.Id = CODM.CorporateRequestId  
 INNER JOIN InvoiceDetails IND WITH(NOLOCK) ON CODM.Id = IND.CorporateRequestDetailId                            
 WHERE COM.Id=@CorpOrderId   
  
 DECLARE @USername VARCHAR(500), @PickupEmailId  VARCHAR(500),@EmailId  VARCHAR(500),@AltId VARCHAR(500),  
 @EmailTo NVARCHAR(1000)  
 SELECT @PickupEmailId=ISNULL(PickupRepresentativeEmail,''),@EmailId = Email  
 FROM CorporateRequests WITH(NOLOCK) WHERE Id = @CorpOrderId  
  
  SELECT @USername=ISNULL(UserName,'')                        
 FROM CorporateRequestDetails CODM WITH(NOLOCK)                              
 INNER JOIN CorporateRequests COM WITH(NOLOCK) ON COM.Id = CODM.CorporateRequestId  
 INNER JOIN InvoiceDetails IND WITH(NOLOCK) ON CODM.Id = IND.CorporateRequestDetailId     
 INNER JOIN Users U WITH(NOLOCK) ON  IND.GeneratedBy = U.AltId  
 WHERE COM.Id=@CorpOrderId   
  
 SET @EmailTo  = @EmailId   
 SET @EmailTo  = CASE WHEN @PickupEmailId <>'' THEN @EmailTo +';'+ @PickupEmailId  ELSE @EmailTo END     
 SET @EmailTo  = CASE WHEN @USername <>'' THEN @EmailTo +';'+ @USername  ELSE @EmailTo END     
  
SELECT ED.Name AS  Match,  CONVERT(NVARCHAR(17),ED.StartDateTime,113) AS EventStartDate,   
TC.Name AS VenueCatName, ISNULL(CRD.ApprovedTickets,0) AS Quantity,   
CONVERT(DECIMAL(18,2),CASE RequestOrderType WHEN 1 THEN 0                             
ELSE CASE WHEN ISNULL(ValueAddedTax,0)<>0 THEN  
(ETA.Price /((CAST((REPLACE(VAT.Value,'%','') ) AS DECIMAL(18,2))/100)+1))  
ELSE ETA.Price End END) AS PricePerTic,  
 RequestOrderType AS IsCompOrder,  
CONVERT(DECIMAL(18,2),CASE RequestOrderType WHEN 1 THEN 0                             
ELSE CASE WHEN ISNULL(ValueAddedTax,0)<>0 THEN  
(ETA.LocalPrice /((CAST((REPLACE(VAT.Value,'%','') ) AS DECIMAL(18,2))/100)+1))  
ELSE ETA.LocalPrice END END) AS LocalPricePerTic,  
CR.SponsorName, ISNULL(CR.SponsorId,0) AS SponsorId, CONVERT(NVARCHAR(19),GETUTCDATE()) AS InvGeneratedDate,  
IND.GrossTicketAmount AS InvTicketAmount,  
CM1.Code AS InvCurrency, ConvenienceCharges AS ConvFeeAmt, ServiceCharge AS ServiceTaxAmt,  
DiscountAmount AS DiscountAmt, NetTicketAmount AS InvoicedAmt,  
(NetTicketAmount - ISNULL(ValueAddedTax,0)) AS TotalAmt,  
NetTicketAmount AS TotalOrderAmt,  
ISNULL(BD.BankDetail,'') AS BankDetails,  
InvoiceNumber AS InvoiceId,   
ISNULL(UserName,'Admin') AS InvGeneratedBy,  
ISNULL(IND.KzFirmName,'Kyazoonga Inc.') as KZFirmName,  
CONVERT(VARCHAR(19),Dueutc) AS InvDueDate,  
CASE PickupRepresentativeFirstName WHEN '' THEN CR.FirstName +' '+ CR.LastName   
ELSE CASE WHEN PickupRepresentativeFirstName IS NULL THEN CR.FirstName +' '+ CR.LastName  
ELSE PickupRepresentativeFirstName +' '+ PickupRepresentativeLastName  
END END AS RepName, CR.Address,CR.City,CR.Country AS CountryName,CR.ZipCode,  
@EmailTo AS DefaultToEmailId, CO.Name AS EventCountry,  
CASE WHEN @TotalItemCount=@InvoiceGeneratedCount THEN 0 ELSE 0 END AS IsDuplicateInvoice,  
ISNULL(ValueAddedTax,0) as VatAmount,   
CASE WHEN VAT.Value IS NULL THEN '0' ELSE  
CONVERT(VARCHAR(50), VAT.Value) + CASE WHEN VAT.ValueTypeId = 1 THEN '%' END END AS  VAT,  
ISNULL(VAT.RegistrationNUmber,'') AS VATRegistrationNo,  
ISNULL(VAT.CompanyName,'') AS CompanyName,ISNULL(VAT.CompanyAddress,'')  AS CompanyAddress,  
ED.EventId  AS EventCatId, ISNULL(VAT.CompanyLogo,'') AS CompanyLogo,   
ISNULL(BD1.BankDetail,'') as  IntermediaryBankDetails,ISNULL(VAT.VenueId,0) AS VenueId,  
CRD.Id AS CorpMapId  
FROM CorporateRequests CR  WITH(NOLOCK)  
INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id  
INNER JOIN InvoiceDetails IND ON CRD.Id=IND.CorporateRequestDetailId   
INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id  
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id  
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id  
INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id  
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON ETA.CurrencyId = CM.Id  
LEFT OUTER JOIN CurrencyTypes CM1 WITH(NOLOCK) ON IND.CurrencyId = CM1.Id    
INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id    
INNER JOIN Cities CTM WITH(NOLOCK) ON VD.CityId = CTM.Id  
INNER JOIN States ST WITH(NOLOCK) ON CTM.StateId = ST.Id  
INNER JOIN Countries CO WITH(NOLOCK) ON ST.CountryId = CO.Id  
LEFT OUTER JOIN ValueAddedTaxDetails VAT WITH(NOLOCK) ON VAT.VenueId=VD.Id  
LEFT OUTER JOIN CurrencyTypes CURR WITH(NOLOCK) ON CURR.Id=ETA.LocalCurrencyId  
LEFT OUTER JOIN BankDetails BD WITH(NOLOCK) ON IND.BankDetailId=BD.Id  
LEFT OUTER JOIN BankDetails BD1 WITH(NOLOCK) ON IND.IntermediaryDeatilId=BD1.Id  
LEFT OUTER JOIN Users U WITH(NOLOCK) ON IND.GeneratedBy=U.AltId  
WHERE CR.Id=@CorpOrderId AND CRD.ApprovedStatus IN(1,3)  
AND IND.IsEnabled=1  
END  