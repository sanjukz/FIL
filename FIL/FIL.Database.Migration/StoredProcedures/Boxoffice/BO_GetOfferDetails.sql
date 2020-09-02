CREATE PROC [dbo].[BO_GetOfferDetails]    --2238,0  
(                    
	@EventCatId BIGINT=0,                  
	@OfferId BIGINT=0                    
)                    
AS                    
BEGIN              
	SELECT 1 AS OfferTypeId,'Ticket Offer' AS OfferType,	1 AS Status
	UNION
	SELECT 2 AS OfferTypeId,'Discount Offer' AS OfferType,	1 AS Status
	UNION
	SELECT 3 AS OfferTypeId,'Promotional' AS OfferType	,1 AS Status
	UNION
	SELECT 4 AS OfferTypeId,'Discount For Every X Customer' AS OfferType,	1 AS Status
END   