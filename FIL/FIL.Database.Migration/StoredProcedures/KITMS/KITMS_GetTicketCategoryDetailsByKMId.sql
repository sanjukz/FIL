CREATE PROC [dbo].[KITMS_GetTicketCategoryDetailsByKMId]   
(  
 
	@KM_Id BIGINT 
)  
AS  
BEGIN  
	DECLARE @EventId BIGINT, @SponsorId BIGINT, @VMCC_Id BIGINT, @SPT_Id BIGINT                    
	SELECT @SponsorId=SponsorId,@VMCC_Id = EventTicketAttributeId FROM  
	CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_ID 
	   
	SELECT E.Id AS EventCatId, ED.Id AS EventId,ED.Name AS EventName, CONVERT(VARCHAR(17), ED.StartDateTime, 113) AS EventStartDate,
	EA.Id AS VMCC_Id, EA.LocalPrice AS PricePerTic, EA.LocalPrice As LocalPricePerTic, V.Name +', '+ C.Name AS VenueAddress, TC.Name AS VenueCatName, TC.Name AS VenueDisplayName,
	CM.Code AS LocalCurrencyName, CM.Id AS Currencyid, CM.Code AS CurrencyName, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,1),0) AS ConvenceCharge,
	ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,2),0) AS TicketServiceTax
	FROM  EventTicketAttributes EA WITH(NOLOCK) 
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.LocalCurrencyId=CM.Id AND EA.Id = @VMCC_Id
	INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
	INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
	INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
	INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
	INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
END