CREATE PROCEDURE [dbo].[BO_GetMoreEvents] --6773                                     
(                                        
	@RetailerId BIGINT                                        
)                                                           
AS                               
BEGIN            
	SELECT E.Id AS EventCatId, E.Name AS EventCatName FROM Events E WITH(NOLOCK)              
	INNER JOIN BoUserVenues UV WITH(NOLOCK)  ON E.Id = UV.EventId              
	-- WHERE E.IsEnabled = 1       
	AND UV.UserId = @RetailerId AND UV.IsEnabled = 1                
	GROUP BY E.Id, E.Name    
End 