ALTER  PROC [dbo].[Scan_GetGate] --26                
(                  
  @LayoutId INT                  
)                  
AS                  
BEGIN           
 SELECT DISTINCT IsNull(D.ScanGateName,G.Name) AS Gate FROM MastervenueLayouts A  WITH(NOLOCK)               
 INNER JOIN TournamentLayoutS B WITH(NOLOCK) ON A.Id = B.MasterVenueLayoutId                
 INNER JOIN MatchLayouts C WITH(NOLOCK) ON B.Id = C.TournamentLayoutId                
 INNER JOIN MatchLayoutSections D WITH(NOLOCK) ON C.Id = D.MatchLayoutId    
 INNER JOIN EntryGates G WITH(NOLOCK) ON D.EntryGateId = G.Id 
 INNER JOIN EventDetails ED WITH(NOLOCK) ON ED.ID=C.EventDetailId              
 WHERE A.Id =@LayoutId AND ISNULL(D.ScanGateName,G.Name)<>'' and ED.IsEnabled=1           
 UNION             
 SELECT 'AllGate' AS Gate     
END   