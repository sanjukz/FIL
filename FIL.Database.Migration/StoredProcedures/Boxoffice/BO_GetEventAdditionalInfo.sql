CREATE PROC [dbo].[BO_GetEventAdditionalInfo]  --1273 
(      
	@EventId BIGINT      
)      
AS      
BEGIN      
	SELECT TermsAndConditions as TCpageUrl, ED.IsEnabled AS EventStatus FROM Events E WITH(NOLOCK)       
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.EventId = E.ID      
	WHERE ED.ID = @EventId      
END      
        
 