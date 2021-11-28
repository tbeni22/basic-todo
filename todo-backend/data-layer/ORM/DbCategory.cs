using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_layer.ORM
{
    [Table("Categories")]
    public class DbCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DbTodoItem> TodoItems { get; set; }
    }
}
