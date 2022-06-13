using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using api_controllers;
using data_layer;

namespace tests
{
    [TestClass]
    public class TodoManagerUnitTests
    {
        private TodoItemManager manager;
        private Mock<ITodoItemRepository> repo;

        [TestInitialize]
        public void Init()
        {
            repo = new Mock<ITodoItemRepository>();
            manager = new TodoItemManager(repo.Object);
        }

        [TestMethod]
        public void Test_MoveNonExistent()
        {
            var item = new TodoItem(1,"","",null, 2, "");

            repo.Setup(r => r.GetItemById(It.IsAny<int>()))
                .Returns((TodoItem)null);

            bool result = manager.MoveItem(item);

            Assert.IsFalse(result, "Move should fail because of the non-existent item.");
            repo.Verify(r => r.GetItemById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Test_MoveWithoutOrderNumber()
        {
            bool result = manager.MoveItem(new TodoItem());

            Assert.IsFalse(result, "Move should fail because of the null value of OrderNumber.");
            repo.Verify(r => r.GetItemById(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void Test_SuccessfulMove()
        {
            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem(1, "a", ".", null, 1, "none"));
            items.Add(new TodoItem(2, "b", "", null, 2, "none"));
            repo.Setup(r => r.GetItemById(1)).Returns(items[0]);
            repo.Setup(r => r.GetItemByOrderNumber(2)).Returns(items[1]);
            var itemToMove = new TodoItem { ID = 1, OrderNumber = 2 };

            bool result = manager.MoveItem(itemToMove);

            Assert.IsTrue(result, "Move should execute successfully.");
            repo.Verify(r => r.UpdateItem(It.Is<TodoItem>(ti => ti.ID == 1 && ti.OrderNumber == 2)), 
                Times.Once, "Item should be moved in database.");
            repo.Verify(r => r.UpdateItem(It.Is<TodoItem>(ti => ti.ID == 2 && ti.OrderNumber == 1)), 
                Times.Once, "Colliding item should also be moved.");
        }
    }
}
