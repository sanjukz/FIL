CREATE PROC [dbo].[KITMS_GetLayoutIdDetails] 
(
	@LayoutId INT
)
AS
BEGIN

SELECT  VL.Id as LayoutId, VL.LayoutName, VD.Id as VenueId, VD.Name as VenueAddress 
FROM MasterVenueLayouts VL WITH(NOLOCK)
INNER JOIN Venues VD WITH(NOLOCK) ON VL.VenueId = VD.Id
WHERE VL.Id = @LayoutId AND VL.IsEnabled = 1
ORDER BY LayoutName DESC

END


CREATE PROC [dbo].[KITMS_GetVenueLayoutList]
(
	@LayoutName VARCHAR(200),
	@VenueId INT
)
AS
BEGIN

SELECT  Id AS LayoutId, LayoutName FROM MasterVenueLayouts WITH(NOLOCK)
WHERE UPPER(LayoutName) LIKE '%'+UPPER(@LayoutName)+'%'
AND IsEnabled = 1 AND VenueId = @VenueId
ORDER BY LayoutName DESC

END

CREATE PROC [dbo].[KITMS_UpdateVenueLayout]
(
    @LayoutId  INT,
	@LayoutName VARCHAR(500),
	@VenueId INT,
	@UpdatedBy VARCHAR(200)
)
AS
BEGIN
	DECLARE @AltId NVARCHAR(500)
	SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @UpdatedBy
	UPDATE MasterVenueLayouts SET LayoutName = @LayoutName, VenueId=@VenueId , 
	UpdatedBy = @AltId, UpdatedUtc = GETUTCDATE()
	WHERE Id=@LayoutId
END

