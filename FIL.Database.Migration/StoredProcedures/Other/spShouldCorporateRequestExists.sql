CREATE PROC [dbo].[spShouldCorporateRequestExists]
(
	@EventCatId BIGINT,
	@EventId NVARCHAR(500),
	@CorporateName NVARCHAR(500),
	@RepresentativeName NVARCHAR(500),
	@VMCCIds NVARCHAR(500),
	@TicketTypeIds NVARCHAR(500),
	@IsCompTicket INT,
	@retValue int OUTPUT
)
AS
BEGIN

DECLARE @VMCCTable TABLE (RowNo INT IDENTITY(1,1), VMCC_Id BIGINT,Event_Id BIGINT)
DECLARE @EventIdsTable TABLE (RowNo INT IDENTITY(1,1), Event_Id BIGINT)
DECLARE @TicketTypeIdTable TABLE (RowNo INT IDENTITY(1,1), TicketTypeId INT)
	
	INSERT INTO @VMCCTable(VMCC_Id)  
	SELECT * FROM dbo.SplitString(@VMCCIds, ',')

	INSERT INTO @TicketTypeIdTable(TicketTypeId)  
	SELECT * FROM dbo.SplitString(@TicketTypeIds, ',')
	
	DECLARE @RowCount INT, @MatchCount INT=0, @TicketTypeId INT = 0
	SELECT @RowCount = COUNT(*) FROM @VMCCTable
	DECLARE @CorporateId BIGINT

	SELECT @CorporateId = ISNULL(A.Id,0) FROM CorporateRequests A WITH(NOLOCK)
	INNER JOIN CorporateRequestDetails B WITH(NOLOCK) ON A.Id= B.CorporateRequestId
	INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.EventTicketAttributeId = C.Id
	INNER JOIN EventTicketDetails D WITH(NOLOCK) ON C.EventTicketDetailId = D.Id
	INNER JOIN EventDetails E WITH(NOLOCK) ON D.EventDetailId = E.Id AND EventId = @EventCatId
	WHERE SponsorName=RTRIM(LTRIM(@CorporateName)) AND FirstName + ' ' +LastName=@RepresentativeName 
	AND  RequestOrderType=@IsCompTicket AND B.EventTicketAttributeId IN(SELECT VMCC_Id FROm @VMCCTable)

	SELECT @MatchCount = ISNULL(COUNT(*),0) FROM CorporateRequests A WITH(NOLOCK)
	INNER JOIN CorporateRequestDetails B WITH(NOLOCK) ON A.Id= B.CorporateRequestId
	WHERE  A.Id = @CorporateId

	IF(@MatchCount=@RowCount)
	BEGIN
		SET @retValue=1
	END
	ELSE
	BEGIN
		SET @retValue=0
	END
	RETURN @retValue
END
