CREATE PROC [dbo].[KITMS_InsertIpDetails]  
(  
   @TransactionId BIGINT,
   @IPAddress VARCHAR(50),  
   @CountryCode VARCHAR(50),  
   @CountryName VARCHAR(500),  
   @RegionCode VARCHAR(500),
   @RegionName VARCHAR(500),
   @City VARCHAR(50),  
   @Zipcode VARCHAR(500),  
   @TimeZone VARCHAR(500),
   @Latitude VARCHAR(500),
   @Longitude VARCHAR(500)
)  
AS   
BEGIN  
	DECLARE @IpDetailId INT = 0;
	IF NOT EXISTS(SELECT Id FROM IpDetails WITH(NOLOCK) WHERE IPAddress = @IPAddress)
	BEGIN
		INSERT INTO IpDetails(IPAddress,CountryCode,CountryName,RegionCode,RegionName,City,Zipcode,TimeZone,Latitude,Longitude,
		MetroCode,CreatedUtc,UpdatedUtc,CreatedBy,UpdatedBy)
		VALUES(@IPAddress,@CountryCode,@CountryName,@RegionCode,@RegionName,@City,@Zipcode,@TimeZone,@Latitude,@Longitude,
		NULL,GETUTCDATE(),NULL,NEWID(),NULL)

		SET @IpDetailId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		SELECT @IpDetailId = Id FROM IpDetails WITH(NOLOCK) WHERE IPAddress = @IPAddress
	END

	UPDATE Transactions SET IPDetailId = @IpDetailId WHERE Id = @TransactionId
END 