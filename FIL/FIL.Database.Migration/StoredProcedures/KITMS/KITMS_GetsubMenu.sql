CREATE PROCEDURE [dbo].[KITMS_GetsubMenu]  
(      
 @MenuId INT, 
 @Userid int    
)      
AS      
BEGIN      
 SELECT @UserId AS UserId, Id AS MenuId, FeatureName AS MenuName, 0 AS DisplayOrder, RedirectUrl AS Url, ParentFeatureId AS ParentId
  FROM Features WITH(NOLOCK) WHERE ParentFeatureId=@MenuId AND IsEnabled =1
END