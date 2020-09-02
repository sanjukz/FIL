CREATE PROC CR_GetAutoEmailReportSchedule      
 AS      
 BEGIN      
 SELECT * FROM AutoEmailReportSchedule WHERE IsEnabled = 1 AND StartDate <= GETDATE() AND EndDate >=GETDATE()    
 END    