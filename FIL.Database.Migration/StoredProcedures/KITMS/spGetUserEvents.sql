CREATE PROCEDURE [dbo].[spGetUserEvents] --4565  
(  
 @UserId BIGINT  
)  
AS  
BEGIN  
 SELECT DISTINCT ECD.Id AS EventCatId,ECD.Name AS EventCatName  
 FROM Events ECD WITH(NOLOCK)  
 INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.EventId=ECD.Id    
 WHERE ECD.Id IN(SELECT DISTINCT EventId FROm EventsUserMappings WITH(NOLOCK) WHERE UserId = @UserId)   
 --AND ED.StartDateTime>=DATEADD(DD,-45,GETUTCDATE())  
 ORDER BY ECD.Id DESC  
END  