CREATE PROC [dbo].[spGetMasterDynamicLayout] --3
(
   @MasterVenueLayoutId BIGINT 
)      
AS      
BEGIN      

SELECT DISTINCT B.Id,Name, DisplayName, SectionName, Capacity, SectionCoordinates,SectionTextCoordinates,
CircleRectangleValue, Styles, IsDisplay, CASE WHEN B.IsEnabled = 1 THEN 1 ELSE 0 END AS IsEnabled,
A.MasterVenueLayoutSectionId,
ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WITH(NOLOCK) WHERE 
Id IN(SELECT  TOP 1 StandIdTemp FROM udfGetSectionHierarchy(A.MasterVenueLayoutSectionId))),'') AS StandName,
ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WITH(NOLOCK) WHERE 
Id IN(SELECT TOP 1 BlockIdTemp  FROM udfGetSectionHierarchy(A.MasterVenueLayoutSectionId))),'') AS BlockName,
ISNULL((SELECT TOP 1 ISNULL(SectionName,'') FROM MasterVenueLayoutSections WITH(NOLOCK) WHERE 
Id IN(SELECT TOP 1 LevelIdTemp  FROM udfGetSectionHierarchy(A.MasterVenueLayoutSectionId))),'') AS LevelName,
VenueLayoutAreaId
FROM MasterDynamicStadiumCoordinates B  WITH(NOLOCK)
LEFT OUTER JOIN MasterDynamicStadiumSectionDetails A WITH(NOLOCK) ON B.Id =A.MasterDynamicStadiumCoordinateId
LEFT OUTER JOIN MasterVenueLayoutSections C WITH(NOLOCK) ON A.MasterVenueLayoutSectionId =C.Id
WHERE B.MasterVenueLayoutId= @MasterVenueLayoutId

END 
