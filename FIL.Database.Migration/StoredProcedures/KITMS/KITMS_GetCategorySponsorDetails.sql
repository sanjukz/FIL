CREATE PROCEDURE [dbo].[KITMS_GetCategorySponsorDetails]                                                       
(                                                          
	 @EventId BIGINT,                                                          
	 @VmccId BIGINT                                                          
)                                                          
AS                                                          
BEGIN  

DECLARE @TempTable TABLE (KM_ID BIGINT, SponsorName VARCHAR(1000), AvailableTic INT, SponsorId BIGINT,        
BlockTic INT, PaidQty INT, ComplimentaryQty INT, UsedPaidQty INT, UsedComplimentaryQty INT, PaidBlockedQty INT, 
CompBlockedQty INT)

INSERT INTO  @TempTable
SELECT KM.Id AS KM_ID,(SELECT SponsorName FROM Sponsors WITH(NOLOCK) WHERE Id = KM.SponsorId) AS SponsorName,                                        
KM.RemainingTickets AS AvailableTic,KM.SponsorId, KM.AllocatedTickets AS BlockTic,
(SELECT ISNULL(SUM(STN.TotalTickets),0)FROM CorporateTransactionDetails STN WITH(NOLOCK) WHERE STN.EventTicketAttributeId = @VmccId AND STN.SponsorId = KM.SponsorId 
AND TransactingOptionId=4) AS PaidQty,                                        
(SELECT ISNULL(SUM(STN.TotalTickets),0)FROM CorporateTransactionDetails STN WITH(NOLOCK) WHERE STN.EventTicketAttributeId = @VmccId AND STN.SponsorId = KM.SponsorId AND
STN.TransactingOptionId=5 )AS ComplimentaryQty,
(ISNULL((SELECT SUM(STA.TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1 AND STA.SponsorId = KM.SponsorId                      
AND STA.EventTicketAttributeId = @VmccId  AND STA.TransactingOptionId IN(4) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = @VmccId AND T.TransactionStatusId=8)),0)) AS UsedPaidQty,                                    
(ISNULL((SELECT SUM(STA.TotalTickets) FROM CorporateTransactionDetails STA WITH(NOLOCK) WHERE STA.IsEnabled = 1 AND STA.SponsorId = KM.SponsorId                                   
AND STA.EventTicketAttributeId = @VmccId  AND STA.TransactingOptionId IN(5) AND STA.TransactionId IN 
(SELECT DISTINCT TransactionId FROM TransactionDetails TD  WITH(NOLOCK)
INNER JOIN Transactions T WITH(NOLOCK) On TD.TransactionId = T.Id AND TD.EventTicketAttributeId = @VmccId AND T.TransactionStatusId=8)),0)) AS UsedComplimentaryQty,                                               
0 AS PaidBlockedQty,0 AS CompBlockedQty                          
FROM CorporateTicketAllocationDetails KM WITH(NOLOCK)             
INNER JOIN EventTicketAttributes VMC WITH(NOLOCK) ON KM.EventTicketAttributeId=VMC.Id                                                                           
WHERE KM.EventTicketAttributeId = @VmccId AND KM.IsEnabled = 1                                            
GROUP BY KM.RemainingTickets,KM.SponsorId,KM.AllocatedTickets,KM.Id,VMC.Id,KM.EventTicketAttributeId     

SELECT * FROM @TempTable

SELECT ISNULL(SUM(BlockTic),0) AS BlockTic, ISNULL(SUM(AvailableTic),0) AS AvailableTic,ISNULL(SUM(PaidQty),0) AS PaidQty, 
ISNULL(SUM(ComplimentaryQty),0) AS ComplimentaryQty,ISNULL(SUM(UsedPaidQty),0) AS UsedPaidQty, 
ISNULL(SUM(UsedComplimentaryQty),0) AS UsedComplimentaryQty,ISNULL(SUM(PaidBlockedQty),0) AS PaidBlockedQty, 
ISNULL(SUM(CompBlockedQty),0) AS CompBlockedQty FROM @TempTable

SELECT ISNULL(VMCC.RemainingTicketForSale,0) AS AvailableTicket, (SELECT ISNULL(SUM(ISNULL(BlockTic,0)),0) FROM @TempTable) AS BlockTic,
CM.Code AS CurrencyName, VMCC.Price, CMLocal.Code +' '+ CONVERT(VARCHAR(200),VMCC.LocalPrice) AS LocalPrice      
FROM EventTicketAttributes VMCC  WITH(NOLOCK)
INNER JOIN CurrencyTypes CMLocal WITH(NOLOCK) ON VMCC.LocalCurrencyId = CMLocal.Id
INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON VMCC.CurrencyId = CM.Id
WHERE VMCC.Id=  @VmccId

SELECT KM.SponsorId, SM.SponsorName FROM Sponsors SM WITH(NOLOCK) 
INNER JOIN CorporateTicketAllocationDetails KM WITH(NOLOCK) ON SM.Id = KM.SponsorId                            
WHERE KM.EventTicketAttributeId = @VmccId and KM.IsEnabled =1       
ORDER BY SM.SponsorName

END