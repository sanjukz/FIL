CREATE proc [dbo].[ITKTS_BO_getDetailsbyTransaID] 
(  
	@TransId VARCHAR(50)  
)  
AS  
BEGIN  
	SELECT * FROM Transactions  WITH(NOLOCK) Where  ID=@TransId  and ChannelId=4  
END  