create or alter trigger PreserveItemOrder
	on TodoItems
	for delete
as
declare @orderNum int
select top 1 @orderNum = OrderNumber from deleted;
update TodoItems set OrderNumber = OrderNumber - 1 where OrderNumber > @orderNum