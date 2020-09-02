CREATE PROC spReportEventData --'2C411AC1-AA33-43E1-BA95-221684EB04F4','D0CE577B-5394-432B-9571-05303B190996',0,'2018-01-01 12:00:00','2050-12-31 12:00:00', 2, 50
(        
--DECLARE         
	@EventAltId UniqueIdentifier = '2C411AC1-AA33-43E1-BA95-221684EB04F4' ,         
	@UserAltId UniqueIdentifier = 'D0CE577B-5394-432B-9571-05303B190996',         
	@EventDetailId Bigint = 0,         
	@FromDate Datetime = NULL,         
	@ToDate Datetime = NULL,
	@PageNumber INT = 1,  
	@NoRecordsPerPage INT  = 100
)        
AS        
BEGIN        
--Events Start        
 SELECT * INTO #Events         
 FROM Events WITH (NOLOCK) WHERE AltId = @EventAltId

 CREATE CLUSTERED INDEX Index_Events_Id ON #Events(Id)
--Events End        

--Users Starts        
 SELECT * INTO #LoginUser        
 FROM Users WITH (NOLOCK) WHERE AltId = @UserAltId
--Users End        
        
--EventDetails Starts (Depending on Role & Assignment)        
 CREATE TABLE #EventDetails(Id bigint, Name nvarchar(100), VenueId int, StartDateTime datetime)        
 INSERT INTO #EventDetails        
 SELECT Id, Name, VenueId, StartDateTime         
 FROM EventDetails WITH (NOLOCK)         
 WHERE Id = CASE WHEN @EventDetailId = 0 THEN Id ELSE @EventDetailId END         
 AND EventId IN (SELECT Id FROM #Events)        
         
 IF((SELECT RolesId FROM #LoginUser) <> 1)        
 BEGIN          
  DELETE FROM #EventDetails        
  INSERT INTO #EventDetails        
  SELECT ed.Id, ed.Name, ed.VenueId, ed.StartDateTime FROM EventDetails ed WITH (NOLOCK)         
  INNER JOIN EventsUserMappings eum WITH (NOLOCK) ON ed.Id = eum.EventDetailId and eum.UserId = (SELECT Id FROM #LoginUser)        
  WHERE ed.Id = CASE WHEN @EventDetailId = 0 THEN ed.Id ELSE @EventDetailId END AND ed.EventId IN (SELECT Id FROM #Events)          
 END

 CREATE CLUSTERED INDEX Index_EventDetails_Id ON #EventDetails(Id)
--EventDetails End

--EventAttributes Starts        
 SELECT * INTO #EventAttributes        
 FROM EventAttributes WITH(NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)
--EventAttributes End

--EventTicketDetails Starts        
 SELECT * INTO #EventTicketDetails        
 FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN (SELECT Id FROM #EventDetails)

 CREATE CLUSTERED INDEX Index_EventTicketDetails_Id ON #EventTicketDetails(Id)
--EventTicketDetails End        
        
--TicketCategories Starts        
 SELECT * INTO #TicketCategories        
 FROM TicketCategories WITH(NOLOCK) WHERE Id IN (SELECT TicketCategoryId FROM #EventTicketDetails)
--TicketCategories End        
        
--EventTicketAttributes Starts        
 SELECT * INTO #EventTicketAttributes        
 FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId IN (SELECT Id FROM #EventTicketDetails)

 CREATE CLUSTERED INDEX Index_EventTicketAttributes_Id ON #EventTicketAttributes(Id)
--EventTicketAttributes End        
        
--TicketFeeDetails Starts        
 SELECT * INTO #TicketFeeDetails        
 FROM TicketFeeDetails WITH(NOLOCK) WHERE EventTicketAttributeId IN (SELECT Id FROM #EventTicketAttributes)
--TicketFeeDetails End        

--Venues Starts        
 SELECT * INTO #Venues        
 FROM Venues WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT VenueId FROM #EventDetails)

 CREATE CLUSTERED INDEX Index_Venues_Id ON #Venues(Id)
--Venues End        
        
--Cities Starts        
 SELECT * INTO #Cities        
 FROM Cities WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT CityId FROM #Venues)

 CREATE CLUSTERED INDEX Index_Cities_Id ON #Cities(Id)
--Cities End        
        
--States Starts        
 SELECT * INTO #States        
 FROM States WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT StateId FROM #Cities)

 CREATE CLUSTERED INDEX Index_States_Id ON #States(Id)
--States End        
        
--Countries Starts        
 SELECT * INTO #Countries        
 FROM Countries WITH(NOLOCK) WHERE Id IN (SELECT DISTINCT CountryId FROM #States)
--Countries End        

SELECT * FROM #Events
SELECT * FROM #EventDetails       
SELECT * FROM #EventAttributes
SELECT * FROM #EventTicketDetails      
SELECT * FROM #TicketCategories 
SELECT * FROM #EventTicketAttributes       
SELECT * FROM #TicketFeeDetails
SELECT * FROM #Venues        
SELECT * FROM #Cities 
SELECT * FROM #States  
SELECT * FROM #Countries    

--DROP TABLE #Events        
--DROP TABLE #LoginUser        
--DROP TABLE #EventDetails        
--DROP TABLE #EventTicketDetails        
--DROP TABLE #EventAttributes        
--DROP TABLE #TicketCategories        
--DROP TABLE #EventTicketAttributes        
--DROP TABLE #TicketFeeDetails        
--DROP TABLE #TransactionDetails        
--DROP TABLE #Transactions        
--DROP TABLE #Users        
--DROP TABLE #TransactionDeliveryDetails        
--DROP TABLE #TransactionPaymentDetails        
--DROP TABLE #IPDetails        
--DROP TABLE #UserCardDetails        
--DROP TABLE #CurrencyTypes        
--DROP TABLE #Venues        
--DROP TABLE #Cities        
--DROP TABLE #States        
--DROP TABLE #Countries        
--DROP TABLE #ReportingColumnsUserMappings        
--DROP TABLE #ReportingColumnsMenuMappings        
--DROP TABLE #ReportingColumns      
        
END





