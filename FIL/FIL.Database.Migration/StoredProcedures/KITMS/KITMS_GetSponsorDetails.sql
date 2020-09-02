CREATE PROC [dbo].[KITMS_GetSponsorDetails]
(
	@SponsorId INT
)
AS
BEGIN
	SELECT A.Id AS SponsorId,SponsorName, CompanyAddress AS AddressLine1,'' AS AddressLine2,
	CompanyCity  AS City, CompanyState AS State, CompanyCountry AS Country, CompanyZipcodeId AS postalcode,
	FirstName As P_FName, LastName AS P_LName, 0 AS P_Gender, Email AS P_EmailId, A.Phonecode+'-'+A.PhoneNumber AS P_MobileNo,
	IdType AS P_IDType, IdNumber AS P_IDNo, '' AS S_FName, '' AS S_LName, 0 AS S_Gender, '' AS S_EmailId, '' AS S_MobileNo, '' AS S_IDType,
	'' AS S_IDNo
	FROM Sponsors A  WITH(NOLOCK)
	 WHERE A.Id=@SponsorId AND A.IsEnabled=1
END