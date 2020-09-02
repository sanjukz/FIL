CREATE PROCEDURE [dbo].[KITMS_GetUserMenu]     
(         
  @UserId INT          
)          
AS          
BEGIN    
IF(@UserId IN(4754,4753, 4882,4565, 4752, 5464,5523,6142,4755,4758))    
BEGIN
	SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder,     
	RedirectUrl AS Url, ParentFeatureId AS ParentId    
	FROM Features WITH(NOLOCK) WHERE ParentFeatureId=0 AND IsEnabled =1 AND Id IN(14)            
	UNION
	SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder,     
	RedirectUrl AS Url, ParentFeatureId AS ParentId    
	FROM Features WITH(NOLOCK) WHERE ParentFeatureId=0 AND IsEnabled =1  AND Id NOT IN(14)      
END    
ELSE    
BEGIN    
   SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder,     
   RedirectUrl AS Url, ParentFeatureId AS ParentId    
   FROM Features WITH(NOLOCK) WHERE ParentFeatureId=0 AND IsEnabled =1 AND Id NOT IN(8,14)    
END    
END 