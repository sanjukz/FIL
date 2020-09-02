CREATE PROC [dbo].[BO_ApplyDiscountAmount] --210000526              
(                
 @EventTransId BIGINT                
)                
AS                
BEGIN              
DECLARE @tblTemp Table (ID INT, TD_Id BIGINT, PricePerTic DECIMAL(18,2), NoOfTic INT)                
      
-----------------------------------------------------------------              
--Buy 4 Get 50% Discount CWI_LUCIA              
-------------------------------------------------------------------------              
INSERT INTO @tblTemp                
SELECT ROW_NUMBER() OVER ( ORDER BY B.Id ), B.Id, B.PricePerTicket, B.TotalTickets               
FROM Transactions A WITH(NOLOCK)                
INNER JOIN TransactionDetails B WITH(NOLOCK) ON A.Id = B.TransactionId                
INNER JOIN EventTicketAttributes C WITH(NOLOCK) ON B.EventTicketAttributeId=C.Id       
INNER JOIN EventTicketDetails D WITH(NOLOCK) ON C.EventTicketDetailId=D.Id             
INNER JOIN TicketCategories E WITH(NOLOCK) ON D.TicketCategoryId = E.Id                
INNER JOIN EventDetails F WITH(NOLOCK) ON D.EventDetailId = F.Id                
WHERE A.ID =@EventTransId      
AND ISNULL(IsSeasonPackage,0) = 0        
AND F.ID  IN (3449)  
And B.TicketTypeId=1             
      
DECLARE @TotalNoOfTic INT = 0, @LoopCount INT = 0, @Counter INT = 1, @InnerCounter INT = 1, @TotalDiscountAmount DECIMAL(18,2) = 0, @DiscountAmount DECIMAL(18,2) = 0, @VMCCMT_Id BIGINT                
DECLARE @PricePerTic DECIMAL(18,2);   
DECLARE @TotalTicketAmount  DECIMAL(18,2)=0.00;           
  
SET @Counter = 1               
               
SELECT @LoopCount = COUNT(*) FROM @tblTemp                
WHILE (@Counter <= @LoopCount)                
BEGIN              
SELECT @TotalNoOfTic = SUM(NoOfTic) FROM @tblTemp WHERE ID = @Counter         
           
IF(@TotalNoOfTic = 4 )               
BEGIN                
 SELECT @VMCCMT_Id = TD_Id, @PricePerTic = (PricePerTic-10)  FROM @tblTemp WHERE ID = @Counter                
 UPDATE TransactionDetails SET PricePerTicket = @PricePerTic WHERE Id = @VMCCMT_Id                
 SET @TotalTicketAmount = 10*@TotalNoOfTic                
END      
     
SET @Counter = @Counter + 1                
END       
       
DELETE FROM @tblTemp       
                
UPDATE Transactions SET NetTicketAmount -= @TotalTicketAmount,GrossTicketAmount-=@TotalTicketAmount WHERE Id = @EventTransId               
SELECT NetTicketAmount as TotalTicAmt FROM Transactions WITH(NOLOCK) WHERE Id = @EventTransId                
              
END 




