using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable

namespace data_layer
{
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Get the list of todos
        /// </summary>
        /// <returns>The list of todos</returns>
        IReadOnlyCollection<TodoItem> ListItems();
        
        /// <summary>
        /// Get a specific todo
        /// </summary>
        /// <param name="id">ID of the todo to query</param>
        /// <returns>The data of the todo</returns>
        TodoItem? GetItemById(int id);
        
        /// <summary>
        /// Add a new todo
        /// </summary>
        /// <param name="item">The todo to add</param>
        /// <returns>The found todo (or null if it doesnt exist) and wheter the operation was successful or not</returns>
        (TodoItem?, bool) Insert(TodoItem item);

        /// <summary>
        /// Deletes a todo
        /// </summary>
        /// <param name="id">The id of the todo to be removed</param>
        /// <returns>Wheter the operation was successful or not</returns>
        bool Remove(int id);

        /// <summary>
        /// Get a list of todos in a specific category
        /// </summary>
        /// <param name="category">The category name used as the filter</param>
        /// <returns>The list of todos satisfying the category requirement</returns>
        IReadOnlyCollection<TodoItem> ListItemsByCategory(string category);

        /// <summary>
        /// Update the data of a todo
        /// </summary>
        /// <param name="updatedItem">The todo object which contains the modifications</param>
        /// <returns>Wheter the operation was successful or not</returns>
        bool UpdateItem(TodoItem updatedItem);

        /// <summary>
        /// Get the todo with the specified order number
        /// </summary>
        /// <param name="orderNumber">The order number to filter items</param>
        /// <returns>The found todo or null if it does not exist</returns>
        TodoItem? GetItemByOrderNumber(int orderNumber);
    }
}
