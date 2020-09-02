CREATE PROC [dbo].[BO_GetVenueLayout] --80120              
(              
	@EventId BIGINT              
)              
AS
BEGIN       
--IF EXISTS(SELECT * FROM ITKTS_Match WHERE Event_ID = @EventId)       
--BEGIN    
--SELECT ED.EventType, ED.EventName, ED.EventStartDate,  Match.MatchStartTime AS EventStartTime, ED.EventEndDate, ED.EventEndTime,           
--VD.VenueAddress, CityM.City_Name, ECD.EventCatId, ECD.EventCatName, ECD.P_id, ECD.HTPhoto AS PhotoPath, Match.Match_id, ECD.InnerPageImage,          
--CASE ED.EventId WHEN 117849 THEN '<span style="font-weight: bold; margin-right: 25px; font-size:13px;"> Only south stand </span>'    
--ELSE '' END AS PackageDesc, ECD.P_id          
--FROM ITKTSEVNT_EventDetails ED          
--INNER JOIN ITKTSEvnt_VenueDetails VD ON Vd.VenueId = ED.EventVenue          
--INNER JOIN ITKTS_City_Mst CityM ON CityM.City_Id = VD.VenueCity          
--INNER JOIN ITKTSEVNT_EventCategoryDetails ECD ON ED.EventType = ECD.EventCatId           
--INNER JOIN ITKTS_Match Match on Match.Event_ID = ED.EventId          
--WHERE ED.EventId=@EventId           
--END    
--ELSE    
BEGIN    
	SELECT ED.EventId as EventType, ED.Name as EventName, ED.StartDateTime as EventStartDate, 
	ED.StartDateTime as EventStartTime, ED.EndDateTime as EventEndDate, ED.EndDateTime as EventEndTime, VD.Name as VenueAddress, CityM.Name as City_Name, ECD.Id as EventCatId, ECD.Name as EventCatName, 1 as P_id, '' AS PhotoPath    
	FROM EventDetails ED WITH(NOLOCK)    
	INNER JOIN Venues VD WITH(NOLOCK) ON VD.Id = ED.VenueId    
	INNER JOIN Cities CityM WITH(NOLOCK) ON CityM.Id = VD.CityId    
	INNER JOIN Events ECD WITH(NOLOCK) ON ED.EventId = ECD.Id     
	WHERE ED.Id=@EventId     
END    
    
SELECT DISTINCT VMCC.LocalPrice as PricePerTic, CT.Code as CurrencyName, CT.Id as Currencyid          
FROM EventDetails ED WITH(NOLOCK)   
Inner Join EventTicketDetails ETD WITH(NOLOCK) ON ED.Id= ETD.EventDetailId          
INNER JOIN EventTicketAttributes VMCC WITH(NOLOCK) ON ETD.ID = VMCC.EventTicketDetailId           
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = VMCC.LocalCurrencyId 
WHERE ED.Id=@EventId AND ED.Isenabled=1 AND VMCC.Isenabled=1                 
              
END   
  