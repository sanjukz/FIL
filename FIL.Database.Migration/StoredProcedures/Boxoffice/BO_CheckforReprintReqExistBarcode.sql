CREATE PROC [dbo].[BO_CheckforReprintReqExistBarcode] -- 851,'0606507040'          
(
	@TransId BIGINT,
	@barcode VARCHAR(100)
)
AS
BEGIN 

SELECT * FROM RePrintRequestDetails WITH(NOLOCK) 
WHERE (MatchSeatTicketDetaildId=@TransId AND ISNULL(IsApproved,0)=1 AND ISNULL(IsReprinted,0)=0 AND BarcodeNumber=@barcode)or              
(MatchSeatTicketDetaildId=@TransId AND ISNULL(IsApproved,0)=0 AND ISNULL(IsReprinted,0)=0 AND BarcodeNumber=@barcode)              

END