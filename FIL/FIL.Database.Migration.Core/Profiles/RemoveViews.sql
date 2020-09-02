declare @viewName varchar(500)
declare cur cursor 

for select [name] from sys.objects where type = 'v'
open cur
fetch next from cur into @viewName
while @@fetch_status = 0
begin
    exec('drop view [' + @viewName + ']')
    fetch next from cur into @viewName
end
close cur
deallocate cur

GO