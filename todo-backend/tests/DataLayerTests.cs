using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;
using data_layer;
using data_layer.ORM;

namespace tests
{
    [TestClass]
    public class DataLayerTests
    {
        private TodoItemRepository repo;

        [TestInitialize]
        public void Init()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
            contextOptionsBuilder.UseSqlServer(@"data source=(localdb)\mssqllocaldb;initial catalog=TodoAppDB;integrated security=true");
            var db = new TodoDbContext(contextOptionsBuilder.Options);
            repo = new TodoItemRepository(db);
        }

        [TestMethod]
        public void Test_ListItems()
        {
            var items = repo.ListItems();

            foreach (var i in items)
            {
                Console.WriteLine($"id: {i.ID}, title: {i.Title}, desc: {i.Description}, " +
                    $"deadline: {i.Deadline}, category: {i.CategoryName}\n");
            }

            Assert.IsNotNull(items, "List cannot be null!");

        }

        [TestMethod]
        [DataRow(1)]
        public void Test_GetItemById(int id)
        {
            var item = repo.GetItemById(id);

            Assert.IsNotNull(item, "Returned null.");
            Assert.AreEqual(item.ID, id, "Wrong item returned.");
        }

        [TestMethod]
        public void Test_Insert()
        {
            var item = new TodoItem(0, "title", "desc", new System.DateTime(2000, 6, 7, 0, 5, 22), 1, "Done");

            int itemsBeforeInsert = repo.ListItems().Count;

            Assert.AreNotEqual(repo.Insert(item), 0, "New item ID cannot be 0!");
            Assert.AreEqual(repo.ListItems().Count, itemsBeforeInsert + 1, "Item was not inserted!");

        }

        [TestMethod]
        [DataRow(4)]
        public void Test_Remove(int idToRemove)
        {
            Assert.IsTrue(repo.Remove(idToRemove), "Removal failed!");
            Assert.ThrowsException<NoSuchRecordException>(() => repo.GetItemById(idToRemove), "Item was not deleted!");
        }

        [TestMethod]
        [DataRow("Done")]
        public void Test_ListItemsByCategory(string categoryName)
        {
            var items = repo.ListItemsByCategory(categoryName);

            Assert.IsNotNull(items, "List cannot be null!");
            foreach (var item in items)
                Assert.AreEqual(item.CategoryName, categoryName);

            foreach (var i in items)
            {
                Console.WriteLine($"Items in category '{categoryName}':");
                Console.WriteLine($"id{i.ID}:\t title: {i.Title}, " +
                    $"desc: {i.Description}, deadline: {i.Deadline}\n");
            }

        }

        [TestMethod]
        [DataRow(24)]
        public void Test_UpdateItem(int id)
        {
            const string newTitle = "new title";

            TodoItem item = repo.GetItemById(id);
            item.Title = newTitle;

            Assert.IsTrue(repo.UpdateItem(item), "Update failed!");

            item = repo.GetItemById(id);

            Assert.AreEqual(item.Title, newTitle);
        }
    }
}
