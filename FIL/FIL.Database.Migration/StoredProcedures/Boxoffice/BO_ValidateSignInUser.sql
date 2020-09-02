CREATE proc [dbo].[BO_ValidateSignInUser] -- 'shashikant.pandey@kyazoonga.com','AQAAAAEAACcQAAAAEEKMogU5PH2MJW9aP8Fi3PmAjICJdQvKfZc6HceE7g1Ilo8xa9IUsPisV/aw/utX8g=='  
(                    
	@Email VARCHAR(100),                    
	@Pwd VARCHAR(100)                    
)        
AS         
BEGIN         
IF EXISTS (SELECT UserName FROM Users WITH(NOLOCK) WHERE UserName = @Email AND [Password] = @Pwd AND IsEnabled = 1 AND ChannelId = 4)        
BEGIN        
	SELECT U.Id as Retailer_Id, UserName,Password,Isnull(CONVERT(int ,IsChildTicket),0) AS [IsChildAvialble], 
	ISNULL(CONVERT(INT,IsSrCitizenTicket),0) as [IsSrCitizenAvialble],      
	0 AS EventCatId,0 as IsJlfDiscount, 0 AS IsReceiptPrint      
	FROM Users U WITH(NOLOCK)
	LEFT OUTER JOIN BoxofficeUserAdditionalDetails  UAD  WITH(NOLOCK) ON U.ID= UAD.UserId
	WHERE UserName = @Email  AND [Password] = @Pwd AND U.IsEnabled = 1 AND ChannelId = 4 
END  
END 