CREATE PROCEDURE [dbo].[spGetMasterVenues] --3
(                    
   @UserId BIGINT                    
)                                                
AS                                                    
BEGIN                                  
	 SELECT DISTINCT ED.Id  AS VenueId, LayoutName +', '+ D.Name AS VenueAddress
	 FROM UserVenueMappings UVM WITH(NOLOCK)
	 INNER JOIN Venues V WITH(NOLOCK) ON UVM.VenueId = V.Id AND UVM.UserId = @UserId
	 INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
	 INNER JOIN States E WITH(NOLOCK) ON C.StateId = E.Id
	 INNER JOIN Countries D WITH(NOLOCK) ON E.CountryId = D.Id
	 INNER JOIN MasterVenueLayouts ED WITH(NOLOCK) ON V.Id=ED.VenueId
	 WHERE ED.VenueId IN(57,392,60,56)
END
