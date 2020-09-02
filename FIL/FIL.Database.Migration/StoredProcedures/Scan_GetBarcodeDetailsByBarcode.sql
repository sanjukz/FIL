  
CREATE PROC [dbo].[Scan_GetBarcodeDetailsByBarcode] --'2603811053'      2603811053   260381105302 
(     
	@Barcode VARCHAR(100) 
) 
AS 
BEGIN 
	--DECLARE @Barcode VARCHAR(100) = '0606506191' 
	DECLARE @TransType INT, @TransID INT, @TicNumId Bigint  
	IF EXISTS(SELECT Id FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE BarcodeNumber=@Barcode ) 
	BEGIN     
		SET @TransID=(SELECT TOP 1 TransactionId FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE BarcodeNumber=@Barcode)     
		SET @TicNumId=(SELECT TOP 1 Id FROM MatchSeatTicketDetails WITH(NOLOCK) WHERE BarcodeNumber=@Barcode) 
	END 
	ELSE If Exists(SELECT Id FROM OfflineBarcodedetails WITH(NOLOCK) WHERE BarcodeNumber=@Barcode) 
	BEGIN     
		SET @TransID=-1     
		SET @TicNumId=(SELECT TOP 1 Id FROM OfflineBarcodedetails WITH(NOLOCK) WHERE BarcodeNumber=@Barcode) 
	END 
	SET @TransType=(SELECT ChannelId FROM Transactions WITH(NOLOCK) WHERE Id=@TransID) 
	IF(@TransType=8) 
	BEGIN     
		SELECT DISTINCT 
		BarcodeNumber AS Barcode, 
		ISNULL(MSTD.EntryGateName,'N/A') AS Gate,  
		(DATEADD(MI,CONVERT(INT, +600),EntryDateTime ))  AS PrintDate, 
		(SELECT TOP 1 S.SponsorName FROM  CorporateTransactionDetails CTD WITH(NOLOCK)     
		INNER JOIN Sponsors S WITH(NOLOCK) ON CTD.SponsorId = S.Id WHERE  CTD.TransactionId =  @TransID) AS Name     
		FROM MatchSeatTicketDetails MSTD WITH(NOLOCK)     
		INNER JOIN MatchLayoutSectionSeats  MLSS WITH(NOLOCK) ON MLSS.Id = MSTD.MatchLayoutSectionSeatId     
		INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id     
		INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id  
		WHERE MSTD.TransactionId = @TransID 
		AND MSTD.Id=@TicNumId     
		ORDER BY (DATEADD(MI,CONVERT(INT, +600),EntryDateTime ))  DESC 
	END 
	ELSE 
	BEGIN     
		IF(@TransID=-1)  
		BEGIN     
			SELECT DISTINCT 
			BarcodeNumber AS Barcode, 
			ISNULL(EntryGateName,'N/A') AS Gate, 
			(DATEADD(MI,CONVERT(INT, +600),EntryDateTime)) AS PrintDate,     
			'N/A' AS Name     
			FROM OfflinebarcodeDetails WITH(NOLOCK)     
			Where Id=@TicNumId     
			ORDER BY (DATEADD(MI,CONVERT(INT, +600),EntryDateTime ))  DESC  
		END  
		ELSE  
		BEGIN  
			SELECT DISTINCT 
			BarcodeNumber AS Barcode, 
			ISNULL(MSTD.EntryGateName,'N/A') AS Gate, 
			(DATEADD(MI,CONVERT(INT, +600),EntryDateTime )) AS PrintDate,     
			FirstName +' '+ LastName AS Name     
			FROM MatchSeatTicketDetails MSTD WITH(NOLOCK)     
			INNER JOIN Transactions T WITH(NOLOCK) ON T.Id = MSTD.TransactionId     
			INNER JOIN MatchLayoutSectionSeats  MLSS WITH(NOLOCK) ON MLSS.Id = MSTD.MatchLayoutSectionSeatId     
			INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id     
			INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id     
			WHERE MSTD.TransactionId = @TransID 
			AND MSTD.Id=@TicNumId    
			ORDER BY (DATEADD(MI,CONVERT(INT, +600),EntryDateTime ))  DESC  
		END 
	END  
END  