CREATE PROC [dbo].[Scan_GetVenue]                            
AS                            
BEGIN                            
 SELECT MVL.Id AS VenueId,CM.Name AS Venue FROM  Venues VD WITH(NOLOCK)                             
 INNER JOIN  Cities  CM WITH(NOLOCK) on VD.CityId=CM.Id                            
 INNER JOIN MastervenueLayouts MVL WITH(NOLOCK) on VD.Id=MVL.VenueId                            
 where VD.Id IN(SELECT DISTINCT VenueId FROM EventDetails WITH(NOLOCK) WHERE EventId IN(137))                            
 AND MVL.IsEnabled=1 AND VD.IsEnabled=1                       
End 