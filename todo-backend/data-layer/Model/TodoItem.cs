using System;
using data_layer.ORM;

namespace data_layer
{
    /// <summary>
    /// DTO class representing a todo item
    /// </summary>
    public class TodoItem
    {
        public TodoItem() { }

        public TodoItem(int id, string title, string desc, DateTime? deadline, int ordernum, string category)
        {
            ID = id;
            Title = title;
            Description = desc;
            Deadline = deadline;
            OrderNumber = ordernum;
            CategoryName = category;
        }

        public TodoItem(DbTodoItem ti) 
            : this(ti.ID, ti.Title, ti.Description, ti.Deadline, ti.OrderNumber, ti.Category.Name)
        { }

        public int ID { get; set;  }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int? OrderNumber { get; set; }
        public string CategoryName { get; set; }
    }
}
