CREATE PROC [dbo].[BO_GetMasterStand] 
(          
	@EventId BIGINT          
)          
AS          
BEGIN 
	SELECT 0 AS Id, 'All Stands' As StandName
END  