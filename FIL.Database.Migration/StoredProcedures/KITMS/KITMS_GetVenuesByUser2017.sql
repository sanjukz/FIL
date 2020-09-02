CREATE PROCEDURE [dbo].[KITMS_GetVenuesByUser2017] --3
(                    
   @UserId BIGINT                    
)                                                
AS                                                    
BEGIN                                  
   SELECT DISTINCT UVM.VenueId,E.Id AS EventCatId, V.Name +', '+ C.Name +' - '+ E.Name AS VenueAddress
 FROM UserVenueMappings UVM WITH(NOLOCK)
 INNER JOIN Venues V WITH(NOLOCK) ON UVM.VenueId = V.Id AND UVM.UserId = @UserId
 INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id
 INNER JOIN EventDetails ED WITH(NOLOCK) ON V.Id=ED.VenueId
 INNER JOIN Events E WITH(NOLOCK) ON ED.EventId=E.Id 
 WHERE E.Id NOT IN(55)
END