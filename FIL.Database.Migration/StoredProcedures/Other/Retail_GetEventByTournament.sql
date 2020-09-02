CREATE proc [dbo].[Retail_GetEventByTournament]                          
(   
--Declare                                        
	@RetailerId BIGINT=336002,            
	@EventCatid BIGINT=2372                                     
)                                        
                  
AS               
BEGIN               
SET NOCOUNT ON           
Declare @BMID varchar(max)          
Select @BMID=IsNull(UserType,0) from BoxofficeUserAdditionalDetails WITH(NOLOCK) where UserId=@RetailerId          
If(@BMID=2)          
Begin          
SELECT DISTINCT ED.Id as EventId,ED.EventId as EventType, ED.Name as EventName              
FROM BoUserVenues R WITH(NOLOCK)               
inner JOIN EventDetails ED WITH(NOLOCK) ON R.EventId = ED.EventId  and      
R.VenueId = ED.VenueId                        
WHERE                                       
R.IsEnabled=1  and ED.EventId=@EventCatid            
and  ED.VenueId in (select DISTINCT ISNULL(venueid,0) from  BoUserVenues WITH(NOLOCK) 
where UserId in(Select UserId from BoxofficeUserAdditionalDetails WITH(NOLOCK) where ParentId=@RetailerId))           
END          
Else          
Begin          
SELECT DISTINCT ED.Id as EventId,ED.EventId as EventType, ED.Name as EventName         
FROM BoUserVenues R WITH(NOLOCK)               
inner JOIN EventDetails ED WITH(NOLOCK) ON R.EventId = ED.EventId and      
R.VenueId = ED.VenueId                   
WHERE                                       
R.IsEnabled=1 And R.UserId =@RetailerId and ED.EventId=@EventCatid            
and  ED.VenueId in (select ISNULL(venueid,0) from  BoUserVenues WITH(NOLOCK) where UserId=@RetailerId)           
END                                              
                  
END      
