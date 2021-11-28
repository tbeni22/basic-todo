using System;
using Microsoft.EntityFrameworkCore;

namespace data_layer.ORM
{

    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) 
            : base(options) { }

        public DbSet<DbTodoItem> TodoItems { get; set; }
        public DbSet<DbCategory> Categories { get; set; }
    }
}
