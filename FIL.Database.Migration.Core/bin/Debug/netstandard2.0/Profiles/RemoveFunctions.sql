declare @funcName varchar(500)
declare cur cursor 

for select [name] from sys.objects where type = 'fn'
open cur
fetch next from cur into @funcName
while @@fetch_status = 0
begin
    exec('drop function [' + @funcName + ']')
    fetch next from cur into @funcName
end
close cur
deallocate cur

GO