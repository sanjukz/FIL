CREATE FUNCTION [dbo].[fn_GetTicketFeeDetails] --4200,1
(
	@EventTicketAttributeId BIGINT, 
	@FeeId INT
)
RETURNS VARCHAR(200)
AS
BEGIN
DECLARE @Charge VARCHAR(200)

SELECT @Charge = CONVERT(VARCHAR(200),Value) +''+ CASE WHEN ValueTypeId =1 THEN '%' ELSE ''  END 
FROM TicketFeeDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @EventTicketAttributeId AND FeeId = @FeeId
RETURN (@Charge)

END