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

    public class TodoItemRepository
    {
        private readonly string connectionString;

        public TodoItemRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private TodoDbContext createDbContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
            contextOptionsBuilder.UseSqlServer(connectionString);
            return new TodoDbContext(contextOptionsBuilder.Options);
        }

        public IReadOnlyCollection<TodoItem> ListItems()
        {
            using (var dbContext = createDbContext())
            {
                var items = from ti in dbContext.TodoItems.AsNoTracking()
                                .Include(ti => ti.Category)
                            orderby ti.OrderNumber
                            select new TodoItem(ti);
                return items.ToList();
            }
        }

        public TodoItem GetItemById(int id)
        {
            using (var dbContext = createDbContext())
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
        }

        public int Insert(TodoItem item)
        {
            using (var dbContext = createDbContext())
            {
                DbTodoItem newItem = new DbTodoItem(item.Title, item.Description, item.Deadline, item.OrderNumber);
                newItem.Category = (
                        from c in dbContext.Categories.AsNoTracking()
                        where c.Name.Equals(item.CategoryName)
                        select c
                    ).Single();
                dbContext.Add(newItem);
                dbContext.SaveChanges();
                return newItem.ID;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                using (var dbContext = createDbContext())
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
            } 
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IReadOnlyCollection<TodoItem> ListItemsByCategory(string category)
        {
            using (var dbContext = createDbContext())
            {
                var items = from ti in dbContext.TodoItems.AsNoTracking()
                                .Include(ti => ti.Category)
                            where ti.Category.Name == category
                            orderby ti.OrderNumber
                            select new TodoItem(ti);
                return items.ToList();
            }
        }

        public bool UpdateItem(TodoItem updatedItem)
        {
            using (var dbContext = createDbContext())
            {
                var itemToUpdate = (from ti in dbContext.TodoItems
                                   where ti.ID == updatedItem.ID
                                   select ti)
                                   .Single();
                itemToUpdate.Title = updatedItem.Title;
                itemToUpdate.Description = updatedItem.Description;
                itemToUpdate.Deadline = updatedItem.Deadline;
                itemToUpdate.OrderNumber = updatedItem.OrderNumber;

                var newCategory = from c in dbContext.Categories.AsNoTracking()
                                  where c.Name == updatedItem.CategoryName
                                  select c;
                itemToUpdate.Category = newCategory.Single();

                return dbContext.SaveChanges() == 1;
            }
        }
    }
}
