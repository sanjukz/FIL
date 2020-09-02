DECLARE @CountryName NVARCHAR(1000), @StateName NVARCHAR(1000), @CityName NVARCHAR(1000), @Address NVARCHAR(1000), @CountryCode NVARCHAR(1000), @Latitude NVARCHAR(1000), @Longitude NVARCHAR(1000)

DECLARE VenueCursor CURSOR FOR   
SELECT DISTINCT
LTRIM(RTRIM(CountryName)), 
LTRIM(RTRIM(Country_Code)), 
LTRIM(RTRIM(State_Name)), 
LTRIM(RTRIM(City_Name)), 
LTRIM(RTRIM(VenueAddress)),
LTRIM(RTRIM(VD.Latitude)),
LTRIM(RTRIM(VD.Longitude)) 
FROM LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_VenueDetails VD WITH (NOLOCK)
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTS_City_MST CM WITH (NOLOCK) ON VD.VenueCity = CM.City_id
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTS_State_MST SM WITH (NOLOCK) ON CM.State_Id = SM.State_Id
INNER JOIN LinkToLiveDB.ITKTS.dbo.ITKTS_Country_MST CCM WITH (NOLOCK) ON SM.Country_Id = CCM.CountryId
WHERE VD.VenueId IN 
(SELECT DISTINCT EventVenue From LinkToLiveDB.ITKTS.dbo.ITKTSEVNT_EventDetails 
WHERE EventType in (13,14,1758,1759,1760,1761,1762,1763,1764,1765,1766,1767,1769,1875,1908,2494,2550,2880,4361,4367,4371,4373,4376))

OPEN VenueCursor  

FETCH NEXT FROM VenueCursor   
INTO @CountryName, @CountryCode, @StateName, @CityName, @Address, @Latitude, @Longitude

WHILE @@FETCH_STATUS = 0  
BEGIN  
	DECLARE @CountryId INT, @StateId INT, @CityId INT
	IF EXISTS(SELECT Id FROM Countries WITH (NOLOCK) WHERE LTRIM(RTRIM(Name)) = @CountryName)
	BEGIN
		SELECT @CountryId = Id 
		FROM Countries WITH (NOLOCK) 
		WHERE LTRIM(RTRIM(Name)) = @CountryName
	END
	ELSE
	BEGIN
		INSERT INTO Countries (Name,IsoAlphaTwoCode,IsoAlphaThreeCode,Phonecode,IsEnabled,CreatedUtc,CreatedBy,AltId)
		SELECT @CountryName, '', '', @CountryCode, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NEWID()		
		SET @CountryId = SCOPE_IDENTITY()
	END
	
	IF EXISTS(SELECT Id FROM States WHERE LTRIM(RTRIM(Name)) = @StateName)
	BEGIN
		SELECT @StateId = Id 
		FROM States WITH (NOLOCK) 
		WHERE LTRIM(RTRIM(Name)) = @StateName
	END
	ELSE
	BEGIN
		INSERT INTO States (Name,Abbreviation,CountryId,IsEnabled,CreatedUtc,CreatedBy,AltId)
		SELECT @StateName, '', @CountryId, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NEWID()
		SET @StateId = SCOPE_IDENTITY()
	END

	IF EXISTS(SELECT Id FROM Cities WHERE LTRIM(RTRIM(Name)) = @CityName)
	BEGIN
		SELECT @CityId = Id 
		FROM Cities WITH (NOLOCK) 
		WHERE LTRIM(RTRIM(Name)) = @CityName
	END
	ELSE
	BEGIN
		INSERT INTO Cities (Name,StateId,IsEnabled,CreatedUtc,CreatedBy,AltId)
		SELECT @CityName, @StateId, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781', NEWID()
		SET @CityId = SCOPE_IDENTITY()
	END

	IF NOT EXISTS(SELECT Id FROM Venues WHERE LTRIM(RTRIM(Name)) = @Address)	
	BEGIN
		INSERT INTO Venues (AltId,Name,AddressLineOne,AddressLineTwo,CityId,Latitude,Longitude,IsEnabled,CreatedUtc,CreatedBy)
		SELECT NEWID(), @Address, '', '', @CityId, @Latitude, @Longitude, 1, GETUTCDATE(), 'C043DDEE-D0B1-48D8-9C3F-309A77F44781' 		
		PRINT @Address + ' Added Sucessfully '
	END	
	ELSE
	BEGIN
		PRINT 'Already Added'
	END

	FETCH NEXT FROM VenueCursor   
	INTO @CountryName, @CountryCode, @StateName, @CityName, @Address, @Latitude, @Longitude
END   
CLOSE VenueCursor;  
DEALLOCATE VenueCursor;  