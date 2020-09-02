CREATE FUNCTION [dbo].[udfGetSectionHierarchy]
(
	@SectionId INT
)
RETURNS  @rtnTable TABLE 
(
	Id INT,
    LevelIdTemp INT,
    BlockIdTemp INT,
	StandIdTemp INT
)
AS
BEGIN
--	SectionType => 1 Stand
--	SectionType => 2 Level
--	SectionType => 3 Block
--	SectionType => 4 Section
DECLARE @LevelIdTemp INT = 0, @BlockIdTemp INT =0, @StandIdTemp INT = 0, @MasterVenueLayoutSectionId INT = 0, @VenueLayoutAreaId INT =1

SELECT @MasterVenueLayoutSectionId = MasterVenueLayoutSectionId
FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @SectionId

SELECT @VenueLayoutAreaId = VenueLayoutAreaId
FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
IF(@VenueLayoutAreaId=3)
BEGIN
	SELECT @BlockIdTemp = Id,@MasterVenueLayoutSectionId=MasterVenueLayoutSectionId,@VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
	SELECT @VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
END
IF(@VenueLayoutAreaId=2)
BEGIN
	SELECT @LevelIdTemp = Id,@MasterVenueLayoutSectionId=MasterVenueLayoutSectionId,@VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
	SELECT @VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
END
IF(@VenueLayoutAreaId=1)
BEGIN
	SELECT  @StandIdTemp = Id
	FROM MasterVenueLayoutSections  WITH(NOLOCK) WHERE Id = @MasterVenueLayoutSectionId
END
INSERT INTO @rtnTable
SELECT 1 AS Id, @LevelIdTemp, @BlockIdTemp, @StandIdTemp
return
END