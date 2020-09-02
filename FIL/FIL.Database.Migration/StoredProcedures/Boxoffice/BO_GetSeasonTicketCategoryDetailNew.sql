CREATE PROC [dbo].[BO_GetSeasonTicketCategoryDetailNew] --16751                              
(        
--Declare                              
	@VMCC_Id BIGINT-- =  16751                           
)                              
AS                              
BEGIN                              
                
DECLARE @NoOfMatch INT                              
          
 DECLARE @EventDetailId INT, @TicketCategoryId INT, @VenueId INT, @EventId INT            
 SELECT @EventDetailId=EventDetailId from EventTicketDetails WITH(NOLOCK) where Id In(Select EventTicketDetailId from EventTicketAttributes WITH(NOLOCK) where Id=@VMCC_Id)            
 SELECT @TicketCategoryId=TicketCategoryId from EventTicketDetails WITH(NOLOCK) where Id In(Select EventTicketDetailId from EventTicketAttributes WITH(NOLOCK) where Id=@VMCC_Id)           
 Select @VenueId=Id from Venues WITH(NOLOCK) where Id In (Select VenueId from EventDetails WITH(NOLOCK) where Id=@EventDetailId)          
 SELECT @EventId=EventId from EventDetails WITH(NOLOCK) where Id=@EventDetailId            
           
SELECT @NoOfMatch = Count(Id)  FROM EventTicketAttributes A WITH(NOLOCK)          
 WHERE A.EventTicketDetailId In( Select Id from EventTicketDetails WITH(NOLOCK) where  TicketCategoryId=@TicketCategoryId           
 --AND EventDetailId IN(Select Id From EventDetails where VenueId=@VenueId and EventId=@EventId and IsEnabled=1))         
 AND EventDetailId IN(Select Id From EventDetails WITH(NOLOCK) where VenueId=@VenueId and EventId=@EventId and Id Not In(22413,22414,22418,22419)))        
 AND  SeasonPackage=1                  
 AND A.IsEnabled=1             
           
 --print  @NoOfMatch                               
                              
SELECT ETA.Id as VMCC_Id,TC.Name as VenueCatName, ETA.TicketCategoryDescription as VenueCatDetail,                 
(CAST(SeasonPackageLocalPrice as decimal(18,2)) / @NoOfMatch )  AS LocalPricePerTic,                             
RemainingTicketForSale as TotalTic, ED.Name as EventName, ED.StartDateTime as EventStartDate, ED.StartDateTime as EventStartTime, CT.Code as CurrencyName,                              
VD.Name as VenueAddress, C.Name as City_Name, ETA.RemainingTicketForSale AS TicPerTrans, ETA.LocalCurrencyId as CurrencyId, E.EventCategoryId As P_Id,                               
'false' AS IsFamilyPkgAvailable, 0 AS DTCMCategoryId, 0 AS FamilyPkgPrice,                              
'' AS PriceTypeId,'' AS PriceTypeCode, ETD.EventDetailId as EventID ,                            
Isnull(ETA.ChildQTY,0) as ChildQTY ,IsNull(ETA.SRCitizenQTY,0) As SRCitizenTotalQTY,Isnull(ETA.ChildDiscount,0)as ChildDiscount,Isnull(ETA.SrCitizenDiscount,0)as SrCitizenDiscount,            
Isnull(ETA.SeasonPackageLocalPrice,0) as SeasonPrice,
Isnull(ETA.SeasonPackage,0) as IsSeason                             
from EventTicketAttributes ETA WITH(NOLOCK)             
Inner JOin EventTicketDetails ETD  WITH(NOLOCK) On ETA.EventTicketDetailId=ETD.ID                          
INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.ID                              
INNER JOIN  EventDetails ED WITH(NOLOCK) ON ED.Id = ETD.EventDetailId                               
INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.LocalCurrencyId          
INNER JOIN Venues VD WITH(NOLOCK) ON Vd.Id = ED.VenueId                              
INNER JOIN Cities C WITH(NOLOCK) ON C.Id = VD.CityId                              
INNER JOIN Events E WITH(NOLOCK) ON E.ID = ED.EventId                              
WHERE ETA.IsEnabled=1 AND ETA.Id=@VMCC_Id                              
ORDER BY Price DESC                              
          
END     
  

  