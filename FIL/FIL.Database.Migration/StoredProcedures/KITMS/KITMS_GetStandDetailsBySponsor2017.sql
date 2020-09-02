CREATE PROC [dbo].[KITMS_GetStandDetailsBySponsor2017]    
(      
 @VenueId BIGINT,     
 @EventCatId BIGINT,         
 @SponsorId BIGINT       
)    
AS                                                   
BEGIN           
    
DECLARE @tblMatches Table (EventId BIGINT)    
DECLARE @EventType BIGINT                             
 DECLARE @tblEventTicketCategory TABLE(Id BIGINT)  
 DECLARE @tblVMCCIds Table (VMCC_Id BIGINT)  
SET @EventType = @EventCatId      
    
INSERT INTO @tblMatches    
SELECT Id FROM EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND EventId=@EventType      
    
INSERT INTO @tblEventTicketCategory    
  SELECT DISTINCT TicketCategoryId FROm EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN    
  (SELECT Id FROm EventDetails WITH(NOLOCK) WHERE VenueId = @VenueId AND EventId = @EventCatId)    
   INSERT INTO @tblVMCCIds    
  SELECT Id FROm EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId IN     
  (SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(SELECT EventId FROM @tblMatches)    
  AND TicketCategoryId IN(SELECT Id from @tblEventTicketCategory))   
    
SELECT  DISTINCT TC.ID AS VenueCatId,-- VMCC.Status,    
 TC.Name  AS Category,    
 (SELECT ISNULL(COUNT(*),0) FROM @tblMatches) AS TotalMatches,    
 (ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                                 
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = KM.EventTicketAttributeId    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId                                      
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND KM.IsEnabled = 1),0)) AS SponsoredTickets,   
      
 (SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA WITH(NOLOCK)    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalTickets,    
    
 (SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA   WITH(NOLOCK)  
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalAvailTickets,     
    
 (SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA  WITH(NOLOCK)   
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) RemainingTics,    
     
    
 (SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA   WITH(NOLOCK)  
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalTics,   
   0 AS BlockedTic,  
  
 (ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                                 
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK)  INNER JOIN Sponsors SM WITH(NOLOCK) on SM.Id = KM.SponsorId AND SM.SponsorTypeId=0    
 INNER JOIN EventTicketAttributes ETATemp WITH(NOLOCK) on ETATemp.Id = KM.EventTicketAttributeId    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETATemp.EventTicketDetailId                                      
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND KM.IsEnabled = 1  AND SM.SponsorTypeId=0),0)) SeatKills,   
    
(ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)    
 INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId    
 INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2) AND STD.IsEnabled=1    
 INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId    
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0))  ttlUsedPaidCompSeats,    
  
 (ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)    
 INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId    
 INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId = 2 AND STD.IsEnabled=1    
 INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId    
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId  AND T.TransactionStatusId=8),0)) Complimentary,    
    
 (ISNULL((SELECT SUM(TD.TotalTickets) FROm Transactions T WITH(NOLOCK)    
 INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId    
 INNER JOIN CorporateTransactionDetails STD WITH(NOLOCK) ON T.Id = STD.TransactionId AND STD.TransactingOptionId IN(1,2)    
 INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on TD.EventTicketAttributeId = SM.Id    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId    
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  BlockUsed,    
  
 (ISNULL((SELECT SUM(TD.TotalTickets)                                                                                 
 FROM  Transactions T WITH(NOLOCK)    
 INNER JOIN TransactionDetails TD WITH(NOLOCK) on T.Id = TD.TransactionId AND T.TransactionStatusId =8 --AND T.ChannelId = 1    
 INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = TD.EventTicketAttributeId    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId        
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND T.TransactionStatusId=8),0))  PublicSales,    
    
 (ISNULL((SELECT SUM(TD.TotalTickets)                                                                                 
 FROM  Transactions T WITH(NOLOCK)    
 INNER JOIN TransactionDetails TD WITH(NOLOCK) on T.Id = TD.TransactionId AND T.TransactionStatusId =8    
 INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = TD.EventTicketAttributeId    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId        
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId),0))  SoldTic    
    
FROM EventTicketDetails ETD WITH(NOLOCK)
 INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON ETD.Id=EA.EventTicketDetailId AND EA.Id IN(SELECT VMCC_Id FROM @tblVMCCIds)    
 INNER JOIN CurrencyTypes CM WITH(NOLOCK) ON EA.CurrencyId=CM.Id    
 INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id    
 INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id    
 INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id    
 INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id    
 INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id    
 WHERE EA.Id IN(SELECT VMCC_Id FROM @tblVMCCIds)    
 AND ETD.TicketCategoryId IN(SELECT * FROM @tblEventTicketCategory)                                   
END  