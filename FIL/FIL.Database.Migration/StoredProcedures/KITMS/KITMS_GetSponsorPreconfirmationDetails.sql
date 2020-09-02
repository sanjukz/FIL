CREATE PROCEDURE [dbo].[KITMS_GetSponsorPreconfirmationDetails]                  
(                                        
	@KM_ID BIGINT                                                       
)                                        
AS                                        
BEGIN                        
                     
DECLARE @EventId BIGINT, @SponsorId BIGINT, @VMCC_Id BIGINT, @SPT_Id BIGINT                    
SELECT @SponsorId=SponsorId,@VMCC_Id = EventTicketAttributeId FROM  
CorporateTicketAllocationDetails WITH(NOLOCK) WHERE  Id = @KM_ID 
	   
SELECT S.SponsorName, SATD.SponsorId,ED.Id AS EventId,E.Name AS EventCatName, CONVERT(VARCHAR(17),ED.StartDateTime,113) AS EventStartDate,
E.Id AS EventCatId, ED.Name AS EventName, V.Name +', '+ C.Name AS VenueAddress, TC.Name AS Category, ED.VenueId, EA.LocalPrice AS PricePerTic,
EA.Id AS Vmcc_Id, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,1),0) AS ConvenceCharge, ISNULL([dbo].[fn_GetTicketFeeDetails](@VMCC_Id,2),0) AS TicketServiceTax,
CM.Code AS CurrencyName, CM.Code AS LocalCurrencyName
FROM Sponsors S WITH(NOLOCK)
INNER JOIN CorporateTicketAllocationDetails SATD WITH(NOLOCK) ON S.Id=SATD.SponsorId AND SATD.Id = @KM_ID
INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON SATD.EventTicketAttributeId=EA.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.LocalCurrencyId=CM.Id
INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id
INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id
INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id
INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id
INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id

SELECT SponsorName,FirstName AS P_FName,LastName AS P_LName, Email AS P_EmailId, PhoneCode +'-'+ PhoneNumber AS P_MobileNo, '' AS P_IDType, '' AS  P_IDNo
FROM Sponsors WITH(NOLOCK) WHERE Id=@SponsorId  
                             
END