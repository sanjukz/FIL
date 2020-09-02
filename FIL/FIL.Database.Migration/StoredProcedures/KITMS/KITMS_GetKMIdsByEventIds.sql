CREATE PROC [dbo].[KITMS_GetKMIdsByEventIds]  
(  
	@SponsorId BIGINT,  
	@EventIds VARCHAR(1000),  
	@VenueCatId BIGINT  
)  
AS  
BEGIN  
	SELECT Id AS KM_Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE SponsorId=@SponsorId 
	AND EventTicketAttributeId IN( SELECT Id FROm EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId 
	IN (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE TicketCategoryId=@VenueCatId AND
	EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))))
END