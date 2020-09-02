CREATE Procedure [dbo].[BO_GetUserParentMenu]               
(                                    
  @UserID BIGINT                                   
)                                    
AS                                    
BEGIN   
  
SELECT RM.Menu_Id,RM.MenuName,RM.DisplayOrder,RM.EventCategory,RM.MenuUrl AS [URL], Ur.MenuId AS HasRight    
FROM BO_MenuMaster RM WITH(NOLOCK)    
INNER JOIN BO_UserMenuRights UR WITH(NOLOCK) ON  Rm.Menu_Id = UR.MenuId  --AND    UR.[User_Id] = @UserID    
WHERE RM.status=1 and RM.ParentMenu_Id IS NULL  AND    UR.[User_Id] = @UserID    
ORDER BY RM.DisplayOrder     
                               
END   