CREATE PROC [dbo].[KITMS_InsertVenueLayout]
(
	@LayoutName VARCHAR(500),
	@VenueId INT,
	@CreatedBy VARCHAR(500),
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION
DECLARE @AltId NVARCHAR(500)
SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @CreatedBy
IF EXISTS(SELECT LayoutId FROM ITKTS_VenueLayout WHERE LayoutName = @LayoutName  AND VenueId=@VenueId AND Status= 1)    
BEGIN             
	    SET @retValue = -100       
END          
ELSE          
BEGIN
	INSERT INTO MasterVenueLayouts(AltId,LayoutName,VenueId,IsEnabled,CreatedUtc,UpdatedUtc,
	CreatedBy,UpdatedBy)
	VALUES(NEWID(),@LayoutName,@VenueId,1,GETUTCDATE(),NULL,@AltId,NULL)
	SET @retValue = SCOPE_IDENTITY() 
END    
IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN          
    COMMIT TRANSACTION        
END        
 RETURN @retValue        
END 


ALTER FUNCTION udfGetSectionHierarchy
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
FROM MasterVenueLayoutSections WHERE Id = @SectionId

SELECT @VenueLayoutAreaId = VenueLayoutAreaId
FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
IF(@VenueLayoutAreaId=3)
BEGIN
	SELECT @BlockIdTemp = Id,@MasterVenueLayoutSectionId=MasterVenueLayoutSectionId,@VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
	SELECT @VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
END
IF(@VenueLayoutAreaId=2)
BEGIN
	SELECT @LevelIdTemp = Id,@MasterVenueLayoutSectionId=MasterVenueLayoutSectionId,@VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
	SELECT @VenueLayoutAreaId = VenueLayoutAreaId
	FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
END
IF(@VenueLayoutAreaId=1)
BEGIN
	SELECT  @StandIdTemp = Id
	FROM MasterVenueLayoutSections WHERE Id = @MasterVenueLayoutSectionId
END
INSERT INTO @rtnTable
SELECT 1 AS Id, @LevelIdTemp, @BlockIdTemp, @StandIdTemp
return
END

CREATE PROC [dbo].[KITMS_InsertVenueLayoutSection]
(
	@SectionName VARCHAR(500),
	@LayoutId INT,
	@Capacity INT,
	@StandId INT,
	@BlockId INT,
	@LevelId INT,
	@SectionType INT,
	@GateName VARCHAR(200),
	@CreatedBy VARCHAR(200),
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION

--	SectionType => 1 Stand
--	SectionType => 2 Level
--	SectionType => 3 Block
--	SectionType => 4 Section

DECLARE @AltId NVARCHAR(500),@GateId INT =0
SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @CreatedBy
IF NOT EXISTS(SELECT Id FROM EntryGates WHERE Name=@GateName)
BEGIN
	INSERT INTO EntryGates(AltId,Name,StreetInformation,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	VALUES(NEWID(),@GateName,'',1,GETUTCDATE(),NULL,@AltId,NULL)
END
SELECT @GateId =Id FROM EntryGates WHERE Name=@GateName

DECLARE @TotalCapacity INT = 0, @SectionIdTemp INT = 0, @SectionCapacityTemp INT = 0

IF(@LevelId <> 0)
BEGIN
	SELECT @TotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections 
	WHERE MasterVenueLayoutSectionId = @LevelId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @LevelId
END
ELSE IF(@BlockId <> 0)
BEGIN

	SELECT @TotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE 
	MasterVenueLayoutSectionId = @BlockId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @BlockId
END
ELSE IF(@StandId <> 0)
BEGIN
	SELECT @TotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE 
	MasterVenueLayoutSectionId = @StandId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1  GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @StandId
END 
ELSE
BEGIN
	SET @TotalCapacity = 0; 
	SET @SectionIdTemp  = @StandId
END

SELECT @SectionCapacityTemp = ISNULL(Capacity,0) FROM MasterVenueLayoutSections WHERE 
Id = @SectionIdTemp AND MasterVenueLayoutId = @LayoutId
AND IsEnabled=1 

PRINT @SectionCapacityTemp
PRINT @TotalCapacity
IF((@SectionCapacityTemp >= @TotalCapacity + @Capacity) OR @SectionType = 1)
BEGIN

INSERT INTO MasterVenueLayoutSections(AltId,SectionName,MasterVenueLayoutId,MasterVenueLayoutSectionId,Capacity,EntryGateId,
VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)        
VALUES(NEWID(),@SectionName, @LayoutId, @SectionIdTemp,@Capacity, @GateId,@SectionType,  1, GETUTCDATE(), NULL, @AltId, NULL,0)        
     
SET @retValue = SCOPE_IDENTITY()
END
ELSE
BEGIN
SET @retValue = -50
END   
IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN          
    COMMIT TRANSACTION        
END        
 RETURN @retValue        
END 

CREATE PROC [dbo].[KITMS_RemoveVenueLayoutSection]
(
	@SectionId INT,
	@IsReactivate INT,
	@UpdatedBy VARCHAR(200),
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION

--	SectionType => 1 Stand
--	SectionType => 2 Level
--	SectionType => 3 Block
--	SectionType => 4 Section

--DECLARE @SectionId INT = 88

DECLARE @AltId NVARCHAR(500),@ParentId INT =0, @Capacity INT =0
SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @UpdatedBy
SELECT @ParentId = MasterVenueLayoutSectionId,@Capacity = ISNULL(Capacity,0) FROM MasterVenueLayoutSections WHERE Id = @SectionId
DELETE FROM MasterVenueLayoutSections WHERE Id = @SectionId
UPDATE MasterVenueLayoutSections SET Capacity += @Capacity,UpdatedBy = @AltId, UpdatedUtc = GETUTCDATE() WHERE Id=@ParentId 

IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN          
    COMMIT TRANSACTION        
END        
 RETURN @retValue   
END

CREATE PROC [dbo].[KITMS_UpdateVenueLayoutSection]
(
    @SectionId  INT,
	@SectionName VARCHAR(500),
	@LayoutId  INT,
	@Capacity INT,
	@SectionType INT,
	@StandId INT,
	@BlockId INT,
	@LevelId INT,
	@GateName VARCHAR(200),
	@UpdatedBy VARCHAR(200),
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION

--	SectionType => 1 Stand
--	SectionType => 2 Level
--	SectionType => 3 Block
--	SectionType => 4 Section
DECLARE @AltId NVARCHAR(500),@GateId INT =0
SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @UpdatedBy
IF NOT EXISTS(SELECT Id FROM EntryGates WHERE Name=@GateName)
BEGIN
	INSERT INTO EntryGates(AltId,Name,StreetInformation,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	VALUES(NEWID(),@GateName,'',1,GETUTCDATE(),NULL,@AltId,NULL)
END
SELECT @GateId =Id FROM EntryGates WHERE Name=@GateName

DECLARE @ParentTotalCapacity INT = 0, @ChildTotalCapacity INT = 0,
@SectionIdTemp INT = 0, @ParentSectionCapacityTemp INT = 0, @SectionCapacityTempCheck INT =0

DECLARE @LevelIdTemp INT = 0, @BlockIdTemp INT =0, @StandIdTemp INT = 0
SELECT @SectionCapacityTempCheck = ISNULL(Capacity,0)
FROM MasterVenueLayoutSections WHERE Id = @SectionId AND MasterVenueLayoutId = @LayoutId AND IsEnabled=1

DECLARE @TempTable table (Id INT,LevelIdTemp INT, BlockIdTemp INT,	StandIdTemp INT)
INSERT INTO @TempTable
SELECT * FROm udfGetSectionHierarchy(@SectionId)
SELECT @LevelId=LevelIdTemp, @BlockId=BlockIdTemp, @StandId= StandIdTemp
FROM @TempTable WHERE Id = 1

IF NOT EXISTS(SELECT Id FROM MasterVenueLayoutSections WHERE Id = @SectionId AND IsEnabled=1)
BEGIN
IF(@BlockId <> 0)
BEGIN
	SELECT @ParentTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE 
	Id = @BlockId AND MasterVenueLayoutId = @LayoutId
	 AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SELECT @ChildTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @BlockId 
	AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @BlockId
END
ELSE IF(@LevelId <> 0)
BEGIN
	SELECT @ParentTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE Id = @LevelId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SELECT @ChildTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @LevelId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @LevelId
END
ELSE IF(@StandId <> 0)
BEGIN
	SELECT @ParentTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE Id = @StandId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SELECT @ChildTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @StandId AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1 GROUP BY MasterVenueLayoutSectionId
	SET @SectionIdTemp  = @StandId
END 
ELSE
BEGIN
	SET @ParentTotalCapacity = 0
	SET @ChildTotalCapacity = 0
	SET @SectionIdTemp  = @SectionId
END

SELECT @ParentSectionCapacityTemp = ISNULL(Capacity,0) FROM MasterVenueLayoutSections WHERE Id = @SectionIdTemp AND MasterVenueLayoutId = @LayoutId
AND IsEnabled=1
DECLARE @CapacityDifference INT = 0
SET @CapacityDifference = @SectionCapacityTempCheck - @Capacity

DECLARE @MasterVenueLayoutSectionId INT = 0
SET @MasterVenueLayoutSectionId =  CASE WHEN @BlockId<>0 THEN @BlockId WHEN @LevelId<>0 THEN @LevelId ELSE @StandId END

IF(@SectionType = 1)
BEGIN 
	SELECT @ChildTotalCapacity = ISNULL(SUM(Capacity),0) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @SectionId 
	AND MasterVenueLayoutId = @LayoutId
	AND IsEnabled=1  GROUP BY MasterVenueLayoutSectionId
	IF(@ChildTotalCapacity <= @Capacity)
	BEGIN 
		UPDATE MasterVenueLayoutSections SET SectionName = @SectionName, Capacity = @Capacity, EntryGateId = @GateId, 
		MasterVenueLayoutSectionId = @MasterVenueLayoutSectionId,
		UpdatedBy = @AltId, UpdatedUtc = GETUTCDATE()
		WHERE Id=@SectionId 
		SET @retValue = 1
	END
	ELSE
	BEGIN
		--Print 'Not Updated'
		SET @retValue = -50
	END 
END
ELSE IF(@ParentTotalCapacity >= @ChildTotalCapacity - @CapacityDifference)
BEGIN
	--PRINT 'Updated'
	UPDATE MasterVenueLayoutSections SET SectionName = @SectionName, Capacity = @Capacity, EntryGateId = @GateId, 
	MasterVenueLayoutSectionId = @MasterVenueLayoutSectionId,
	UpdatedBy = @AltId, UpdatedUtc = GETUTCDATE()
	WHERE Id=@SectionId 
	SET @retValue = 1
END
ELSE
BEGIN
	--Print 'Not Updated'
	SET @retValue = -50
END 
PRINT @retValue

END
ELSE
BEGIN
	IF(@SectionCapacityTempCheck <> @Capacity)
	BEGIN
	 SET @retValue = -500
	END
	ELSE
	BEGIN
		UPDATE MasterVenueLayoutSections SET SectionName = @SectionName, Capacity = @Capacity, EntryGateId = @GateId, 
		MasterVenueLayoutSectionId = @MasterVenueLayoutSectionId,
		UpdatedBy = @AltId, UpdatedUtc = GETUTCDATE() WHERE Id=@SectionId 
		SET @retValue = 1
	END
END

IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN          
    COMMIT TRANSACTION        
END
RETURN @retValue    
END 

CREATE PROCEDURE [dbo].[KITMS_GetVenueLayoutSectionList]
(
	@LayoutId INT,
	@SectionName VARCHAR(200),
	@SectionType INT,
	@StandId INT,
	@BlockId INT,
	@LevelId INT
)
AS
BEGIN
 IF(@BlockId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @BlockId AND IsEnabled=1
END
ELSE IF(@LevelId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @LevelId AND IsEnabled=1
END
ELSE IF(@StandId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @StandId AND IsEnabled=1
END
ELSE
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND IsEnabled=1
END

END

CREATE PROCEDURE [dbo].[KITMS_GetVenueLayoutAllSectionList]
(
	@LayoutId INT,
	@SectionName VARCHAR(200),
	@SectionType INT,
	@StandId INT,
	@BlockId INT,
	@LevelId INT
)
AS
BEGIN
IF(@BlockId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @BlockId
END
ELSE IF(@LevelId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @LevelId
END
ELSE IF(@StandId <> 0)
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType AND MasterVenueLayoutSectionId = @StandId
END
ELSE
BEGIN
	SELECT Id As SectionId, SectionName FROM MasterVenueLayoutSections WITH(NOLOCk) WHERE MasterVenueLayoutId = @LayoutId AND UPPER(SectionName) LIKE '%' + UPPER(@SectionName) + '%'
	AND VenueLayoutAreaId = @SectionType
END
END

CREATE PROC [dbo].[KITMS_GetVenueLayoutSectionDetails]
(
    @SectionId  INT
)
AS
BEGIN

DECLARE @LevelId INT = 0, @BlockId INT =0, @StandId INT = 0
DECLARE @TempTable table (Id INT,LevelIdTemp INT, BlockIdTemp INT,	StandIdTemp INT)
INSERT INTO @TempTable
SELECT * FROm udfGetSectionHierarchy(@SectionId)
SELECT @LevelId=LevelIdTemp, @BlockId=BlockIdTemp, @StandId= StandIdTemp
FROM @TempTable WHERE Id = 1

SELECT DISTINCT A.Id AS SectionId, SectionName,MasterVenueLayoutId AS LayoutId,Capacity,B.Name As GateName,
@StandId AS StandId, @BlockId AS BlockId,@LevelId As LevelId,
ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
SectionId IN(@StandId)),'') AS StandName,
ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
SectionId IN(@BlockId)),'') AS BlockName,
ISNULL((SELECT TOP 1 ISNULL(SectionName,'') FROM MasterVenueLayoutSections WHERE 
SectionId IN(@LevelId)),'') AS LevelName,
A.IsEnabled AS Status
FROM MasterVenueLayoutSections A
INNER JOIN EntryGates B ON A.EntryGateId = B.Id
WHERE A.Id = @SectionId
END

ALTER PROCEDURE [dbo].[KITMS_InsertSeatLayoutByVenueLayout]
(
	@xml XML,
	@SectionId INT,
	@retValue INT OUTPUT
)
AS
BEGIN
--	SeatType => 0 Space / blank Seat
--	SeatType => 1 Available Seat
--	SeatType => 2 Killed Seat
--	SeatType => 3 Blocked Seat

IF NOT EXISTS(SELECT Id FROM MasterVenueLayoutSections WITH (NOLOCK) WHERE Id = @SectionId)
BEGIN
	DECLARE @AltId NVARCHAR(500),@GateId INT =0
	SELECT @AltId = CreatedBy FROm MasterVenueLayoutSections WITH(NOLOCk) WHERE Id = @SectionId
	
    INSERT INTO MasterVenueLayoutSectionSeats(AltId,SeatTag, MasterVenueLayoutSectionId, RowNumber, ColumnNumber, 
	SeatTypeId, IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
    SELECT NEWID(),
	Seats.value('(SeatTag)[1]','VARCHAR(100)') AS SeatTag,
	@SectionId,
	Seats.value('(RowNumber)[1]','VARCHAR(100)') AS RowNumber, 
	Seats.value('(ColumnNumber)[1]','VARCHAR(100)') AS ColumnNumber,
	Seats.value('(SeatType)[1]','INT') AS SeatTypeId,
	1, GETUTCDATE(), NULL,@AltId,NULL
	FROM
	@xml.nodes('/Seats/Seat')AS TEMPTABLE(Seats)
	
	SET @retValue = 1
END
ELSE
BEGIN
     SET @retValue = -10
END
RETURN @retValue        
END 

CREATE PROCEDURE [dbo].[KITMS_UpdateSeatLayoutByVenueLayout_Temp]
(
	@xml XML,
	@SectionId INT,
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION

--	SeatType => 0 Space / blink Seat
--	SeatType => 1 Available Seat
--	SeatType => 2 Killed Seat
--	SeatType => 3 Blocked Seat

DECLARE @tblSeatLayout TABLE(
	Id INT IDENTITY(1,1),
	SeatId BIGINT,
	SeatTag VARCHAR(200),
	RowNumber VARCHAR(200),
	ColumnNumber VARCHAR(200),
	SeatType int
)

	DECLARE @AltId NVARCHAR(500),@GateId INT =0
	SELECT @AltId = CreatedBy FROm MasterVenueLayoutSections WITH(NOLOCk) WHERE Id = @SectionId

	INSERT INTO @tblSeatLayout(SeatId,SeatTag, RowNumber, ColumnNumber, SeatType)
    SELECT
    Seats.value('(SeatId)[1]','BIGINT') AS SeatId,
	Seats.value('(SeatTag)[1]','VARCHAR(100)') AS SeatTag,
	Seats.value('(RowNumber)[1]','VARCHAR(100)') AS RowNumber, 
	Seats.value('(ColumnNumber)[1]','VARCHAR(100)') AS ColumnNumber,
	Seats.value('(SeatType)[1]','INT') AS SeatType
	FROM
	@xml.nodes('/Seats/Seat')AS TEMPTABLE(Seats)

    UPDATE MasterVenueLayoutSectionSeats SET
	SeatTag = tbl.SeatTag,
	RowNumber = tbl.RowNumber, 
	ColumnNumber = tbl.ColumnNumber,
	SeatTypeId = tbl.SeatType,
	UpdatedBy = @AltId,
	UpdatedUtc = GETUTCDATE()
	FROM @tblSeatLayout tbl
	INNER JOIN MasterVenueLayoutSectionSeats VLS ON tbl.SeatId= VLS.Id
	WHERE VLS.Id = tbl.SeatId AND VLS.MasterVenueLayoutSectionId = @SectionId AND tbl.SeatId <> 0
	
	DELETE FROM MasterVenueLayoutSectionSeats 
	WHERE Id NOT IN (SELECT DISTINCT SeatId FROM @tblSeatLayout WHERE SeatId <> 0) AND MasterVenueLayoutSectionId = @SectionId
	
	INSERT INTO MasterVenueLayoutSectionSeats(AltId,SeatTag, MasterVenueLayoutSectionId, RowNumber, ColumnNumber, 
	SeatTypeId, IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
    SELECT  NEWID(),
	tblLayout.SeatTag AS SeatTag,
	@SectionId,
	tblLayout.RowNumber AS RowNumber, 
	tblLayout.ColumnNumber AS ColumnNumber,
	tblLayout.SeatType  AS SeatTypeId,
	1, GETUTCDATE(), NULL,@AltId,NULL
	FROM
	@tblSeatLayout tblLayout
	WHERE tblLayout.SeatId = 0
		
	SET @retValue = 1

IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN          
    COMMIT TRANSACTION        
END        
 RETURN @retValue        
END 


CREATE PROCEDURE [dbo].[KITMS_GetVenueLayoutSectionSeatLayout] --23
(
		@SectionId INT
)
AS
BEGIN

	SELECT  Id as SeatId,SeatTag,MasterVenueLayoutSectionId as SectionId, RowNumber,ColumnNumber, 
	SeatTypeId as SeatType, CASE WHEN IsEnabled=1 THEN 1 ELSE 0 END as Status 
	FROM MasterVenueLayoutSectionSeats WITH (NOLOCK) WHERE MasterVenueLayoutSectionId = @SectionId
	ORDER BY Id ASC

END

ALTER PROC [dbo].[KITMS_GetLayoutIdByVenue]
(
	@VenueId BIGINT,
	@CreatedBy NVARCHAR(300)
)
AS
BEGIN
	DECLARE @VenueName NVARCHAR(300)
	DECLARE @AltId NVARCHAR(500),@GateId INT =0
	SELECT @AltId = AltId FROm Users WITH(NOLOCk) WHERE UserName = @CreatedBy
	IF EXISTS (SELECT Id FROM MasterVenueLayouts WITH(NOLOCK) WHERE VenueId=@VenueId AND IsEnabled=1)
	BEGIN
		SELECT Id as LayoutId FROM MasterVenueLayouts WITH(NOLOCK) WHERE VenueId=@VenueId AND IsEnabled=1
	END
	ELSE
	BEGIN
		SELECT @VenueName=Name FROM Venues WITH(NOLOCK) WHERE Id=@VenueId
		INSERT INTO MasterVenueLayouts(AltId,LayoutName,VenueId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
		VALUES(NEWID(),@VenueName, @VenueId, 1, GETUTCDATE(), NULL, @AltId,NULL)

		SELECT SCOPE_IDENTITY() AS LayoutId
	END
END

CREATE PROC [dbo].[KITMS_GetSectionDetailsByVenueLayout]--12,0,0,0,0
(
		@SectionId INT,
		@LayoutId INT
)
AS
BEGIN
DECLARE @tblSectionIds TABLE(TempId INT IDENTITY(1,1),SectionId INT)

DECLARE @tblSectionStandIds TABLE(TempId INT IDENTITY(1,1), SectionId INT)
DECLARE @tblStandLevelIds TABLE(TempId INT IDENTITY(1,1), SectionId INT)
DECLARE @tblStandBlockIds TABLE(TempId INT IDENTITY(1,1), SectionId INT)

DECLARE @SectionType INT = 0
SELECT @SectionType = VenueLayoutAreaId FROM MasterVenueLayoutSections WHERE Id=@SectionId

DECLARE @CounterLevel INT = 0, @L INT = 1
DECLARE @CounterBlock INT = 0, @B INT = 1

DECLARE @LevelId INT =0
DECLARE @BlockId INT =0 

IF(@SectionId =0)
BEGIN

    INSERT INTO @tblSectionStandIds
	SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE VenueLayoutAreaId= 1 AND MasterVenueLayoutId =@LayoutId 
		
	--SELECT * FROM @tblSectionStandIds
	
	DECLARE @CounterStand INT = 0, @S INT = 1
	SELECT @CounterStand = COUNT(*) FROM @tblSectionStandIds
	WHILE(@S <= @CounterStand)
	BEGIN
	
		SELECT @SectionId = SectionId FROM @tblSectionStandIds WHERE TempId = @S
		
	    INSERT INTO @tblSectionIds
		SELECT @SectionId
		
		INSERT INTO @tblStandLevelIds
		SELECT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId=@SectionId

		SELECT @CounterLevel = COUNT(*) FROM @tblStandLevelIds
		WHILE(@L <= @CounterLevel)
		BEGIN		
			
			SELECT @LevelId= SectionId FROM @tblStandLevelIds WHERE TempId= @L
							
			INSERT INTO @tblSectionIds
			SELECT @LevelId
			
			--SELECT @LevelId	
			
			INSERT INTO @tblStandBlockIds
			SELECT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId=@LevelId ORDER BY 1
			
			--SELECT * FROM @tblStandBlockIds			
			
			
			SELECT @CounterBlock = COUNT(*) FROM @tblStandBlockIds
			SELECT TOP 1 @B = TempId FROM @tblStandBlockIds ORDER BY TempId ASC
			SELECT TOP 1 @CounterBlock = TempId FROM @tblStandBlockIds ORDER BY TempId DESC
			
			WHILE(@B <= @CounterBlock)
			BEGIN 
				SELECT @BlockId= SectionId FROM @tblStandBlockIds WHERE TempId= @B
				
				INSERT INTO @tblSectionIds
				SELECT @BlockId
				
			   INSERT INTO @tblSectionIds
			   SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @BlockId
				
			SET @B = @B + 1
			END
			
			DELETE FROM @tblStandBlockIds
		SET @L = @L+ 1
		END
	SET @S = @S+ 1
	END

END
ELSE
BEGIN
IF(@SectionType=4)
	BEGIN
		--Level
		INSERT INTO @tblSectionIds
		SELECT @SectionId
END	
ELSE IF(@SectionType=3)
	BEGIN
		--Level
		INSERT INTO @tblSectionIds
		SELECT @SectionId

	   INSERT INTO @tblSectionIds
	   SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @SectionId

END	
ELSE IF(@SectionType=2)
	BEGIN
		--Level
		INSERT INTO @tblSectionIds
		SELECT @SectionId
			
			INSERT INTO @tblStandBlockIds
			SELECT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId=@SectionId
			
			--SELECT * FROM @tblStandBlockIds

			SELECT @CounterBlock = COUNT(*) FROM @tblStandBlockIds
			SELECT TOP 1 @B = TempId FROM @tblStandBlockIds ORDER BY TempId ASC
			SELECT TOP 1 @CounterBlock = TempId FROM @tblStandBlockIds ORDER BY TempId DESC
			
			WHILE(@B <= @CounterBlock)
			BEGIN
				SELECT @BlockId= SectionId FROM @tblStandBlockIds WHERE TempId= @B
				
				INSERT INTO @tblSectionIds
				SELECT @BlockId
				
			   INSERT INTO @tblSectionIds
			   SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @BlockId
				
			SET @B = @B + 1
			END

END		

ELSE IF(@SectionType=1)
	BEGIN
		--Level
		INSERT INTO @tblSectionIds
		SELECT @SectionId
		
		INSERT INTO @tblStandLevelIds
		SELECT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId=@SectionId

		SELECT @CounterLevel = COUNT(*) FROM @tblStandLevelIds
		WHILE(@L <= @CounterLevel)
		BEGIN
		
			
			SELECT @LevelId= SectionId FROM @tblStandLevelIds WHERE TempId= @L
							
			INSERT INTO @tblSectionIds
			SELECT @LevelId
			
			--SELECT @LevelId	
			
			INSERT INTO @tblStandBlockIds
			SELECT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId=@LevelId 
			
			--SELECT * FROM @tblStandBlockIds			
			
			
			SELECT @CounterBlock = COUNT(*) FROM @tblStandBlockIds
			SELECT TOP 1 @B = TempId FROM @tblStandBlockIds ORDER BY TempId ASC
			SELECT TOP 1 @CounterBlock = TempId FROM @tblStandBlockIds ORDER BY TempId DESC
			
			WHILE(@B <= @CounterBlock)
			BEGIN 
				SELECT @BlockId= SectionId FROM @tblStandBlockIds WHERE TempId= @B
				
				INSERT INTO @tblSectionIds
				SELECT @BlockId
				
			   INSERT INTO @tblSectionIds
			   SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = @BlockId
				
			SET @B = @B + 1
			END
			
			DELETE FROM @tblStandBlockIds
		SET @L = @L+ 1
		END
END		
END

DECLARE @tblSectionResult TABLE(SectionId INT, SectionName VARCHAR(500), LayoutId INT,Capacity INT, GateName VARCHAR(500),
StandId INT,  LevelId INT, BlockId INT,  StandName VARCHAR(500),StandIdNew INT,LevelName VARCHAR(500), BlockName VARCHAR(500), SeatStatus INT, 
remainingCapacity INT)


DECLARE @counter INT =0, @i INT =1
SELECT @counter =COUNT(*) FROM @tblSectionIds
WHILE(@i <= @counter)
BEGIN
DECLARE @SectionIdTemp INT = 0
SELECT @SectionIdTemp = SectionId FROM @tblSectionIds WHERE TempId= @i

INSERT INTO @tblSectionResult
SELECT DISTINCT  VL.Id AS SectionId,  VL.SectionName, VL.MasterVenueLayoutId As LayoutId, VL.Capacity, 
ISNULL(EG.Name,'') As GateName, (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)) AS StandId,  
(SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) AS BlockId, (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) AS LevelId,

CASE WHEN (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN CASE WHEN (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN 
CASE WHEN (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN SectionName ELSE 
ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN((SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END
ELSE ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN( (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy((SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)))))),'') END
ELSE ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN(SELECT DISTINCT CASE WHEN (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)) <> 0 THEN 
(SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id))
 ELSE ((SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id))) END AS StandId FROM MasterVenueLayoutSections VL1 WHERE Id =  (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END 
AS StandName,

CASE WHEN (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN CASE WHEN (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN 
CASE WHEN (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN SectionId ELSE 
ISNULL((SELECT TOP 1 SectionId FROM MasterVenueLayoutSections WHERE 
Id IN((SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END
ELSE ISNULL((SELECT TOP 1 SectionId FROM MasterVenueLayoutSections WHERE 
Id IN( (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy((SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)))))),'') END
ELSE ISNULL((SELECT TOP 1 SectionId FROM MasterVenueLayoutSections WHERE 
Id IN(SELECT DISTINCT CASE WHEN (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id)) <> 0 THEN 
(SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(VL.Id))
 ELSE ((SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id))) END AS StandId 
FROM MasterVenueLayoutSections VL1 WHERE Id =  (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END 
 AS StandIdNew,

CASE WHEN (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN 
CASE WHEN (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN SectionName
ELSE ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN(SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE  Id = (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END
ELSE ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN(SELECT DISTINCT CASE WHEN (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 
THEN CASE WHEN (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) = (SELECT DISTINCT Id FROM MasterVenueLayoutSections 
WHERE Id = (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id))) THEN 
(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterVenueLayoutSections 
WHERE Id = (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id))) 
ELSE (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) END 
ELSE (SELECT TOP 1 LevelIdTemp FROM udfGetSectionHierarchy(VL.Id)) END 
AS LevelId FROM MasterVenueLayoutSections VL1 
WHERE Id =  (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END AS LevelName,

CASE WHEN (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)) = 0 THEN  SectionName
ELSE ISNULL((SELECT TOP 1 SectionName FROM MasterVenueLayoutSections WHERE 
Id IN(SELECT DISTINCT Id FROM MasterVenueLayoutSections WHERE 
Id =  (SELECT TOP 1 BlockIdTemp FROM udfGetSectionHierarchy(VL.Id)))),'') END AS BlockName,

CASE WHEN  VL.VenueLayoutAreaId =1  THEN CASE WHEN VL.Capacity = 
(SELECT SUM(CapaCity) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND 
VenueLayoutAreaId=1) THEN 2 
ELSE CASE WHEN VL.Id IN(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterVenueLayoutSectionSeats WHERE MasterVenueLayoutSectionId = VL.Id) THEN 1 ELSE 0 END END
ELSE 
CASE WHEN  VL.VenueLayoutAreaId =3  THEN CASE WHEN VL.Capacity = 
(SELECT SUM(CapaCity) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId = 3) THEN 2 
ELSE CASE WHEN VL.Id IN
(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterVenueLayoutSectionSeats WHERE MasterVenueLayoutSectionId = VL.Id) THEN 1 ELSE 0 END END
ELSE 
CASE WHEN  VL.VenueLayoutAreaId =2  THEN CASE WHEN VL.Capacity = 
(SELECT SUM(CapaCity) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id) THEN 2 
ELSE CASE WHEN VL.Id IN(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterVenueLayoutSectionSeats WHERE MasterVenueLayoutSectionId = VL.Id) 
THEN 1 ELSE 0 END END
ELSE 
CASE WHEN VL.Id IN(SELECT DISTINCT MasterVenueLayoutSectionId FROM MasterVenueLayoutSectionSeats WHERE MasterVenueLayoutSectionId = VL.Id) THEN 1 ELSE 0 END
END
END
END AS SeatStatus,

CASE WHEN  VL.VenueLayoutAreaId =1  THEN CASE WHEN VL.Capacity = 
(SELECT SUM(CapaCity) FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND 
VenueLayoutAreaId=1) THEN 0 
ELSE VL.Capacity - ISNULL((SELECT SUM(CapaCity) FROM MasterVenueLayoutSections WHERE
 MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId=1),0) END
ELSE 
CASE WHEN  VL.VenueLayoutAreaId =3  THEN CASE WHEN VL.Capacity = (SELECT SUM(CapaCity) 
FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId=3) THEN 0 
ELSE VL.Capacity - ISNULL((SELECT SUM(CapaCity) FROM MasterVenueLayoutSections
WHERE MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId=3),0) END
ELSE 
CASE WHEN  VL.VenueLayoutAreaId =2  THEN CASE WHEN VL.Capacity = (SELECT SUM(CapaCity) 
FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId=2) THEN 0 
ELSE VL.Capacity -  ISNULL((SELECT SUM(CapaCity) 
FROM MasterVenueLayoutSections WHERE MasterVenueLayoutSectionId = VL.Id AND VenueLayoutAreaId=2),0) END
ELSE VL.Capacity END
END
END AS RemainingCapacity

FROM MasterVenueLayoutSections VL WITH (NOLOCK)
LEFT OUTER JOIN MasterVenueLayoutSectionSeats VLSS WITH (NOLOCK) ON VL.Id = VLSS.MasterVenueLayoutSectionId
LEFT OUTER JOIN EntryGates EG WITH (NOLOCK) ON VL.EntryGateId = EG.Id
WHERE  VL.MasterVenueLayoutId = @LayoutId AND VL.IsEnabled = 1
AND  VL.Id = @SectionIdTemp

SET @i = @i + 1
END

SELECT * FROM @tblSectionResult

END

CREATE PROC [dbo].[KITMS_GetVenueList] 
(
	@VenueName VARCHAR(200),
	@UserId INT
)
AS
BEGIN

	SELECT  Vd.Id As VenueId, CONVERT(NVARCHAR(500),VD.Name)+', '+ CM.Name 
	 AS VenueAddress 
	FROM Venues VD
	INNER JOIN Cities CM WITH(NOLOCK) ON CM.Id=VD.CityId
	WHERE UPPER(VD.Name) LIKE '%'+UPPER(@VenueName)+'%' 
	AND  Vd.IsEnabled = 1 AND (VD.Name IS NOT NULL AND VD.Name <> '')
	ORDER BY Vd.Name ASC
END

CREATE PROC [dbo].[KITMS_GetTournamentList]
(
	@TournamentName VARCHAR(200)
)
AS
BEGIN

SELECT  ECD.Id As EventCatId, Name As EventCatName FROM Events ECD
WHERE UPPER(ECD.Name) LIKE '%'+UPPER(@TournamentName)+'%'
AND ECD.IsEnabled = 1
ORDER BY ECD.Name DESC

END

CREATE PROC [dbo].[KITMS_GetVenueListByTournament]
(
	@VenueName VARCHAR(200),
	@TournamentId INT
)
AS
BEGIN

SELECT  DISTINCT VD.Id AS VenueId, VD.Name AS VenueAddress FROM Venues VD
INNER JOIN EventDetails ED ON VD.Id  = ED.VenueId
WHERE UPPER(VD.Name) LIKE '%'+UPPER(@VenueName)+'%'
AND VD.IsEnabled = 1 AND ED.EventId = @TournamentId
ORDER BY VD.Name DESC

END

CREATE PROCEDURE [dbo].[KITMS_AssignVenueLayoutToTournament]
(
	@EventCatId INT,
	@VenueId INT,
	@SectionIds VARCHAR(MAX),
	@CreatedBy VARCHAR(200),
	@retValue INT OUTPUT
)
AS
BEGIN
BEGIN TRANSACTION
--DECLARE
--	@EventCatId BIGINT=999352,
--	@EventCatId INT=1502,
--	@VenueId INT=906,
--	@SectionIds VARCHAR(MAX)='13',
--	@CreatedBy VARCHAR(200)='SJ',
--	@retValue INT  

SELECT @CreatedBy = AltId FROm Users WITH(NOLOCk) WHERE UserName = @CreatedBy

DECLARE @tblSectionIds TABLE(Id INT IDENTITY(1,1), SectionId INT)
DECLARE @tblEventIds TABLE(Id INT IDENTITY(1,1), EventCatId INT)

INSERT INTO @tblSectionIds
SELECT Id FROM MasterVenueLayoutSections WHERE Id IN(SELECT KeyWord FROM SplitString(@SectionIds,','))

DECLARE @TournamentLayoutId INT = 0, @LayoutId INT = 0, @EventCatName VARCHAR(500)
SELECT * FROM  MasterVenueLayoutSections WHERE Id IN(SELECT KeyWord FROM SplitString(@SectionIds,','))
SELECT  @LayoutId = ISNULL(MasterVenueLayoutId,0) FROM  MasterVenueLayoutSections WHERE Id IN(SELECT KeyWord FROM SplitString(@SectionIds,','))

SELECT  @EventCatName = ISNULL(Name,'') FROM  Events WHERE Id = @EventCatId

--SELECT @LayoutId
IF EXISTS(SELECT Id FROM  TournamentLayouts WHERE MasterVenueLayoutId = @LayoutId AND EventId = @EventCatId)
BEGIN
	
	SELECT @TournamentLayoutId = Id FROM TournamentLayouts WHERE MasterVenueLayoutId = @LayoutId AND EventId = @EventCatId
END
ELSE
BEGIN
	INSERT INTO TournamentLayouts(AltId,LayoutName,EventId,MasterVenueLayoutId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
	SELECT TOp 1 NEWID(),LayoutName +' - '+ @EventCatName,@EventCatId,@LayoutId,1,GETUTCDATE(),NULL,@CreatedBy,NULL FROM MasterVenueLayouts
	WHERE Id = @LayoutId
	SET @TournamentLayoutId = SCOPE_IDENTITY()
END

--SELECT * FROM @tblSectionIds

INSERT INTO @tblEventIds SELECT @EventCatId

DECLARE @EventCounter INT, @SectionCounter INT
SELECT @EventCounter = COUNT(*) FROM @tblEventIds
SELECT @SectionCounter = COUNT(*) FROM @tblSectionIds

DECLARE @EventCatIdTemp BIGINT, @SectionIdTemp BIGINT

DECLARE @i INT =0, @j INT = 0
DECLARE @LevelIdOld INT, @BlockIdOld INT, @StandIdOld INT

SET @i = 1
WHILE(@i <= @EventCounter)
BEGIN
	SELECT @EventCatIdTemp = EventCatId FROM @tblEventIds WHERE Id= @i
	PRINT 'EventCatId ' +  CONVERT(VARCHAR(200),@EventCatIdTemp)
	SET @j = 1;
	WHILE(@j <= @SectionCounter)
	BEGIN

		DECLARE  @LevelIdNew INT = 0, @BlockIdNew INT = 0, @StandIdNew INT = 0, @SectionIdNew INT
	
		SELECT @SectionIdTemp = SectionId FROM @tblSectionIds WHERE Id= @j 
		DECLARE @TempTable table (Id INT,LevelIdTemp INT, BlockIdTemp INT,	StandIdTemp INT)
		INSERT INTO @TempTable
		SELECT * FROm udfGetSectionHierarchy(@SectionIdTemp)
		SELECT @LevelIdOld=LevelIdTemp, @BlockIdOld=BlockIdTemp, @StandIdOld= StandIdTemp
		FROM @TempTable WHERE Id = 1
			
		IF(@StandIdOld <> 0)
		BEGIN
			IF NOT EXISTS(SELECT Top 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @StandIdOld)
			BEGIN			
				INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
				Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
				SELECT NEWID(),SectionName, @StandIdOld, @TournamentLayoutId, 0,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
				GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
				WHERE Id = @StandIdOld
				SET @StandIdNew = SCOPE_IDENTITY()
				
				INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
				ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
				SELECT NEWID(), SeatTag ,@StandIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
				GETUTCDATE(), NULL,@CreatedBy, NULL
				FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @StandIdOld ORDER BY VLS.SeatId 
			END
			ELSE
			BEGIN
				SELECT TOP 1 @StandIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @StandIdOld
				AND IsEnabled =1
			END
		END
		ELSE
		BEGIN
			SET @StandIdNew = 0
		END
		
		IF(@LevelIdOld <> 0)
		BEGIN
			IF NOT EXISTS(SELECT Top 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelIdOld)
			BEGIN
				DECLARE @LevelStandIdOld INT = 0
				DECLARE @LevelStandIdNew INT = 0
				SET @LevelStandIdOld =  (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(@LevelIdOld))
				IF(@LevelStandIdOld <> 0)
				BEGIN
					IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelStandIdOld)
					BEGIN
						INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
						Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
						SELECT NEWID(),SectionName, @LevelStandIdOld, @TournamentLayoutId, 0,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
						GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
						WHERE Id = @LevelStandIdOld
						SET @LevelStandIdNew = SCOPE_IDENTITY();
				
						INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
						ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
						SELECT NEWID(), SeatTag ,@LevelStandIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
						GETUTCDATE(), NULL,@CreatedBy, NULL
						FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @LevelStandIdOld ORDER BY VLS.SeatId 
					END
					ELSE
					BEGIN
						SELECT TOP 1 @LevelStandIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelStandIdOld
						AND IsEnabled =1
					END
				END
				ELSE
				BEGIN
					SET @LevelStandIdNew = 0
				END
								
				IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelIdOld)
				BEGIN
					INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
					Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
					SELECT NEWID(),SectionName, @LevelIdOld, @TournamentLayoutId, @LevelStandIdNew,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
					GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
					WHERE Id = @LevelIdOld

					SET @LevelIdNew = SCOPE_IDENTITY();
				
					INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
					ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
					SELECT NEWID(), SeatTag ,@LevelIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
					GETUTCDATE(), NULL,@CreatedBy, NULL
					FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @LevelIdOld ORDER BY VLS.SeatId 
				END	
				ELSE
				BEGIN
					SELECT TOP 1 @LevelIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelIdOld
					AND IsEnabled =1
				END				
		END
		ELSE
		BEGIN
			SELECT TOP 1 @LevelIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @LevelIdOld
			AND IsEnabled =1
		END
		END
					
		IF(@BlockIdOld <> 0)
        BEGIN
			IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockIdOld)
			BEGIN
				DECLARE @BlockStandIdOld INT = 0,  @BlockLevelIdOld INT = 0
				DECLARE @BlockStandIdNew INT = 0, @BlockLevelIdNew INT = 0
				DECLARE @TempTable1 table (Id INT,LevelIdTemp INT, BlockIdTemp INT,	StandIdTemp INT)
				INSERT INTO @TempTable1
				SELECT * FROm udfGetSectionHierarchy(@BlockIdOld)
				SELECT @BlockLevelIdOld=LevelIdTemp, @BlockStandIdOld= StandIdTemp
				FROM @TempTable1 WHERE Id = 1
								
			 IF(@BlockStandIdOld <> 0)
			 BEGIN
				IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockStandIdOld)
				BEGIN
					INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
					Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
					SELECT NEWID(),SectionName, @BlockStandIdOld, @TournamentLayoutId, 0,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
					GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
					WHERE Id = @BlockStandIdOld
					SET @BlockStandIdNew = SCOPE_IDENTITY();
				
					INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
					ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
					SELECT NEWID(), SeatTag ,@BlockStandIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
					GETUTCDATE(), NULL,@CreatedBy, NULL
					FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @BlockStandIdOld ORDER BY VLS.SeatId
				END
				ELSE
				BEGIN
					SELECT TOP 1 @BlockStandIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockStandIdOld
					AND IsEnabled =1
				END
	         END
			IF(@BlockLevelIdOld <> 0)
		    BEGIN
				IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockLevelIdOld)
				BEGIN
					DECLARE @BlockLevelStandIdOld INT = 0
					DECLARE @BlockLevelStandIdNew INT = 0
					SET @BlockLevelStandIdOld =  (SELECT TOP 1 StandIdTemp FROM udfGetSectionHierarchy(@BlockLevelIdOld))
					
						IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockLevelStandIdOld)
						BEGIN
							INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
							Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
							SELECT NEWID(),SectionName, @BlockLevelStandIdOld, @TournamentLayoutId, 0,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
							GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
							WHERE Id = @BlockLevelStandIdOld
							SET @BlockLevelStandIdNew = SCOPE_IDENTITY();
				
							INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
							ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
							SELECT NEWID(), SeatTag ,@BlockLevelStandIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
							GETUTCDATE(), NULL,@CreatedBy, NULL
							FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @BlockLevelStandIdOld ORDER BY VLS.SeatId
						END
						ELSE
						BEGIN
							SELECT TOP 1 @BlockLevelStandIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockLevelStandIdOld
							AND IsEnabled =1
						END
						INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
						Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
						SELECT NEWID(),SectionName, @BlockLevelStandIdOld, @TournamentLayoutId, @BlockLevelStandIdNew,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
						GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
						WHERE Id = @BlockLevelIdOld

						SET @BlockLevelIdNew = SCOPE_IDENTITY();
				
						INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
						ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
						SELECT NEWID(), SeatTag ,@BlockLevelIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
						GETUTCDATE(), NULL,@CreatedBy, NULL
						FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @BlockLevelIdOld ORDER BY VLS.SeatId
			    END
			    ELSE
				BEGIN
					SELECT TOP 1 @BlockLevelIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockLevelIdOld
					AND IsEnabled =1
				END	
			END
			IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockIdOld)
			BEGIN
				INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
				Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
				SELECT NEWID(),SectionName, @BlockIdOld, @TournamentLayoutId, @BlockLevelIdNew,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
				GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
				WHERE Id = @BlockIdOld
				SET @BlockIdNew = SCOPE_IDENTITY();

				INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
				ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
				SELECT NEWID(), SeatTag ,@BlockIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
				GETUTCDATE(), NULL,@CreatedBy, NULL
				FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @BlockIdOld ORDER BY VLS.SeatId
			END	
			ELSE
			BEGIN
				SELECT TOP 1 @BlockIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockIdOld
				AND IsEnabled =1
			END	
			END
			ELSE
			BEGIN
				SELECT TOP 1 @BlockIdNew = ISNULL(Id,0) FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @BlockIdOld
				AND IsEnabled =1
			END	
        END

		IF NOT EXISTS(SELECT TOP 1 Id FROM TournamentLayoutSections WHERE MasterVenueLayoutSectionId = @SectionIdTemp)
		BEGIN
			DECLARE @MasterVenueLayoutSectionId INT = 0
			SET @MasterVenueLayoutSectionId =  CASE WHEN @BlockIdNew<>0 THEN @BlockIdNew WHEN @LevelIdNew<>0 THEN @LevelIdNew ELSE @StandIdNew END

			INSERT INTO TournamentLayoutSections(AltId,SectionName,MasterVenueLayoutSectionId,TournamentLayoutId,TournamentLayoutSectionId,
			Capacity,EntryGateId,VenueLayoutAreaId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy,SectionId)
			SELECT NEWID(),SectionName, @SectionIdTemp, @TournamentLayoutId, @MasterVenueLayoutSectionId,Capacity, EntryGateId,VenueLayoutAreaId, IsEnabled, 
			GETUTCDATE(), NULL,@CreatedBy, NULL,0 FROM MasterVenueLayoutSections
			WHERE Id = @SectionIdTemp
			
			SET @SectionIdNew = SCOPE_IDENTITY();
			INSERT INTO TournamentLayoutSectionSeats(AltId,SeatTag,TournamentLayoutSectionId,RowNumber,ColumnNumber,RowOrder,
			ColumnOrder,SeatTypeId,IsEnabled,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
			SELECT NEWID(), SeatTag ,@SectionIdNew, RowNumber, ColumnNumber, RowOrder,ColumnOrder,SeatTypeId, IsEnabled, 
			GETUTCDATE(), NULL,@CreatedBy, NULL
			FROM MasterVenueLayoutSectionSeats VLS WHERE MasterVenueLayoutSectionId = @SectionIdTemp ORDER BY VLS.SeatId
		END	
		PRINT 'Section Id New ' +  CONVERT(VARCHAR(200),@SectionIdNew)
		SET @retValue = 1

		SET @j =@j + 1
	END

	SET @i =@i + 1
END

IF(@@ERROR <> 0)        
BEGIN        
 SET @retValue = 0        
 ROLLBACK TRANSACTION        
END        
ELSE        
BEGIN  
	PRINT 'Committed'        
    COMMIT TRANSACTION        
END        
 RETURN @retValue        
END 