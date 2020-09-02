CREATE PROC [dbo].[BO_GetTicketSrNoReport]  --4522                   
(      
  @Retailer_id BIGINT       
)                           
        
AS       
BEGIN      
SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY SESN.Ticket_SrNoID DESC) AS [Sr. No.], U.FirstName+' '+U.LastName AS [BO User Name], 
U.UserName AS [Box Office ID], SESN.StartSrNo AS [Ticket Stock Start], SESN.EndSrNo AS [Ticket Stock End], 
CONVERT(VARCHAR(500),DATEADD(MI,CONVERT(INT,EA.TimeZone),SESN.CreatedDate),110) AS [Updated Date/ Time]      
--SESN.CreatedDate AS [DateTime]    CONVERT(VARCHAR(11),ED.EventStartDate,113)                 
FROM BO_StartandEndingSrNo SESN WITH(NOLOCk) 
INNER  JOIN BoxofficeUserAdditionalDetails RUM WITH(NOLOCk) on SESN.Retailer_id=RUM.UserId 
INNER JOIN Users U WITH(NOLOCk) ON U.Id=RUM.UserId        
INNER  JOIN EventDetails ED WITH(NOLOCk) on ED.Venueid=SESN.venueid and ED.EventId  in (select distinct EventId from BoUserVenues WITH(NOLOCK) where UserId=@Retailer_id and IsEnabled=1)
INNER JOIN  EventAttributes EA WITH(NOLOCk) On EA.EventDetailId=ED.Id                      
WHERE RUM.ParentId=@Retailer_id --and  SESN.CreatedDate > '2017-03-01 00:00:00.000'      
GROUP BY SESN.Ticket_SrNoID,U.FirstName,U.LastName,U.UserName,StartSrNo,EndSrNo,SESN.CreatedDate,EA.TimeZone       
UNION      
SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY SESN.Ticket_SrNoID DESC) AS [Sr. No.], U.FirstName+' '+U.LastName AS [BO User Name],
U.UserName AS [Box Office ID], SESN.StartSrNo AS [Ticket Stock Start], SESN.EndSrNo AS [Ticket Stock End], 
CONVERT(VARCHAR(500),DATEADD(MI,CONVERT(INT,EA.TimeZone),SESN.CreatedDate),110) AS [Date/Time]                    
--SESN.CreatedDate AS [DateTime]      
FROM BO_StartandEndingSrNo SESN WITH(NOLOCk)       
INNER  JOIN BoxofficeUserAdditionalDetails RUM WITH(NOLOCk) on SESN.Retailer_id=RUM.UserId  
INNER JOIN Users U WITH(NOLOCk) ON U.Id=RUM.UserId    
INNER  JOIN EventDetails ED WITH(NOLOCk) on ED.Venueid=SESN.venueid and ED.EventId  in (select distinct EventId   from BoUserVenues WITH(NOLOCK) where UserId=@Retailer_id and IsEnabled=1) 
INNER JOIN  EventAttributes EA WITH(NOLOCk) On EA.EventDetailId=ED.Id                              
WHERE RUM.UserId=@Retailer_id --and  SESN.CreatedDate > '2017-03-01 00:00:00.000'      
GROUP BY SESN.Ticket_SrNoID,U.FirstName,U.LastName,U.UserName,StartSrNo,EndSrNo,SESN.CreatedDate,EA.TimeZone       
        
End