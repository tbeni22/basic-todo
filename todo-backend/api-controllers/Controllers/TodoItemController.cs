using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data_layer;

#nullable enable

namespace api_controllers.Controllers
{
    [Route("todos")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoItemManager manager;

        public TodoItemController(TodoItemManager context)
        {
            manager = context;
        }

        // GET todos/
        // Get the list of todo items
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return manager.ListItems();
        }

        // GET todos/<id>
        // Get the data of a todo with the specified id
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get([FromRoute] int id)
        {
            var item = manager.GetItemById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST todos/
        // Add a new todo
        [HttpPost]
        public ActionResult Post([FromBody] TodoItem newItem)
        {
            if (newItem.Title == null) return BadRequest();
            (TodoItem? item, bool modified) = manager.Insert(newItem);
            if (item == null) return BadRequest();
            return modified ? Created("todos/" + item.ID, item) : new StatusCodeResult(500);
        }

        // PUT todos/<id>
        // Update an existing todo
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] int id, [FromBody] TodoItem item) // todo: are there problems with properties without setters?
        {
            if (id != item.ID) return BadRequest();
            return manager.UpdateItem(item) ? Ok() : BadRequest();
        }

        // DELETE todos/<id>
        // Delete a todo
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            manager.Remove(id);
            return NoContent();
        }

        // PUT todos/<id>/move
        // Move a todo to a different place in the order (atomic)
        [HttpPut("{id}/move")]
        public ActionResult MoveItem([FromRoute] int id, [FromBody] TodoItem item)
        {
            if (id != item.ID) return BadRequest();
            bool success = manager.MoveItem(item);
            return success ? Ok() : BadRequest();
        }
    }
}
