CREATE PROC [dbo].[BO_PickUpSummary]
AS
BEGIN
SELECT                                                                                                                              
(SELECT COUNT(DISTINCT A.Id) FROM Transactions A WITH(NOlock)  
INNER JOIN TransactionDetails B WITH(NOlock)  ON A.Id =B.TransactionId
INNER JOIN TransactionDeliveryDetails TDD WITH(NOlock)  ON B.Id=TDD.TransactionDetailId 
WHERE A.TransactionStatusId=8 And B.EventTicketAttributeId 
IN(SELECT Id FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId   
IN(SELECT Id FROM EventTicketDetails WITH(NOLOCK)  WHERE EventDetailId In(182268,182269) and IsEnabled=1))
AND TDD.DeliveryStatus=1) AS Picked,      
(SELECT COUNT(DISTINCT A.Id) FROM Transactions A WITH(NOLOCK) 
INNER JOIN TransactionDetails B WITH(NOLOCK) On A.Id =B.TransactionId
INNER JOIN TransactionDeliveryDetails TDD WITH(NOLOCK) On B.Id=TDD.TransactionDetailId 
WHERE A.TransactionStatusId=8 And B.EventTicketAttributeId
IN(Select Id from EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId   
IN(SELECT Id FROM EventTicketDetails WITH(NOLOCK) WHERE EventDetailId In(182268,182269) and IsEnabled=1))
AND ISNull(TDD.DeliveryStatus,0)=0) AS NotPicked,      
(SELECT COUNT(DISTINCT A.Id) FROM Transactions A WITH(NOLOCK) 
INNER JOIN TransactionDetails B WITH(NOLOCK) On A.Id =B.TransactionId
INNER JOIN TransactionDeliveryDetails TDD WITH(NOLOCK) On B.Id=TDD.TransactionDetailId 
WHERE A.TransactionStatusId=8 And B.EventTicketAttributeId 
IN(SELECT Id FROM EventTicketAttributes WITH(NOLOCK) WHERE EventTicketDetailId   
IN(SELECT Id from EventTicketDetails WITH(NOLOCK) WHERE EventDetailId IN(182268,182269) AND IsEnabled=1))) AS Total 
END     