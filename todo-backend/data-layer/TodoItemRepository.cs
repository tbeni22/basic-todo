using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using data_layer.ORM;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace data_layer
{
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

        public TodoItem? GetItemById(int id)
        {
            var item = from i in dbContext.TodoItems.AsNoTracking()
                            .Include(ti => ti.Category)
                        where i.ID == id
                        select new TodoItem(i);
            return item.SingleOrDefault();
        }

        public (TodoItem?, bool) Insert(TodoItem item)
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
            var category = (
                    from c in dbContext.Categories
                    where c.Name.Equals(item.CategoryName)
                    select c
                ).SingleOrDefault();

            if (category == null) return (null, true);
            newItem.Category = category;
            dbContext.Add(newItem);

            int rowsModified = dbContext.SaveChanges();
            return (new TodoItem(newItem), rowsModified == 1);
        }

        public bool Remove(int id)
        {
            var itemToRemove = (
                            from i in dbContext.TodoItems
                            where i.ID == id
                            select i
                        ).SingleOrDefault();
            if (itemToRemove == null)
                return false;
            dbContext.Remove(itemToRemove);
            return dbContext.SaveChanges() == 1;
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
            var itemToUpdate = (from ti in dbContext.TodoItems
                                where ti.ID == updatedItem.ID
                                select ti)
                                .SingleOrDefault();
            if (itemToUpdate == null)
                return false;
            itemToUpdate.Title = updatedItem.Title;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.Deadline = updatedItem.Deadline;
            if (updatedItem.OrderNumber != null)
                itemToUpdate.OrderNumber = (int)updatedItem.OrderNumber;

            var newCategory = (from c in dbContext.Categories
                                where c.Name == updatedItem.CategoryName
                                select c).SingleOrDefault();
            if (newCategory == null)
                return false;
            itemToUpdate.Category = newCategory;

            dbContext.SaveChanges();
            return true;
        }

        public TodoItem? GetItemByOrderNumber(int orderNumber)
        {
            var dbItem = dbContext.TodoItems
                .Where(i => i.OrderNumber == orderNumber)
                .SingleOrDefault();
            return dbItem == null ? null : new TodoItem(dbItem);
        }

        public bool MoveItem(TodoItem itemToMove)
        {
            if (itemToMove.OrderNumber == null) return false;

            var itemFromDb = dbContext.TodoItems
                .Where(i => i.ID == itemToMove.ID)
                .SingleOrDefault();
            var itemToSwitch = dbContext.TodoItems
                .Where(i => i.OrderNumber == itemToMove.OrderNumber)
                .SingleOrDefault();

            if (itemFromDb == null || itemToSwitch == null) return false;

            itemToSwitch.OrderNumber = itemFromDb.OrderNumber;
            itemFromDb.OrderNumber = (int)itemToMove.OrderNumber;

            return dbContext.SaveChanges() == 2;
        }
    }
}
