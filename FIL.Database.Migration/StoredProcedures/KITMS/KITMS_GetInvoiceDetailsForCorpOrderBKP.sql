CREATE PROC [dbo].[KITMS_GetInvoiceDetailsForCorpOrderBKP]   --11829,3                      
(                      
--DECLARE                      
	@CorpMapId BIGINT=4262,                      
	@InvoiceGeneratedBy INT=3                      
)                      
AS                      
BEGIN   
--DECLARE @CorpMapId BIGINT=117
DECLARE @tblInviceNames TABLE(EventId BIGINT, Name NVARCHAR(500))
INSERT INTO @tblInviceNames
VALUES (55,'INV-KZ-CPL2016-'),
(75,'INV-KZ-WISL2018-'),
(2264,'INV-KZ-INDWI2017-'),
(2256,'INV-KZ-AFGHANWI2017-'),
(2240,'REF-KZ-FIFAWC17-'),
(2238,'INV-KZ-CPL2017-'),
(2180,'INV-KZ-PAKWI2017-'),
(2172,'INV-KZ-INDVsAUS2017-'),
(2081,'INV-KZ-Gana2017-'),
(2074,'INV-KZ-ENGWI2017-'),
(2053,'INV-KZ-IPTL2016-'),
(2047,'INV-KZ-MCA2016-'),
(2002,'INV-KZ-PCAIndVsEng2016-'),
(1950,'INV-KZ-PCA2016-'),
(1858,'INV-KZ-IHS2016-'),
(1750,'NV-KZ-TS2016-'),
(1696,'INV-KZ-CPL2016-')

DECLARE @CorpOrderId BIGINT, @AttributeNumber BIGINT 
SELECT @CorpOrderId=CorporateRequestId FROM CorporateRequestDetails WITH(NOLOCK) WHERE Id=@CorpMapId        
SELECT @AttributeNumber=ISNULL(MAX(AttributeNumber),0) FROM InvoiceDetails WITH(NOLOCK)

SELECT DISTINCT                       
CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate, CRD.Id AS CorpMapId,ED.Name AS Match,
ED.Eventid As EventType, TC.Id AS VenueCatName, ISNULL(RequestOrderType,0) AS IsCompOrder,
CONVERT(DECIMAL(18,2),(CASE RequestOrderType WHEN 1 THEN 0 ELSE CASE WHEN ISNULL(VAT.Value,'0')<>'0' THEN
(ISNULL(ETA.Price,0) /((CAST((VAT.Value) AS DECIMAL(18,2))/100)+1))
ELSE ISNULL(ETA.Price,0) END END)) AS PricePerTic,
ISNULL(CRD.ApprovedTickets,0) AS Quantity,
CASE RequestOrderType WHEN 1 THEN 0                       
ELSE ISNULL((ETA.Price * CRD.ApprovedTickets),0) END AS TotalPrice,
CASE RequestOrderType WHEN 1 THEN '0' ELSE [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) 
END AS ConvFee,
CONVERT(DECIMAL(18,2),CASE WHEN (RequestOrderType=1 OR ISNULL([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1),'0')='0') 
THEN 0 ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(18,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100,0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1)),0) 
END END) AS ConvCharge, 
CM.Code AS CurrencyName, CM.Id AS Currencyid,
CASE RequestOrderType WHEN 1 THEN '0' ELSE 
ISNULL([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2),'0') END AS ServiceTax,

CONVERT(DECIMAL(18,2),ISNULL(CASE  RequestOrderType WHEN 1
THEN 0 ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2) ) > 0 THEN
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( (ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100) *
(CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100),0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1))*
(CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100),0)
END
ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( (ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100) +
 CONVERT(DECIMAL(18,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2)),0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1))+
 CONVERT(DECIMAL(18,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2)),0)
END 
END
END,0.00)) AS ServiceCharge,

