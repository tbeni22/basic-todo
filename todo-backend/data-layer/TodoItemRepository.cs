using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using data_layer.ORM;
using Microsoft.EntityFrameworkCore;

namespace data_layer
{
    public class NoSuchRecordException : Exception
    {
        public NoSuchRecordException(string message = null) : base(message) { }
    }

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoDbContext dbContext;

        public TodoItemRepository(TodoDbContext db)
        {
            this.dbContext = db;
        }

        public IReadOnlyCollection<TodoItem> ListItems()
        {
            var items = from ti in dbContext.TodoItems.AsNoTracking()
                            .Include(ti => ti.Category)
                        orderby ti.OrderNumber
                        select new TodoItem(ti);
            return items.ToList();
        }

        public TodoItem GetItemById(int id)
        {
            try
            {
                var item = from i in dbContext.TodoItems.AsNoTracking()
                                .Include(ti => ti.Category)
                            where i.ID == id
                            select new TodoItem(i);
                return item.Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new NoSuchRecordException(ex.Message);
            }
        }

        public (TodoItem, bool) Insert(TodoItem item)
        {
            try
            {
                int orderNumber;
                if (item.OrderNumber == null)
                {
                    try
                    {
                        var maxOrderNum = dbContext.TodoItems.Max(ti => ti.OrderNumber);
                        orderNumber = maxOrderNum + 1;
                    }
                    catch (InvalidOperationException)
                    {
                        orderNumber = 0;    // necessary if table is empty
                    }
                }
                else
                    orderNumber = (int)item.OrderNumber;
                DbTodoItem newItem = new DbTodoItem(item.Title, item.Description != null ? item.Description : "", item.Deadline, orderNumber);
                newItem.Category = (
                        from c in dbContext.Categories
                        where c.Name.Equals(item.CategoryName)
                        select c
                    ).Single();
                dbContext.Add(newItem);
                int rowsModified = dbContext.SaveChanges();
                return (new TodoItem(newItem), rowsModified == 1);
            }
            catch (InvalidOperationException)
            {
                throw new NoSuchRecordException();
            }
        }

        public bool Remove(int id)
        {
            try
            {
                dbContext.Remove(
                        (
                            from i in dbContext.TodoItems
                            where i.ID == id
                            select i
                        )
                        .Single()
                    );
                return dbContext.SaveChanges() == 1;
            } 
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IReadOnlyCollection<TodoItem> ListItemsByCategory(string category)
        {
            var items = from ti in dbContext.TodoItems.AsNoTracking()
                            .Include(ti => ti.Category)
                        where ti.Category.Name == category
                        orderby ti.OrderNumber
                        select new TodoItem(ti);
            return items.ToList();
        }

        public bool UpdateItem(TodoItem updatedItem)
        {
            try
            {
                var itemToUpdate = (from ti in dbContext.TodoItems
                                    where ti.ID == updatedItem.ID
                                    select ti)
                                    .Single();
                itemToUpdate.Title = updatedItem.Title;
                itemToUpdate.Description = updatedItem.Description;
                itemToUpdate.Deadline = updatedItem.Deadline;
                if (updatedItem.OrderNumber != null)
                    itemToUpdate.OrderNumber = (int)updatedItem.OrderNumber;

                var newCategory = from c in dbContext.Categories
                                    where c.Name == updatedItem.CategoryName
                                    select c;
                itemToUpdate.Category = newCategory.Single();

                return dbContext.SaveChanges() == 1;
            }
            catch (InvalidOperationException)
            {
                throw new NoSuchRecordException();
            }
        }
    }
}
