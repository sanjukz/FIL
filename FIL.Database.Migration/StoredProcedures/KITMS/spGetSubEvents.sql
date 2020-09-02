CREATE PROCEDURE [dbo].[spGetSubEvents] --2164,61,1,4565  
(  
 @EventCatId  BIGINT,  
 @VenueId BIGINT,  
 @IsLoginBased INT=0,  
 @UserId INT=0  
)  
AS  
BEGIN  
 DECLARE @tblEvent TABLE(EventId INT)  
 IF(@IsLoginBased=0)  
 BEGIN  
  SELECT DISTINCT ED.Id AS EventCatId,   
  ED.Name + ' (' + Convert(VARCHAR(17),ED.StartDateTime,113) +')' AS EventCatName  
  FROM  EventDetails ED WITH(NOLOCK)   
  WHERE ED.EventId = @EventCatId  AND ED.VenueId=@VenueId AND ED.IsEnabled=1  
 END  
 ELSE  
 BEGIN  
  IF EXISTS (SELECT A.Id FROM UserVenueMappings A WITH(NOLOCK)   
  INNER JOIN EventDetails B WITH(NOLOCK) On A.VenueId = B.VenueId  
  WHERE A.UserId=@UserId AND B.EventId=@EventCatId AND A.VenueId=@VenueId)  
  BEGIN  
   INSERT INTO @tblEvent  
   SELECT DISTINCT Id FROM EventDetails ED WITH(NOLOCK) WHERE EventId=@EventCatId --AND ED.IsEnabled=1   
   AND VenueId=@VenueId  
  END  
  ELSE IF EXISTS (SELECT A.Id FROM EventsUserMappings A WITH(NOLOCK)   
  INNER JOIN EventDetails B WITH(NOLOCK) On A.EventDetailId = B.Id AND B.EventId = @EventCatId  
  WHERE A.UserId=@UserId AND A.EventDetailId=0 AND VenueId=@VenueId)  
  BEGIN  
   INSERT INTO @tblEvent  
   SELECT DISTINCT Id FROM EventDetails ED WITH(NOLOCK) WHERE EventId=@EventCatId AND ED.IsEnabled=1   
   AND VenueId=@VenueId  
  END  
  ELSE  
  BEGIN  
   INSERT INTO @tblEvent  
   SELECT EventDetailId FROM EventsUserMappings A WITH(NOLOCK)    
   INNER JOIN EventDetails ED WITH(NOLOCK) ON A.EventId = ED.EventId  
   WHERE UserId=@UserId AND VenueId=@VenueId --AND ED.IsEnabled =1  
  END  
  
  
  DECLARE @SeasonId BIGINT,@SeasonName NVARCHAR(200)  
  IF(@EventCatId IN(137) AND @UserId NOT IN(1))  
  BEGIN  
   SET @SeasonId= CASE @VenueId WHEN 55 THEN -1  WHEN 56 THEN -2   
   WHEN 57 THEN -3 WHEN 58 THEN -4 WHEN 59 THEN -5  WHEN 60 THEN -6   
   WHEN 62 THEN -7 WHEN 392 THEN -8 END   
   SET @SeasonName= CASE @SeasonId WHEN -1 THEN 'Season Tickets (Trinidad And Tobago)'   
   WHEN -2 THEN 'Season Tickets (Saint Lucia)'   
   WHEN -3 THEN 'Season Tickets (Barbados)'   
   WHEN -4 THEN 'Season Tickets (Guyana)'   
   WHEN -5 THEN 'Season Tickets (Jamaica)'   
   WHEN -6 THEN 'Season Tickets (Saint Kitts And Nevis)'   
   WHEN -7 THEN 'Season Tickets (United States)'   
   WHEN -8 THEN 'Season Tickets (Antigua And Barbuda)'   
   END   
  
   SELECT DISTINCT ED.Id, ED.Name + ' (' + Convert(VARCHAR(17),StartDateTime,113) +')' AS Name  
   FROM Events ECD WITH(NOLOCK)   
   INNER JOIN EventDetails ED WITH(NOLOCK) ON ECD.Id=ED.EventId  
   WHERE ED.Id IN(SELECT EventId FROM @tblEvent) AND ED.IsEnabled=1  
   UNION  
   SELECT @SeasonId,@SeasonName   
      END  
      ELSE  
   BEGIN  
   SELECT DISTINCT ED.Id, ED.Name + ' (' + Convert(VARCHAR(17),StartDateTime,113) +')' AS Name  
   FROM Events ECD  WITH(NOLOCK)   
   INNER JOIN EventDetails ED WITH(NOLOCK) ON ECD.Id=ED.EventId  
   WHERE ED.Id IN(SELECT EventId FROM @tblEvent) --AND ED.IsEnabled=1  
   END  
 END  
END  
  