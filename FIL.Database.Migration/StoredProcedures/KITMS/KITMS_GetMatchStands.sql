CREATE PROC [dbo].[KITMS_GetMatchStands] 
(  
	@EventId BIGINT  
)  
AS  
BEGIN 

	DECLARE @tblStandCoordinateIds TABLE(CoordinateId INT, VenueCatName VARCHAR(500), VenueCatId INT)
	
	INSERT INTO @tblStandCoordinateIds
	SELECT A.ID, C.Name AS VenueCatName,C.Id As VenueCatId FROM DynamicStadiumCoordinates A WITH(NOLOCK) 
	INNER JOIN DynamicStadiumTicketCategoriesDetails B WITH(NOLOCK) ON A.Id=B.DynamicStadiumCoordinateId
	INNER JOIN TicketCategories C WITH(NOLOCK) ON B.TicketCategoryId=C.Id
	WHERE A.VenueId IN  
    (SELECT VenueId FROM EventDetails WITH(NOLOCK) WHERE Id=@EventId)  
	ORDER BY DisplayName
	
	SELECT CoordinateId AS ID, VenueCatName AS DisplayName FROM(
	SELECT *,ROW_NUMBER() OVER(PARTITION BY VenueCatId ORDER BY VenueCatId) AS RowRank FROM @tblStandCoordinateIds
	) RESULT
	WHERE RESULT.RowRank =1
END
