using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using data_layer;

namespace api_controllers
{
    public class TodoItemManager
    {
        private readonly ITodoItemRepository repo;

        public TodoItemManager(ITodoItemRepository context)
        {
            repo = context;
        }

        public IReadOnlyCollection<TodoItem> ListItems()
            => repo.ListItems();
        public TodoItem GetItemById(int id)
            => repo.GetItemById(id);
        public  (TodoItem, bool) Insert(TodoItem item)
            => repo.Insert(item);
        public bool Remove(int id)
            => repo.Remove(id);
        public bool UpdateItem(TodoItem updatedItem)
            => repo.UpdateItem(updatedItem);

        /// <summary>
        /// Moves a todo item to a different place in the order (atomic)
        /// </summary>
        /// <param name="itemToMove">The todo item to move, with the new order number set.</param>
        /// <returns>Wheter the operation was successful or not</returns>
        public bool MoveItem(TodoItem itemToMove)
        {
            if (itemToMove.OrderNumber == null) return false;

            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var itemFromDb = repo.GetItemById(itemToMove.ID);
                if (itemFromDb == null) return false;

                // if the item is already in its correct place, the operation is done
                if (itemFromDb.OrderNumber == itemToMove.OrderNumber)
                    return true;

                var itemToSwitch = repo.GetItemByOrderNumber((int)itemToMove.OrderNumber);
                if (itemToSwitch == null) return false;

                itemToSwitch.OrderNumber = itemFromDb.OrderNumber;
                itemFromDb.OrderNumber = itemToMove.OrderNumber;

                repo.UpdateItem(itemFromDb);
                repo.UpdateItem(itemToSwitch);

                tran.Complete();
                return true;
            }
        }
    }
}
