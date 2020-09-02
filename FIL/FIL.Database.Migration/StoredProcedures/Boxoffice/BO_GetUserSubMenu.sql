CREATE Procedure [dbo].[BO_GetUserSubMenu]               
(                                  
  @UserID BIGINT,                    
  @ParentMenu_Id bigint                                  
)                                  
AS                                  
BEGIN    
    
SELECT RM.Menu_Id,RM.MenuUrl as [URL], RM.MenuName, RM.DisplayOrder, RM.EventCategory, Ur.MenuId AS HasRight     
FROM BO_MenuMaster RM WITH(NOLOCK)    
INNER JOIN BO_UserMenuRights UR WITH(NOLOCK) ON  Rm.Menu_Id = UR.MenuId    
WHERE RM.status=1 and UR.[User_Id] = @UserID  and ParentMenu_Id =@ParentMenu_Id       
ORDER BY RM.DisplayOrder     
                            
END   