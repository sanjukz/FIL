CREATE PROC [dbo].[BO_GetFloatamtReport] --4683  
(               
	@Retailer_id BIGINT             
)
AS              
BEGIN 
	SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY FA.PCash_Id DESC) AS [Sr. No.], U.FirstName+' '+U.LastName AS [BO User Name], 
	U.UserName AS [Box Office ID], FA.[AmtIn(USD)] as [Amount (USD)],FA.[AmtIn(Local)] as [Amount (Local Currency)], 
	FA.CreatedDate AS [Date/Time], CM.Code as CurrencyName              
	FROM BO_FloatAmount FA WITH(NOLOCK) 
	INNER JOIN Users U WITH(NOLOCK) on FA.Retailer_id=U.ID       
	LEFT OUTER JOIN BoxofficeUserAdditionalDetails UAD WITH(NOLOCK) on UAD.UserId=U.Id      
	INNER  JOIN EventDetails ED WITH(NOLOCK) on ED.VenueId=FA.VenueId and ED.EventId in (select distinct EventId from BoUserVenues where UserId=@Retailer_id and IsEnabled=1)    
	LEFT OUTER JOIN EventAttributes EA  WITH(NOLOCK) On EA.EventDetailId=ED.Id  
	INNER JOIN CurrencyTypes CM WITH(NOLOCK) on CM.Id= ISNULL(FA.LocalCurrencyId,1)
	WHERE U.Id=@Retailer_id  -- and FA.CreatedDate > '2017-03-01 00:00:00.000'             
	GROUP BY FA.PCash_Id,u.FirstName,u.LastName,u.UserName,FA.[AmtIn(USD)],FA.[AmtIn(Local)],FA.CreatedDate,
	EA.TimeZone, CM.Code
END    