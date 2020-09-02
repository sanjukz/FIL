CREATE PROC [dbo].[KITMS_GetSponsorId]       
(          
	@KITMSMasterId BIGINT     
)          
AS           
BEGIN                  
	SELECT SponsorId FROM CorporateTicketAllocationDetails WITH(NOLOCK)  WHERE Id=@KITMSMasterId AND IsEnabled=1  
END 