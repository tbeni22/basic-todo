if object_id('dbo.TodoItem', 'U') is not null
    drop table TodoItems
if object_id('dbo.Category', 'U') is not null
    drop table Categories

create table Categories (
    ID int not null identity(1,1)
		constraint PK_CategoryID primary key,
    Name nvarchar(63)
);

create table TodoItems (
	ID int not null identity(1,1) 
		constraint PK_TodoItemID primary key,
	Title nvarchar(127) not null,
	Description nvarchar(max) not null,
	Deadline datetime not null,
	CategoryID int not null 
		constraint FK_CategoryID foreign key (CategoryID) references Categories (ID),
	OrderNumber int not null
);

insert into Categories (Name)
values ('Pending'), ('In progress'), ('Done'), ('Postponed');