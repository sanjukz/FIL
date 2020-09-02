CREATE proc [dbo].[BOUser_GetEventByRetailer]  --355830               
(                            
	@RetailerId BIGINT                            
)     
AS     
BEGIN     
SET NOCOUNT ON     
	SELECT DISTINCT EC.Id as EventCatId, EC.Name as EventCatName,EC.EventCategoryId as p_id 
	FROM BoUserVenues R WITH(NOLOCK) 
	JOIN Events EC WITH(NOLOCK) ON R.EventId = EC.ID                                     
	WHERE --p_id in (1,2,4,5,6,7,8,16) AND                             
	R.IsEnabled=1 AND EC.IsEnabled=1         
	--Ec.EventCatId in(818,837,834,848)       
	AND R.UserId IN (SELECT DISTINCT UserId FROM BoxofficeUserAdditionalDetails WITH(NOLOCK) 
	WHERE Parentid=@RetailerId)    
END   