CONVERT(DECIMAL(18,2),CASE WHEN RequestOrderType=1 THEN 0
ELSE
ISNULL(ETA.Price*CRD.ApprovedTickets,0) +
ISNULL((CASE WHEN (RequestOrderType=1 OR ISNULL([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1),'0')='0') 
THEN 0 ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(18,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100,0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1)),0) 
END END),0) +
ISNULL(CASE  RequestOrderType WHEN 1
THEN 0 ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2) ) > 0 THEN
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( (ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100) *
(CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100),0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1))*
(CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100),0)
END
ELSE
CASE WHEN CHARINDEX('%', [dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1) ) > 0 THEN
ISNULL( (ETA.Price*CRD.ApprovedTickets* 
CONVERT(DECIMAL(4,2),replace([dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1), '%','')) /100) +
 CONVERT(DECIMAL(18,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2)),0)
ELSE ISNULL(ETA.Price*CRD.ApprovedTickets + 
CONVERT(DECIMAL(4,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,1))+
 CONVERT(DECIMAL(18,2),[dbo].[fn_GetTicketFeeDetails](CRD.EventTicketAttributeId,2)),0)
END 
END
END,0.00)
END) AS TotalAmount,
ISNULL(CR.SponsorName,'') AS SponsorName, ISNULL(CR.SponsorId,0) AS SponsorId, GETUTCDATE() AS InvoiceDate,
ISNULL(CURR.Id,'') AS LocalCurrId,                      
ISNULL(CURR.Code,'') AS LocalCurrName,                      
CASE RequestOrderType WHEN 1 THEN 0 ELSE ISNULL(ETA.LocalPrice,0) END AS LocalPricePerTic, 
ISNULL(INV.Name,'INV-KZ-')+ CONVERT(NVARCHAR(10),ISNULL(@AttributeNumber,0)+1) AS InvoiceManualNo,
CO.Name AS EventCountry,
ISNULL(CONVERT(VARCHAR(20),ISNULL(VAT.Value,0)) +''+CASE WHEN ValueTypeId =1 THEN '%' END,0) AS  VAT,                      
ISNULL(VAT.RegistrationNumber,'') AS VATRegistrationNo,
ISNULL (VAT.VenueId,0) AS VenueId,
CONVERT(DECIMAL(18,2),CASE RequestOrderType WHEN 1 THEN 0 ELSE                    
 CASE WHEN ISNULL(VAT.Value,'0')<>'0' THEN        
 (ISNULL(ETA.Price,0)) -(ISNULL(ETA.Price,0) /((Cast((REPLACE(VAT.Value,'%','') ) 
 as Decimal(18,2))/100)+1))
 else ISNULL(VAT.Value,'0') END END) AS VatAmount,
  0 AS Discount
FROM CorporateRequests CR WITH(NOLOCK)
INNER JOIN  CorporateRequestDetails CRD WITH(NOLOCK)  ON CRD.CorporateRequestid = CR.Id
LEFT OUTER JOIN InvoiceDetails IND ON CRD.Id=IND.CorporateRequestDetailId 
INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON CRD.EventTicketAttributeId = ETA.Id
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON ETA.CurrencyId = CM.Id  
INNER JOIN Venues VD WITH(NOLOCK) ON ED.VenueId = VD.Id  
INNER JOIN Cities CTM WITH(NOLOCK) ON VD.CityId = CTM.Id
INNER JOIN States ST WITH(NOLOCK) ON CTM.StateId = ST.Id
INNER JOIN Countries CO WITH(NOLOCK) ON ST.CountryId = CO.Id
LEFT OUTER JOIN ValueAddedTaxDetails VAT WITH(NOLOCK) ON VAT.VenueId=VD.Id
LEFT OUTER JOIN CurrencyTypes CURR WITH(NOLOCK) ON CURR.Id=ETA.LocalCurrencyId
LEFT OUTER JOIn @tblInviceNames INV ON ED.EventId = INV.EventId
WHERE CR.Id=@CorpOrderId AND CRD.ApprovedStatus IN(1,3)
END
