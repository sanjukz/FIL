CREATE PROC CR_GetAutoEmailEventDetailByReportId  
(  
 @ReportId INT  
)  
AS  
BEGIN  
 SELECT DISTINCT ED.Id, ED.Name AS EventName, ED.StartDateTime AS EventStartDateTime, C.Name AS VenueCity   
 FROM EventDetails ED        
 INNER JOIN AutoEmailReportEventDetailMapping AEREDM WITH (NOLOCK) ON AEREDM.EventDetailId = ED.Id                                                   
 INNER JOIN Venues V WITH (NOLOCK) ON V.Id = ED.VenueId                                                      
 INNER JOIN Cities C WITH (NOLOCK) ON C.Id = V.CityId                                      
 INNER JOIN States S WITH (NOLOCK) ON S.Id = C.StateId                                                  
 INNER JOIN Countries COU WITH (NOLOCK) ON COU.Id = S.CountryId         
 WHERE AEREDM.AutoEmailReportScheduleId = @ReportId       
 ORDER BY ED.StartDateTime        
END