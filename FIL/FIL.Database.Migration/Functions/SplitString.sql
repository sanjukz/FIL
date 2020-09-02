CREATE  FUNCTION [dbo].[SplitString]       
(        
 @String VARCHAR (max),        
 @Delimiter CHAR (1)        
 )        
RETURNS @ValueTable TABLE (KeyWord VARCHAR(4000))        
BEGIN        
 SET @String=@String+@Delimiter+'Terminator'        
 DECLARE @Word VARCHAR(20)        
 WHILE CHARINDEX(@Delimiter,@String,0) <> 0        
 BEGIN        
  SELECT        
  @Word=RTRIM(LTRIM(SUBSTRING(@String,1,CHARINDEX(@Delimiter,@String,0)-1))),        
  @String=RTRIM(LTRIM(SUBSTRING(@String,CHARINDEX(@Delimiter,@String,0)+1,LEN(@String))))        
        
  IF LEN(@Word) > 0        
   insert into @ValueTable (Keyword) Values (@Word)        
 END        
RETURN        
END 