using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_layer.ORM
{
    [Table("TodoItems")]
    public class DbTodoItem
    {
        public DbTodoItem(string title, string desc, DateTime? deadline, int ordernum)
        {
            Title = title;
            Description = desc;
            Deadline = deadline;
            OrderNumber = ordernum;
        }

        public DbTodoItem() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set;  }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int OrderNumber { get; set; }
        [ForeignKey("CategoryID")]
        public DbCategory Category { get; set; }
    }
}
