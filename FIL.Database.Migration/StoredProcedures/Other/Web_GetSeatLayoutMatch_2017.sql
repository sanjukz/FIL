CREATE PROCEDURE [dbo].[Web_GetSeatLayoutMatch_2017] --90447    
(               
	@VMCC_ID bigint          
)           
AS           
BEGIN    
Declare @EventTicketDetails bigint;  
Select @EventTicketDetails=EventTicketDetailId from EventTicketAttributes WITH(NOLOCK) where Id=@VMCC_ID  
  
	SELECT A.ID as TicNumId, A.MatchLayoutSectionSeatId as SeatId, B.SeatTag, B.RowNumber, B.ColumnNumber, ISNULL(B.RowOrder, B.RowNumber) AS RowOrder, ISNULL(B.ColumnOrder, B.ColumnNumber) AS ColumnOrder,     
	B.SeatTypeId AS SeatType, A.SeatStatusId As Status, D.Id as VMCC_Id, E.Name as VenueCatName,     
	C.Id as SectionId,CASE WHEN A.SponsorID IS NOT NULL THEN 8 ELSE A.SeatStatusId END as SeatStatusId , A.SponsorID, C.SectionName,     
	A.PrintStatusId AS IsPrint, 1 AS IsAvailable, 0 as IsBoxOffice, 0 as TicPerTrans, 0 as IsWheelChair, 0 as PricePertic, CCM.Name as CurrencyName, SM.SponsorName as SponsorName,'' as ViewFromStand,    
	ISNULL(SM.SponsorName, ETD.FirstName + ' ' + ETD.LastName) AS CustomerName,'Boxoffice' AS TransactionBy      
	-- CASE TransactionBy WHEN 1 THEN 'Website' WHEN 2 THEN 'Retail' WHEN 3 THEN 'Box Office' WHEN 4 THEN 'Corporate' ELSE '' END AS TransactionBy    
	--FROM ITKTSEVNT_TicketNumberDetails A WITH (NOLOCK)    
	--INNER JOIN ITKTS_MatchLayoutSectionSeats B WITH (NOLOCK) ON A.SeatId=B.SeatId AND A.VMCC_Id=@VMCC_ID                         
	--INNER JOIN ITKTS_MatchLayoutSections C WITH (NOLOCK) ON B.SectionId=C.SectionId                            
	--INNER JOIN ITKTSEVNT_VenueMapCategoryClass D WITH (NOLOCK) ON A.VMCC_ID = D.VMCC_ID                     
	--INNER JOIN ITKTSEVNT_VenueCategoryDetails E WITH (NOLOCK) ON E.VenueCatID = D.VenueCatId    
	--LEFT OUTER JOIN ITKTS_CurrencyMapping CMAP WITH (NOLOCK) ON CMAP.Vmcc_id = D.VMCC_Id     
	--LEFT OUTER JOIN ITKTS_CurrencyMaster CCM  WITH (NOLOCK) ON CCM.Currencyid = CMAP.CurrencyId     
	--LEFT OUTER JOIN Sponsor_MST SM WITH (NOLOCK) ON SM.SponsorId = A.SponsorID     
	--LEFT OUTER JOIN ITKTSEVNT_EventTransDetails ETD WITH (NOLOCK) ON ETD.EventTransid = A.TransID    
	--LEFT OUTER JOIN Itkts_UserTransMapping UTM  WITH (NOLOCK) ON UTM.TransID = A.TransID    
	--ORDER BY B.RowOrder DESC, B.ColumnOrder, A.SeatId    
   
	FROM MatchSeatTicketDetails A WITH (NOLOCK)    
	INNER JOIN MatchLayoutSectionSeats B WITH (NOLOCK) ON A.MatchLayoutSectionSeatId=B.Id AND A.EventTicketDetailId=@EventTicketDetails                         
	INNER JOIN MatchLayoutSections C WITH (NOLOCK) ON B.MatchLayoutSectionId=C.Id                            
	INNER JOIN EventTicketAttributes D WITH (NOLOCK) ON A.EventTicketDetailId = D.EventTicketDetailId    
	INNER JOIN EventTicketDetails ED WITH (NOLOCK) ON D.EventTicketDetailId = ED.Id                     
	INNER JOIN TicketCategories E WITH (NOLOCK) ON E.Id = ED.TicketCategoryId  
	LEFT OUTER JOIN CurrencyTypes CCM  WITH (NOLOCK) ON CCM.Id = D.LocalCurrencyId     
	LEFT OUTER JOIN Sponsors SM WITH (NOLOCK) ON SM.Id = A.SponsorId     
	LEFT OUTER JOIN Transactions ETD WITH (NOLOCK) ON ETD.Id = A.TransactionId    
	--LEFT OUTER JOIN Itkts_UserTransMapping UTM  WITH (NOLOCK) ON UTM.TransID = A.TransID    
	ORDER BY B.RowOrder DESC, B.ColumnOrder, A.MatchLayoutSectionSeatId   
END