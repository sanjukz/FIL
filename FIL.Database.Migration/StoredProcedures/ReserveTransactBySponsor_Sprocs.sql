USE [KzOLTP]
GO
/****** Object:  PROCEDURE [dbo].[KITMS_GetSponsorListByVenue2017]       Script Date: 5/31/2018 4:21:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetSponsorListByVenue2017]                        
(                                
 @VenueId INT,    
 @EventCatId BIGINT    
)                                
AS                                
BEGIN        
                            
SELECT  S.SponsorName,S.Id                                 
FROM Sponsors S                                   
WHERE S.IsEnabled=1 ORDER BY S.SponsorName ASC      
           
SELECT   S.SponsorName,S.Id                                  
FROM Sponsors S                                    
WHERE S.Id  IN ( SELECT DISTINCT SponsorId FROM EventSponsorMappings ESM WHERE ESM.EventDetailId IN    
 (SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId= @EventCatId) AND  ESM.IsEnabled =1)     
AND S.IsEnabled=1 ORDER BY S.SponsorName ASC      
                                 
END     
GO
/****** Object:  [dbo].[KITMS_GetCategoryDetailsForSponsor2017]    Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetCategoryDetailsForSponsor2017]    
(      
 @VenueId BIGINT,    
 @VenueCatId BIGINT,    
 @EventCatId BIGINT,    
 @SponsorId BIGINT                      
)    
AS                  
BEGIN       
    
DECLARE @EventType BIGINT    
DECLARE @Cnt BIGINT          
    
SET @EventType = @EventCatId                     
    
DECLARE @tblVmmc TABLE    
(    
 VmccId BIGINT,    
 EventID BIGINT    
)                                      
    
INSERT INTO @tblVmmc  select ETA.Id,ETD.EventDetailId FROM EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
where ETD.EventDetailId IN(SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND eventId =@EventType) AND ETD.TicketCategoryId=@VenueCatId   
    
SELECT @Cnt= COUNT(*) FROM CorporateTicketAllocationDetails WHERE  SponsorId=@SponsorId   
 AND EventTicketAttributeId IN (SELECT DISTINCT VmccId FROM @tblVmmc)    
    
IF (@Cnt <> 0)    
BEGIN    
PRINT 'IN'    
 SELECT SM.Id, SM.SponsorName, ISNULL(SUM(CTAD.AllocatedTickets),0) AS SponsorBlocked, ISNULL(SUM(CTAD.RemainingTickets),0) AS UnClassifiedBlocked        
 FROM CorporateTicketAllocationDetails CTAD LEFT OUTER JOIN Sponsors SM ON SM.Id= CTAD.SponsorId                        
 WHERE  EventTicketAttributeId IN (SELECT DISTINCT VmccId FROM @tblVmmc)  AND CTAD.SponsorId=@SponsorId AND CTAD.IsEnabled = 1    
 GROUP BY  SM.Id, SM.SponsorName     
END                                    
ELSE    
BEGIN    
PRINT 'Else'    
   SELECT Id, SponsorName,0 AS SponsorBlocked, 0 AS UnClassifiedBlocked    
   FROm Sponsors WHERE Id=@SponsorId    
END    
    
SELECT ISNULL(SUM(AvailableTicketForSale),0) AS AvailableTicket FROM EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
where ETD.EventDetailId IN(SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND eventId =@EventType) AND ETD.TicketCategoryId=@VenueCatId   
    
END 
GO
/****** Object:  [dbo].[KITMS_GetMatchesForSponsor2017]     Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KITMS_GetMatchesForSponsor2017]    
(          
 @SponsorId BIGINT,          
 @VenueCatId BIGINT,    
 @EventCatId BIGINT,    
 @VenueId BIGINT     
)          
AS          
BEGIN          
    
DECLARE  @VmccId TABLE(VmccId BIGINT)     
    
INSERT INTO @VmccId    
  select ETA.Id FROM EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
where ETD.EventDetailId IN(SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND eventId =@EventCatId) AND ETD.TicketCategoryId=@VenueCatId     
    
DECLARE @tblSonsorReserveDetails TABLE(EventId BIGINT,EventName VARCHAR(200), Status INT)    
DECLARE @tblSonsorTransactDetails TABLE(EventId BIGINT, EventName VARCHAR(200), Status INT)    
DECLARE @tblMatches Table (RowId INT IDENTITY(1,1), EventId BIGINT)    
    
INSERT INTO @tblMatches    
SELECT DISTINCT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId=@EventCatId    
    
DECLARE  @MatchCounter INT = 1, @MatchCount INT =0, @EventIdTemp INT =0,@SumAvailableTic INT = 0    
    
SELECT @MatchCount = ISNULL(COUNT(*),0) FROM @tblMatches    
WHILE(@MatchCounter<=@MatchCount)    
BEGIN    
 SELECT @EventIdTemp = EventId FROM @tblMatches WHERE RowId = @MatchCounter    
 SELECT @SumAvailableTic = ISNULL(SUM(RemainingTickets),0) FROM CorporateTicketAllocationDetails WHERE EventTicketAttributeId IN(SELECT VmccId FROM @VmccId)  
  AND SponsorId= @SponsorId    
     
 INSERT INTO @tblSonsorTransactDetails    
 SELECT @EventIdTemp, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets For Transaction <b>' +     
 CONVERT(VARCHAR(200),@SumAvailableTic) +'</b> )' AS EventName,    
 CASE WHEN  CAST(ED.EndDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status          
 FROM EventDetails ED     
 WHERE ED.EventId = @EventIdTemp    
 ORDER BY StartDateTime ASC     
     
 SET @MatchCounter= @MatchCounter + 1    
END    
    
 INSERT INTO @tblSonsorReserveDetails    
 SELECT ED.Id, '<b>'+CONVERT(VARCHAR(MAX),ED.Name) + '</b><br/> ( Available Tickets <b>' +     
 CONVERT(VARCHAR(200),ISNULL(SUM(ETA.RemainingTicketForSale),0)) +'</b> )' AS EventName,    
 CASE WHEN  CAST(ED.EndDateTime AS DATETIME) >= GETDATE() THEN 1 ELSE 1 END AS Status          
FROM EventDetails ED      
 INNER JOIN EventTicketDetails ETD ON ED.Id = ETD.EventDetailId     
 INNER JOIN EventTicketAttributes ETA ON ETA.EventTicketDetailId=ETD.Id     
 WHERE ETD.TicketCategoryId = @VenueCatId      
 AND ED.Id IN(SELECT EventId FROM @tblMatches)      
 GROUP BY ED.Id,ED.name,ETA.RemainingTicketForSale, ED.EndDateTime,ED.StartDateTime      
 ORDER BY ED.StartDateTime  ASC    
     
 SELECT DISTINCT * FROM @tblSonsorReserveDetails    
 SELECT DISTINCT * FROM @tblSonsorTransactDetails    
    
END 
GO
/****** Object:  [dbo].[KITMS_GetExistingCategorySponsors]     Script Date: 5/31/2018 4:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_GetExistingCategorySponsors]    
(    
    
 @EventId BIGINT,    
 @VenueCatId BIGINT    
)    
AS    
BEGIN    
    
SELECT MS.Id, CONVERT(VARCHAR(200),MS.SponsorName) +' (Avaiable Tic ' + CONVERT(VARCHAR(200), SUM(RemainingTickets)) +')' AS SponsorName                                   
FROM Sponsors MS                                                        
INNER JOIN CorporateTicketAllocationDetails S ON MS.Id = S.Sponsorid                                           
Where MS.IsEnabled=1   
 AND S.EventTicketAttributeId IN (SELECT ETA.Id  from EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
where ETD.EventDetailId IN(SELECT Id FROM EventDetails WHERE Id =@EventId) AND ETD.TicketCategoryId=@VenueCatId)    
GROUP BY MS.Id,MS.SponsorName    
    
END 
GO
/****** Object: [dbo].[KITMS_TransferSeatsByMatch]     Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_TransferSeatsByMatch]                                           
(                                              
 @SponsorId BIGINT,      
 @SponsorToId BIGINT,                                            
 @EventId BIGINT,      
 @Seats VARCHAR(MAX),                                                  
 @VenueCatId BIGINT,                                               
 @TicketQty INT,                                              
 @UpdateBY VARCHAR(500)                                               
)                                              
AS                                              
BEGIN      
      
BEGIN TRANSACTION trans      
BEGIN TRY      
                                    
DECLARE @BlockTic BIGINT=0                                  
DECLARE @AvailableTic BIGINT=0                                  
DECLARE @KM_ID BIGINT=0                        
DECLARE @EventName VARCHAR(255)                                
DECLARE @TicketForSale BIGINT                                  
DECLARE @ReturnVal VARCHAR(5000) =''                                   
DECLARE @Flag VARCHAR(50) = 'Default'      
DECLARE @Price DECIMAL      
     
         
DECLARE @KM_IDNew  BIGINT      
DECLARE @VmccId_New BIGINT            
DECLARE @BlockTic_New BIGINT             
DECLARE @AvailableTic_New BIGINT           
         
SET @Flag='Default'     
                     
DECLARE @VmccId BIGINT     
    
SELECT @VmccId = ETA.Id FROM EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId WHERE TicketCategoryId = @VenueCatId                    
AND ETD.EventDetailId = @EventId       
     
    
SET @KM_ID = (SELECT ISNULL(Id,0) FROM CorporateTicketAllocationDetails WHERE SponsorId = @SponsorId          
and EventTicketAttributeId = @VmccId)     
SET @Price=   (SELECT Price FROM CorporateTicketAllocationDetails WHERE SponsorId = @SponsorId          
and EventTicketAttributeId = @VmccId)        
          
SELECT @BlockTic = AllocatedTickets,@AvailableTic=RemainingTickets FROM  CorporateTicketAllocationDetails WHERE EventTicketAttributeId = @KM_ID               
 IF(@KM_ID > 0)     
 BEGIN    
 IF(@TicketQty < @AvailableTic+1)              
 BEGIN              
            
  SET @BlockTic = @BlockTic-@TicketQty              
  SET @AvailableTic = @AvailableTic-@TicketQty            
          
  UPDATE CorporateTicketAllocationDetails set AllocatedTickets = @BlockTic, RemainingTickets = @AvailableTic WHERE Id = @KM_ID            
              
  SELECT @KM_IDNew=Id,@BlockTic_New = AllocatedTickets,@AvailableTic_New=RemainingTickets FROM  CorporateTicketAllocationDetails     
  WHERE SponsorId = @SponsorToId AND EventTicketAttributeId = @VmccId            
               
  SET @BlockTic_New = @BlockTic_New+@TicketQty            
  SET @AvailableTic_New = @AvailableTic_New+@TicketQty            
              
  IF(@KM_IDNew IS NOT NULL)            
  BEGIN            
  UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = @BlockTic_New, RemainingTickets = @AvailableTic_New WHERE Id = @KM_IDNew             
  END            
  ELSE            
  BEGIN            
    INSERT INTO CorporateTicketAllocationDetails VALUES(newId(),@VmccId,@SponsorToId,@TicketQty,@TicketQty,@Price,Null,null,1,GETDATE(),NewID(),getdate(),@UpdateBY)           
  END            
      
  update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS   
  INNER JOIN MatchLayoutSections MLS ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN  
   MatchLayouts ML ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId =@EventId)  
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    update MatchSeatTicketDetails set SponsorId=@SponsorId where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD Inner Join MatchLayoutSectionSeats MLSS On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId =@EventId and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
       
  INSERT INTO CorporateTicketAllocationDetailLogs VALUES(@KM_ID,NULL,3,@TicketQty,@Price,1,GetDate(),NEWID(),GETDATE(),@UpdateBY)      
     
     SET @Flag='Changed'      
END           
END    
IF(@Flag='Changed')                  
BEGIN         
 DECLARE @SponsorName VARCHAR(500), @SponsorToName VARCHAR(500)    
     SELECT @SponsorName = SponsorName FROM Sponsors WHERE Id = @SponsorId    
     SELECT @SponsorToName = SponsorName FROM Sponsors WHERE Id = @SponsorToId     
         
     SELECT @EventName = ED.name FROM EventDetails ED JOIN EventTicketDetails VMCC ON     
 VMCC.EventDetailId = ED.EventId where VMCC.EventDetailId= @EventId      
         
 SET @ReturnVal += CONVERT(VARCHAR(100),@TicketQty) + ' setas transfered successfully for <b>' +  @EventName+ '</b> from '+@SponsorName+ ' to '+ @SponsorToName                  
END       
ELSE                  
BEGIN           
 SELECT @EventName = ED.name FROM EventDetails ED JOIN EventTicketDetails VMCC ON     
 VMCC.EventDetailId = ED.EventId where VMCC.EventDetailId= @EventId                    
 SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@AvailableTic) + ' seats are avaialble for transfer for <b>'+ @EventName + '</b>'                  
END                  
         
SELECT @ReturnVal                           
COMMIT TRANSACTION trans      
END TRY      
BEGIN CATCH      
ROLLBACK TRANSACTION trans       
END CATCH                               
END 
 GO
/****** Object: [dbo].[dbo].[KITMS_BlockSeatsByMatch]     Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KITMS_BlockSeatsByMatch]                               
(                                          
  @SponsorId BIGINT,                                          
  @EventIds VARCHAR(MAX),  
  @Seats VARCHAR(MAX),        
  @VenueCatId BIGINT,                                           
  @tics INT,                                          
  @UpdateBY VARCHAR(500)                                           
)                                          
AS                                          
BEGIN    
    
BEGIN TRANSACTION trans    
BEGIN TRY    
                                        
DECLARE @BlockTic BIGINT, @AvailableTic BIGINT, @KM_ID BIGINT, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',    
@Flag VARCHAR(50) = 'Default',@EventId BIGINT, @VmccId BIGINT,@AltId VARCHAR(500), @KITMSMasterId BIGINT, @Price DECIMAL(18,2)    
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY    
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                    
INSERT INTO #vmccIds    
  
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A    
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))                        
  
DECLARE @SponsorType INT = 1  
SELECT @SponsorType = SponsorTypeId FROm Sponsors WHERE Id= @SponsorId  
  
DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds    
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN    
   
 IF NOT EXISTS(select MSTD.Id from  MatchSeatTicketDetails MSTD Inner Join MatchLayoutSectionSeats MLSS On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
    BEGIN              
  SELECT @TicketForSale =RemainingTicketForSale, @Price= LocalPrice FROM EventTicketAttributes WHERE Id=@VmccId                    
  
   IF(@TicketForSale-@tics>=0)                    
  BEGIN                  
  
   IF EXISTS(SELECT Id FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
   AND SponsorId=@SponsorId AND IsEnabled = 1)    
   BEGIN    
    SELECT @KITMSMasterId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
    AND SponsorId=@SponsorId AND IsEnabled = 1    
     
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets+@tics, RemainingTickets = RemainingTickets+@tics,    
    UpdatedBy = @AltId, UpdatedUtc = GETDATE() WHERE Id = @KITMSMasterId     
  
                               
  END  
  ELSE    
   BEGIN    
    INSERT INTO CorporateTicketAllocationDetails(AltId,EventTicketAttributeId,SponsorId, AllocatedTickets, RemainingTickets, Price,     
    IsEnabled, CreatedUtc, CreatedBy)                          
    VALUES(NEWID(),@VMCCId, @SponsorId,@tics,@tics,@Price,1,GETDATE(),@AltId)    
    SET @KITMSMasterId = SCOPE_IDENTITY()    
   END        
   UPDATE EventTicketAttributes SET RemainingTicketForSale = RemainingTicketForSale - @tics WHERE Id=@VMCCId    
   INSERT INTO CorporateTicketAllocationDetailLogs(CorporateTicketAllocationDetailId, AllocationOptionId, TotalTickets, Price, IsEnabled, CreatedUtc, CreatedBy)    
   VALUES(@KITMSMasterId,1,@tics,@Price,1,GETDATE(),@AltId);                    
    
  SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId                   
  IF(@SponsorType=0)  
  BEGIN  
   update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS   
  INNER JOIN MatchLayoutSections MLS ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN  
   MatchLayouts ML ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )  
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    
  update MatchSeatTicketDetails set SponsorId=@SponsorId where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD Inner Join MatchLayoutSectionSeats MLSS On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
  END  
  ELSE  
  BEGIN  
   update MatchLayoutSectionSeats set SeatTypeId=3 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS   
  INNER JOIN MatchLayoutSections MLS ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN  
   MatchLayouts ML ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )  
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    update MatchSeatTicketDetails set SponsorId=@SponsorId where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD Inner Join MatchLayoutSectionSeats MLSS On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))  
  END  
   SET @Flag='Changed'    
   IF(@Flag='Changed')                                
   BEGIN                           
    SET @ReturnVal += CONVERT(VARCHAR(100),@tics) + ' Tickets Blocked Successfully for ' + @EventName+ ',<br/> '       
   END   
  END                      
  
  ELSE                              
  BEGIN                    
   SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId   
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TicketForSale) + ' seats are availble for <b>'+ @EventName + '</b>, '  
    END                              
 END  
 ELSE                              
  BEGIN                    
   SELECT @EventName = ED.Name FROM EventDetails ED  WITH(NOLOCK) WHERE Id= @EventId   
   SET @ReturnVal += 'Sorry, seats are no longer availble for <b>'+ @EventName + '</b>, Please select again.'  
    END     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                  
END                   
                        
CLOSE curEvent;                   
DEALLOCATE curEvent;                  
  
SELECT @ReturnVal                       
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                                  
END
 GO
/****** Object: [dbo].[dbo].[KITMS_GetSeatInfo]    Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE PROCEDURE [dbo].[KITMS_GetSeatInfo]  
(                                                              
  @EventIds VARCHAR(MAX)='',                                          
  @VenueCatId BIGINT ,    
  @SponsorId INT                                         
)                                          
AS                                          
BEGIN  
   
DECLARE  @tblEvent  TABLE                
(      
 EventId BIGINT,     
 VmccId BIGINT  
)                  
  
INSERT INTO @tblEvent  
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A    
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))     
  
DECLARE @EventId BIGINT                    
DECLARE @VmccId BIGINT  
DECLARE @BlockTickets INT  
DECLARE @AvailableTickets BIGINT  
DECLARE @KMId BIGINT, @EventName VARCHAR(500) , @AvailableTic INT                  
  
DECLARE @tblSeats TABLE  
(  
  EventName VARCHAR(500),  
  AvailableSeats INT,  
  BlockSeats INT  
)  
  
DECLARE curEvent CURSOR FOR                                     
SELECT * FROM @tblEvent  
                 
OPEN  curEvent;                   
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                  
 WHILE @@FETCH_STATUS=0                  
 BEGIN                  
   SELECT @KMId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
    AND SponsorId=@SponsorId AND IsEnabled = 1                 
  SELECT @EventName = ED.Name FROM EventDetails ED WHERE ED.Id= @EventId   
  SELECT @AvailableTic=ISNULL(RemainingTickets,0) FROM  CorporateTicketAllocationDetails where Id= @KMId   
     
  SELECT @BlockTickets = COUNT(*) FROM CorporateTicketAllocationDetails  
  WHERE SponsorId = @SponsorId  and EventTicketAttributeId = @VmccId  
    
  INSERT INTO @tblSeats  
  SELECT @EventName, @AvailableTic, @BlockTickets  
    
FETCH NEXT FROM curEvent INTO @VmccId, @EventId   
END  
CLOSE curEvent;                   
DEALLOCATE curEvent;   
  
SELECT * FROM @tblSeats  
END  
 GO
/****** Object: [dbo].[dbo].[KITMS_GetStandDetailsBySponsor2017]     Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
SELECT Id FROM EventDetails WHERE VenueId = @VenueId AND EventId=@EventType      
    
INSERT INTO @tblEventTicketCategory    
  SELECT DISTINCT TicketCategoryId FROm EventTicketDetails WHERE EventDetailId IN    
  (SELECT Id FROm EventDetails WHERE VenueId = @VenueId AND EventId = @EventCatId)    
   INSERT INTO @tblVMCCIds    
  SELECT Id FROm EventTicketAttributes WHERE EventTicketDetailId IN     
  (SELECT Id FROM EventTicketDetails WHERE EventDetailId IN(SELECT EventId FROM @tblMatches)    
  AND TicketCategoryId IN(SELECT Id from @tblEventTicketCategory))   
    
SELECT  DISTINCT TC.ID AS VenueCatId,-- VMCC.Status,    
 TC.Name  AS Category,    
 (SELECT ISNULL(COUNT(*),0) FROM @tblMatches) AS TotalMatches,    
 (ISNULL((SELECT SUM(KM.AllocatedTickets)                                                                                 
 FROM  CorporateTicketAllocationDetails KM WITH(NOLOCK) INNER JOIN EventTicketAttributes SM WITH(NOLOCK) on SM.Id = KM.EventTicketAttributeId    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = SM.EventTicketDetailId                                      
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND KM.IsEnabled = 1),0)) AS SponsoredTickets,   
      
 (SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalTickets,    
    
 (SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) TotalAvailTickets,     
    
 (SELECT ISNULL(SUM(RemainingTicketForSale),0) FROM EventTicketAttributes ETA    
 INNER JOIN EventTicketDetails ETDTemp WITH(NOLOCK) on ETDTemp.Id = ETA.EventTicketDetailId     
 WHERE ETDTemp.TicketCategoryId = ETD.TicketCategoryId AND ETDTemp.EventDetailId IN (SELECT * FROM @tblMatches)) RemainingTics,    
     
    
 (SELECT ISNULL(SUM(AvailableTicketForSale),0) FROM EventTicketAttributes ETA    
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
    
FROM EventTicketDetails ETD    
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
 GO
/****** Object: [dbo].[dbo].[KITMS_TransferCategoryTicketsToSponsor]     Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
CREATE PROCEDURE [dbo].[KITMS_TransferCategoryTicketsToSponsor]                   
(                                        
  @EventId BIGINT,   
  @SponsorToId BIGINT,                     
  @VenueCatId BIGINT,                         
  @TicketQty INT,                        
  @UpdateBY VARCHAR(500),   
  @SponsorFromId BIGINT                    
)                        
AS                        
BEGIN     
  
BEGIN TRANSACTION trans    
BEGIN TRY               
DECLARE @BlockTic BIGINT           
DECLARE @AvailableTic BIGINT           
DECLARE @TicketForSale BIGINT  
DECLARE @Price DECIMAL            
               
      
DECLARE @BlockTic_New BIGINT = 0        
DECLARE @AvailableTic_New BIGINT = 0          
DECLARE @ReturnVal VARCHAR(5000)              
DECLARE @Flag VARCHAR(50) = 'Default'            
            
DECLARE @TempTable TABLE  
(  
 BlockTic INT, AvailableTic INT, Status INT  
)   
             
DECLARE @KMTransferFromId BIGINT =0  
DECLARE @KMTransferToId BIGINT =0  
DECLARE @VmccId BIGINT =0  
  
SELECT @VmccId = A.Id  FROm EventTicketAttributes A    
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId    
WHERE EventDetailId =@EventId  
SET @Price=   (SELECT Price FROM CorporateTicketAllocationDetails WHERE SponsorId = @SponsorFromId            
and EventTicketAttributeId = @VmccId)  
    
SELECT @KMTransferFromId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
   AND SponsorId=@SponsorFromId AND IsEnabled = 1  
  
  
SELECT @KMTransferToId = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId    
   AND SponsorId=@SponsorToId AND IsEnabled = 1  
  
IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WHERE  Id = @KMTransferFromId)    
BEGIN          
         
SELECT @BlockTic = AllocatedTickets,@AvailableTic=RemainingTickets FROM  CorporateTicketAllocationDetails WHERE Id = @KMTransferFromId             
        
 IF(@TicketQty < @AvailableTic+1)            
 BEGIN            
          
   SET @BlockTic = @BlockTic-@TicketQty            
   SET @AvailableTic = @AvailableTic-@TicketQty          
            
   UPDATE CorporateTicketAllocationDetails set AllocatedTickets = @BlockTic, RemainingTickets = @AvailableTic WHERE Id = @KMTransferFromId          
             
   SELECT @BlockTic_New = ISNULL(AllocatedTickets,0),@AvailableTic_New=ISNULL(RemainingTickets,0) FROM  CorporateTicketAllocationDetails WHERE Id = @KMTransferToId      
     
   SET @BlockTic_New = @BlockTic_New+@TicketQty          
   SET @AvailableTic_New = @AvailableTic_New+@TicketQty     
             
   IF EXISTS(SELECT * FROM CorporateTicketAllocationDetails WHERE  Id = @KMTransferToId)     
   BEGIN       
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = @BlockTic_New, RemainingTickets = @AvailableTic_New WHERE Id = @KMTransferToId           
   END          
   ELSE          
   BEGIN  
   INSERT INTO CorporateTicketAllocationDetails VALUES(newId(),@VmccId,@SponsorToId,@TicketQty,@TicketQty,@Price,Null,null,1,GETDATE(),NewID(),getdate(),@UpdateBY)           
   END   
     
   IF NOT EXISTS(SELECT * FROM CorporateTicketAllocationDetails WHERE  Sponsorid=@SponsorToId AND  EventTicketAttributeId=@VmccId)     
   BEGIN       
   INSERT INTO EventSponsorMappings VALUES(@SponsorToId, @EventId,1,NEWID(), GETDATE(),null,@UpdateBY)          
   END            
    INSERT INTO CorporateTicketAllocationDetailLogs VALUES(@KMTransferFromId,NULL,3,@TicketQty,@Price,1,GetDate(),NEWID(),GETDATE(),@UpdateBY)            
  
   INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 1)  
   SELECT * FROM @TempTable  
             
END     
ELSE                
BEGIN                
   INSERT INTO @TempTable VALUES(@TicketQty, @AvailableTic, 0)  
   SELECT * FROM @TempTable    
END           
END   
COMMIT TRANSACTION trans    
END TRY    
BEGIN CATCH    
ROLLBACK TRANSACTION trans     
END CATCH                  
END
 GO
/****** Object: [dbo].[dbo].[KITMS_GetTicketAvailability]      Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
CREATE PROCEDURE [dbo].[KITMS_GetTicketAvailability]                                      
(                                                               
  @EventIds VARCHAR(MAX),                                            
  @VenueCatId BIGINT,                                             
  @TicketQty INT                                             
)                                            
AS                                            
BEGIN    
     
DECLARE  @tblEvent  TABLE                  
(        
 EventId BIGINT,       
 VmccId BIGINT    
)                    
    
INSERT INTO @tblEvent   
 SELECT ETA.Id , ETD.EventDetailId from EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
where ETD.EventDetailId IN(SELECT *  FROM SplitString(@EventIds,',')) AND ETD.TicketCategoryId=@VenueCatId       
    
DECLARE @ReturnVal VARCHAR(MAX) =''     
DECLARE @EventId BIGINT                      
DECLARE @VmccId BIGINT    
DECLARE @TotalTic BIGINT, @EventName VARCHAR(500)                      
    
DECLARE curEvent CURSOR FOR                                       
SELECT * FROM @tblEvent    
                   
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN                    
  SELECT @TotalTic =RemainingTicketForSale FROM EventTicketAttributes WHERE Id=@VmccId                      
  SELECT @EventName = ED.Name FROM EventDetails ED WHERE ED.Id= @EventId     
      
  IF(@TotalTic-@TicketQty < 0)                    
  BEGIN     
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@TotalTic) + ' Tickets are Availble for '+ @EventName + ',<br/> '    
  END    
      
FETCH NEXT FROM curEvent INTO @VmccId, @EventId     
END    
CLOSE curEvent;                     
DEALLOCATE curEvent;     
    
SELECT @ReturnVal AS Status    
    
END    
 GO
/****** Object: [dbo].[dbo].[KITMS_CheckBlockSeatAvailability]      Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
CREATE PROCEDURE [dbo].[KITMS_CheckBlockSeatAvailability]    
(                                                                
  @EventIds VARCHAR(MAX)='',                                            
  @VenueCatId BIGINT ,      
  @SponsorId INT,                                           
  @TicketQty INT                                             
)                                            
AS                                            
BEGIN    
     
DECLARE  @tblEvent  TABLE                  
(        
 EventId BIGINT,       
 VmccId BIGINT    
)                    
    
INSERT INTO @tblEvent    
SELECT ETA.Id , ETD.EventDetailId from EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId    
where ETD.EventDetailId IN(SELECT *  FROM SplitString(@EventIds,',')) AND ETD.TicketCategoryId=@VenueCatId   
    
DECLARE @ReturnVal VARCHAR(MAX) =''     
DECLARE @EventId BIGINT                      
DECLARE @VmccId BIGINT    
DECLARE @KMId BIGINT, @EventName VARCHAR(500) , @AvailableTic INT                    
    
DECLARE curEvent CURSOR FOR                                       
SELECT * FROM @tblEvent    
                   
OPEN  curEvent;                     
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                    
 WHILE @@FETCH_STATUS=0                    
 BEGIN                    
  SELECT @KMId =ISNULL(Id,0) FROM CorporateTicketAllocationDetails WHERE SponsorId = @SponsorId          
  AND  EventTicketAttributeId = @VmccId                  
  SELECT @EventName = ED.Name FROM EventDetails ED WHERE ED.Id= @EventId     
  SELECT @AvailableTic=ISNULL(RemainingTickets,0) FROM  CorporateTicketAllocationDetails where Id= @KMId     
      
  IF(@TicketQty > @AvailableTic)                    
  BEGIN     
   SET @ReturnVal += 'Only '+ CONVERT(VARCHAR(100),@AvailableTic) + ' Tickets are Availble for '+ @EventName + ',<br/> '    
  END    
      
FETCH NEXT FROM curEvent INTO @VmccId, @EventId     
END    
CLOSE curEvent;                     
DEALLOCATE curEvent;     
    
SELECT @ReturnVal AS Status    
    
END    
   GO
/****** Object: [dbo].[dbo].[KITMS_GetEventsVmccId]      Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 
CREATE PROCEDURE [dbo].[KITMS_GetEventsVmccId]                                       
(                                                               
  @EventIds VARCHAR(MAX),                                            
  @VenueCatId BIGINT                                           
)                                            
AS                                            
BEGIN    
    
 SELECT  DISTINCT ETA.Id AS VmccId, ED.Id, ED.Name from EventTicketAttributes ETA INNER JOIN EventTicketDetails ETD ON ETD.Id=ETA.EventTicketDetailId  
 INNER JOIN EventDetails ED ON ETD.EventDetailId = ED.Id    
where ETD.EventDetailId IN(SELECT *  FROM SplitString(@EventIds,',')) AND ETD.TicketCategoryId=@VenueCatId   
    
END  
   GO
/****** Object: [dbo].[dbo].[KITMS_ReleaseSeatsByMatch]     Script Date: 8/8/2018 5:21:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO  
   CREATE PROCEDURE [dbo].[KITMS_ReleaseSeatsByMatch]                                           
(                                              
 @SponsorId BIGINT,                                              
 @EventIds VARCHAR(MAX),      
 @Seats VARCHAR(MAX),                                                  
 @VenueCatId BIGINT,                                               
 @tics INT,                                              
 @UpdateBY VARCHAR(500)                                               
)                                              
AS                                              
BEGIN      
      
BEGIN TRANSACTION trans      
BEGIN TRY      
                         
                    
                      
declare @BlockTic BIGINT=0, @AvailableTic BIGINT=0, @KM_ID BIGINT=0, @EventName VARCHAR(255), @TicketForSale BIGINT, @ReturnVal VARCHAR(5000) ='',      
@Flag VARCHAR(50) = 'Default',@AltId VARCHAR(500),@EventId BIGINT, @VmccId BIGINT, @Price DECIMAL(18,2)      
SELECT  @AltId = AltId FROM USers WITH(NOLOCK) WHERE UserName =@UpdateBY      
CREATE TABLE #vmccIds(VmccId BIGINT,EventId BIGINT)                      
INSERT INTO #vmccIds      
SELECT A.Id, B.EventDetailId FROm EventTicketAttributes A      
INNER JOIN EventTicketDetails B ON A.EventTicketDetailId =B.Id AND B.TicketCategoryId = @VenueCatId      
WHERE EventDetailId IN(SELECT * FROM SplitString(@EventIds,','))                     
    
DECLARE curEvent CURSOR FOR SELECT * FROM #vmccIds      
OPEN  curEvent;                       
FETCH NEXT FROM curEvent INTO @VmccId, @EventId                       
while @@FETCH_STATUS=0                    
BEGIN           
 SET @Flag='Default'     
     
  SELECT @KM_ID = ISNULL(Id,0) FROM CorporateTicketAllocationDetails WITH(NOLOCK) WHERE EventTicketAttributeId = @VmccId AND SponsorId=@SponsorId AND IsEnabled = 1         
  SELECT @BlockTic = ISNULL(AllocatedTickets,0),@AvailableTic=ISNULL(RemainingTickets,0),@Price =Price From  CorporateTicketAllocationDetails where Id= @KM_ID          
    
 IF(@tics < @AvailableTic+1)          
 BEGIN          
   UPDATE CorporateTicketAllocationDetails SET AllocatedTickets = AllocatedTickets - @tics,       
  RemainingTickets = RemainingTickets -@tics  WHERE Id = @KM_ID     
           
        
  update MatchLayoutSectionSeats set SeatTypeId=1 where   MatchLayoutSectionId in (SElect MLSS.MatchLayoutSectionId from MatchLayoutSectionSeats MLSS   
  INNER JOIN MatchLayoutSections MLS ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN  
   MatchLayouts ML ON ML.Id=MLs.MatchLayoutId where ML.EventDetailId in(SELECT * FROM dbo.SplitString(@EventIds,',')) )  
   and Id in  (SELECT * FROM dbo.SplitString(@Seats,','))  
    update MatchSeatTicketDetails set SponsorId=null where id in (select MSTD.Id from  MatchSeatTicketDetails MSTD Inner Join MatchLayoutSectionSeats MLSS On MSTD.MatchLayoutSectionSeatId=MLSS.Id INNER JOIN MatchLayoutSections MLS  
  ON MLSS.MatchLayoutSectionId=MLS.id INNER JOIN MatchLayouts ML ON ML.Id=MLs.MatchLayoutId INNER JOIN EventTicketDetails ETD ON  ETD.ID=MSTD.EventTicketDetailId   
   where ML.EventDetailId in (SELECT * FROM dbo.SplitString(@EventIds,',')) and ETD.EventDetailId=3443 and ETD.TicketCategoryId=@VenueCatId and MSTD.SeatStatusId=1  
   and MSTD.MatchLayoutSectionSeatId in (SELECT * FROM dbo.SplitString(@Seats,',')))   
      
  SET @Flag='Changed'      
 END          
  SELECT @EventName = ED.Name from EventDetails ED WHERE Id= @EventId      
 IF(@Flag='Changed')                    
 BEGIN                    
  SET @ReturnVal += CONVERT(Varchar(100),@tics) + ' tickets released successfully for ' +  @EventName+ ',<br/> '                     
 END         
 ELSE                    
 BEGIN                   
  SET @ReturnVal += 'Only '+ CONVERT(Varchar(100),@AvailableTic) + ' tickets are avaialble to release for '+ @EventName + ',<br/> '                    
 END                    
      
 FETCH NEXT FROM curEvent INTO @VmccId, @EventId         
END                         
      
CLOSE curEvent;               
DEALLOCATE curEvent;            
      
DROP TABLE #vmccIds                   
SELECT @ReturnVal                             
COMMIT TRANSACTION trans        
END TRY        
BEGIN CATCH        
ROLLBACK TRANSACTION trans         
END CATCH                                 
END 
 GO
