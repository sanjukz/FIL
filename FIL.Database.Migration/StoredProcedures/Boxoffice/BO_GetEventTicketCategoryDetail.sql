CREATE PROC [dbo].[BO_GetEventTicketCategoryDetail] --182                                        
(                                            
	@VMCC_Id BIGINT                                            
)                                            
AS                                            
BEGIN                                            
	SELECT ETA.Id as VMCC_Id, TC.Name VenueCatName,ETA.TicketCategoryDescription as VenueCatDetail,
	ETA.Price AS PricePerTic,ETA.RemainingTicketForSale AS TotalTic, ED.Name AS EventName, 
	ED.StartDateTime AS EventStartDate, ED.StartDateTime AS EventStartTime, CT.Code AS CurrencyName,                                            
	V.Name AS VenueAddress, c.Name as City_Name, 10 AS TicPerTrans, ETA.LocalCurrencyId AS Currencyid, 
	E.EventCategoryId AS P_Id, 'false' AS IsFamilyPkgAvailable, 0 AS DTCMCategoryId, 0  AS FamilyPkgPrice,                                     
	Isnull(LocalPrice,0) AS LocalPricePerTic, ISNULL(ETA.ChildQTY,0) AS ChildQTY,
	ISNULL(ETA.SRCitizenQTY,0)  AS SRCitizenTotalQTY, ISNULL(ChildDiscount,0) AS [ChildDiscount], 
	ISNULL(SrCitizenDiscount,0) AS [SrCitizenDiscount], ETD.EventDetailId AS EventId, 0 AS SeasonDiscount,                          
	0 AS SeasonPrice, 'false' AS IsSeason                                         
	FROM EventTicketAttributes ETA WITH(NOLOCK)
	Inner Join EventTicketDetails ETD  WITH(NOLOCK) ON ETD.ID=ETA.EventTicketDetailId                                                    
	INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id                                               
	INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.Id = ETD.EventDetailId   
	INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON CT.Id = ETA.LocalCurrencyID                                               
	INNER JOIN Venues V WITH(NOLOCK) ON v.Id = ED.VenueId                                            
	INNER JOIN Cities C WITH(NOLOCK) ON C.ID = V.CityId                                            
	INNER JOIN Events E WITH(NOLOCK) ON E.Id = ED.EventId 
	WHERE ED.IsEnabled=1 AND ETA.Id=@VMCC_Id AND ETA.IsEnabled=1                                             
	ORDER BY Price DESC                        
END       