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

            repo.Setup(r => r.GetItemById(It.IsAny<int>()))
                .Returns((TodoItem)null);
        }

        [TestMethod]
        public void Test_MoveNonExistent()
        {
            var item = new TodoItem(1,"","",null, 2, "");

            repo.Setup(r => r.GetItemById(It.IsAny<int>()))
                .Returns((TodoItem)null);

            bool result = manager.MoveItem(item);

            Assert.IsFalse(result, "Move should fail becouse of the non-existent item.");
            repo.Verify(r => r.GetItemById(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void Test_MoveWithoutOrderNumber()
        {
            bool result = manager.MoveItem(new TodoItem());

            Assert.IsFalse(result, "Move should fail becouse of the null value of OrderNumber.");
            repo.Verify(r => r.GetItemById(It.IsAny<int>()), Times.Never);
        }
    }
}
