CREATE PROC [dbo].[spCheckSeatAvailabilityBeforeBlocking]  
(  
--DECLARE   
 @TicketNumberId TEXT,  
 @retValue INT OUTPUT  
)  
AS  
BEGIN  
   
 DECLARE @MatchSeatTicketDetailCount INT =0, @ActualSeatCount INT =0  
    SELECT @MatchSeatTicketDetailCount = COUNT(*) FROM MatchSeatTicketDetails A WITH(NOLOCK)  
 INNER JOIN MatchLayoutSectionSeats B WITH(NOLOCK) On A.MatchLayoutSectionSeatId = B.Id  
 WHERE A.Id IN (SELECT Id FROM MatchSeatTicketDetails WHERE MatchLayoutSectionSeatId IN(SELECT * FROM dbo.SplitString(@TicketNumberId, ',')))   
 AND B.SeatTypeId = 1 AND SeatStatusId=1 AND A.SponsorId IS NULL AND TransactionId IS NULL  
   
 SELECT @ActualSeatCount =COUNT(*) FROM dbo.SplitString(@TicketNumberId, ',')  
 IF(@ActualSeatCount=@MatchSeatTicketDetailCount)  
 BEGIN  
  SET @retValue=0   
 END  
 ELSE  
 BEGIN  
  SET @retValue=1  
 END  
  
 --SET @retValue=0  
END  