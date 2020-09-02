CREATE PROCEDURE [dbo].[BO_ApplyPromoCode]  --'ST.LUCIA',210001836                                                                                          
(                                                                                                  
	@PromoCode VARCHAR(100),                                                                                                  
	@EventTransId BIGINT                                                                                                  
)                                                                                                  
AS                                                                                                  
BEGIN                           
DECLARE @tblTemp Table (ID INT IDENTITY(1,1), VMCCMT_Id BIGINT, PricePerTic DECIMAL(18,2), NoOfTic INT, PaidTickets INT)                        
DECLARE @tblPrice Table (ID INT IDENTITY(1,1), VMCCMT_Id BIGINT, PricePerTic DECIMAL(18,2))                        
DECLARE @TotalNoOfTic INT = 0, @LoopCount INT = 0, @Counter INT = 1, @InnerCounter INT = 1, @TotalDiscountAmount DECIMAL(18,2) = 0, @DiscountAmount DECIMAL(18,2) = 0, @VMCCMT_Id BIGINT ,@PricePerTic DECIMAL(18,2),@TotalTicketAmount   DECIMAL(18,2)                                
                        
IF(UPPER(@PromoCode) = 'STLC0')                                            
BEGIN                        
INSERT INTO @tblTemp                        
SELECT B.Id, B.PricePerTicket, B.TotalTickets, 0 
FROM Transactions A WITH(NOLOCK)                
INNER JOIN TransactionDetails B WITH(NOLOCK) ON A.Id = B.TransactionId                
INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.EventTicketAttributeId=C.Id       
INNER JOIN EventTicketDetails D WITH(NOLOCK) ON C.EventTicketDetailId=D.Id             
INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id                
INNER JOIN EventDetails F WITH(NOLOCK) ON D.EventDetailId = F.Id                
WHERE A.ID =@EventTransId      
AND IsNUll(IsSeasonPackage,0) = 0        
AND F.ID  IN (3449)  
And B.TicketTypeId=1 
Print 1                                 
                        
 SET @Counter = 1                        
 SELECT @LoopCount = COUNT(*) FROM @tblTemp                        
 IF((SELECT SUM(NoOfTic) FROM @tblTemp) >0)                        
 BEGIN                        
 WHILE (@Counter <= @LoopCount)                        
 BEGIN                        
       
 Print 2 
 SELECT @VMCCMT_Id = VMCCMT_Id, @PricePerTic = (PricePerTic-PricePerTic)  FROM @tblTemp WHERE ID = @Counter                
 UPDATE TransactionDetails SET PricePerTicket = @PricePerTic  WHERE Id = @VMCCMT_Id   AND  PricePerTicket<>0              
 SET @TotalTicketAmount = 0*@TotalNoOfTic   
 Print  @PricePerTic 
  Print  @TotalTicketAmount 
                
  SET @Counter = @Counter + 1                        
 END                        
  
	Print 3  
	UPDATE Transactions SET NetTicketAmount = @TotalTicketAmount,GrossTicketAmount=@TotalTicketAmount WHERE Id = @EventTransId AND NetTicketAmount<>0 AND GrossTicketAmount<>0                                                 
    SELECT *,0 as DiscountAmount ,'ST.Lucia' as PromoString,0 as PromoEvent, 'Promocode applied sucessfully.' as PromoMessage, '1' as PromoStatus FROM Transactions WITH(NOLOCK) 
	WHERE Id = @EventTransId                        
 END                         
 ELSE                        
 BEGIN                        
 SELECT *,0 as DiscountAmount ,'ST.Lucia' as PromoString,0 as PromoEvent, 'Promocode applied sucessfully.' as PromoMessage, '1' as PromoStatus FROM Transactions WITH(NOLOCK)  
	WHERE Id = @EventTransId           
               
   END                        
END  
                         
 ELSE                                      
  BEGIN                                     
   SELECT *,0 as DiscountAmount ,'Invalid' as PromoString,0 as PromoEvent, 'Please insert a valid promocode' as PromoMessage, '0' as PromoStatus FROM Transactions WITH(NOLOCK) 
   WHERE Id = @EventTransId                  
  END                 
  END    