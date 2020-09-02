CREATE PROC [dbo].[Web_GetSeasonVmccIdTemp] --16216           
(      
  @Vmcc_Id BIGINT                 
)      
AS                 
BEGIN                   
	  DECLARE @EventDetailId INT, @TicketCategoryId INT, @VenueId INT, @EventId INT           
	  Select @EventDetailId=EventDetailId from EventTicketDetails WITH(NOLOCK) where 
	  Id In(Select EventTicketDetailId from EventTicketAttributes WITH(NOLOCK) where Id=@Vmcc_Id)      
	  Select @TicketCategoryId=TicketCategoryId from EventTicketDetails WITH(NOLOCK) where 
	  Id In(Select EventTicketDetailId from EventTicketAttributes WITH(NOLOCK) where Id=@Vmcc_Id)      
	  Select @VenueId=Id from Venues WITH(NOLOCK) where Id In (Select VenueId from EventDetails WITH(NOLOCK) where Id=@EventDetailId)         
	  Select @EventId=EventId from EventDetails WITH(NOLOCK) where Id=@EventDetailId                   
        
	  SELECT Id as VMCC_Id FROM EventTicketAttributes A   WITH(NOLOCK)       
	  WHERE A.EventTicketDetailId In( Select Id from EventTicketDetails WITH(NOLOCK) where  TicketCategoryId=@TicketCategoryId         
	  --AND EventDetailId IN(Select Id From EventDetails where VenueId=@VenueId and EventId=@EventId and IsEnabled=1))      
	  AND EventDetailId IN(Select Id From EventDetails WITH(NOLOCK) where VenueId=@VenueId and EventId=@EventId 
	  And Id Not In(22413,22414,22418,22419)))      
	  AND  SeasonPackage=1 AND A.IsEnabled=1               
  END  
