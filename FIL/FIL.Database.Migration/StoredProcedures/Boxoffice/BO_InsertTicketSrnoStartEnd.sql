CREATE PROCEDURE [dbo].[BO_InsertTicketSrnoStartEnd]                        
(            
	@StartSrNo VARCHAR(500),           
	@EndSrNo VARCHAR(500),          
	@Retailerid BIGINT             
)                        
AS                   
DECLARE @TimeZone VARCHAR (100)                  
DECLARE @venueid INT             
DECLARE @eventType int            
SELECT  TOP 1 @venueid= venueid FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId AND IsEnabled=1 ORDER BY Id desc            
SELECT TOP 1 @eventType= EventId from BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId AND IsEnabled=1 ORDER BY Id desc            
SELECT TOP 1 @TimeZone=ISNULL(EA.TimeZone,0) FROM EventAttributes EA WITH(NOLOCK)   
LEFT OUTER JOIN EventDetails ED  WITH(NOLOCK)  ON EA.EventDetailId=ED.Id    
WHERE VenueId=@VenueId           
AND EventId IN (SELECT DISTINCT EventId FROM BoUserVenues WITH(NOLOCK) WHERE UserId=@RetailerId and IsEnabled=1)      
            
BEGIN              
INSERT INTO BO_StartandEndingSrNo (StartSrNo,EndSrNo,Retailer_id,CreatedDate,VenueId) 
VALUES (@StartSrNo,@EndSrNo,@Retailerid,DATEADD(MINUTE, CONVERT(INT,@TimeZone),GETUTCDATE()),@VenueId);               
SELECT 'Ticket serial numbers inserted successfully – Start No is '+ CONVERT(VARCHAR(20),@StartSrNo)+' – End No is  '+CONVERT(VARCHAR(20),@EndSrNo)+'' AS [Message]                        
END 