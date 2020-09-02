CREATE PROC [dbo].[KITMS_UpdateSponsorDetails]   
(
	@SponsorId INT,  
	@SponsorName VARCHAR(500),   
	@AddressLine1 VARCHAR(100),   
	@AddressLine2 VARCHAR(100),   
	@City VARCHAR(30),   
	@State VARCHAR(30),  
	@Country VARCHAR(30),  
	@PostalCode VARCHAR(30),  
	@PCFirstName VARCHAR(30),  
	@PCLastName VARCHAR(30),  
	@PCGender VARCHAR(30),  
	@PCEmail VARCHAR(50),   
	@PCMobile VARCHAR(50),  
	@PCIDType VARCHAR(50),   
	@PCIDNo VARCHAR(50),   
	@SCFirstName VARCHAR(30),   
	@SCLastName VARCHAR(30),   
	@SCGender VARCHAR(30),   
	@SCEmail VARCHAR(50),   
	@SCMobile VARCHAR(30),    
	@SCIDType VARCHAR(50),   
	@SCIDNo VARCHAR(50),
	@Updatedby VARCHAR(200)
)  
AS   
BEGIN  
DECLARE @PhoneCode VARCHAR(10), @PhoneNumber VARCHAR(20), @ZipCodeId INT, @AltId NVARCHAR(200)
SELECT CHARINDEX('-',@PCMobile)
SET @PhoneCode = SUBSTRING(@PCMobile, 0, CHARINDEX('-',@PCMobile));
SET @PhoneNumber = SUBSTRING(@PCMobile, CHARINDEX('-',@PCMobile)+1, LEN(@PCMobile));
SELECT @ZipCodeId = ID FROM Zipcodes WITH(NOLOCK) WHERE Postalcode = @PostalCode
SELECT @AltId = Altid FROm Users WITH(NOLOCK) WHERE UserName = @Updatedby

UPDATE Sponsors SET SponsorName = @SponsorName,FirstName = @PCFirstName,LastName=@PCLastName,Email=@PCEmail,PhoneCode=@PhoneCode,
PhoneNumber =@PhoneNumber ,CompanyAddress = @AddressLine1,CompanyCity = @City,
CompanyState = @State,CompanyCountry = @Country,CompanyZipcodeId = @PostalCode,
IsEnabled=1,UpdatedUtc=GETDATE(),UpdatedBy = @AltId,IdType = @PCIDType, IdNumber = @PCIDNo
WHERE Id = @SponsorId

END