using System.Collections.Generic;
using System.Threading.Tasks;

namespace data_layer
{
    public interface ITodoItemRepository
    {
        IReadOnlyCollection<TodoItem> ListItems();
        TodoItem GetItemById(int id);
        (TodoItem, bool) Insert(TodoItem item);
        bool Remove(int id);
        IReadOnlyCollection<TodoItem> ListItemsByCategory(string category);
        bool UpdateItem(TodoItem updatedItem);
    }
}
