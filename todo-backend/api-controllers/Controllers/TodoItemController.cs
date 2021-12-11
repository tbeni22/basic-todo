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
        private readonly ITodoItemRepository repo;

        public TodoItemController(ITodoItemRepository context)
        {
            repo = context;
        }

        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return repo.ListItems();
        }

        // GET todos/<id>
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get([FromRoute] int id)
        {
            var item = repo.GetItemById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST todos
        [HttpPost]
        public ActionResult Post([FromBody] TodoItem newItem)
        {
            if (newItem.Title == null) return BadRequest();
            (TodoItem? item, bool modified) = repo.Insert(newItem);
            if (item == null) return BadRequest();
            return modified ? Created("todos/" + item.ID, item) : new StatusCodeResult(500);
        }

        // PUT todos/<id>
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] int id, [FromBody] TodoItem item) // todo: are there problems with properties without setters?
        {
            if (id != item.ID) return BadRequest();
            return repo.UpdateItem(item) ? Ok() : BadRequest();
        }

        // DELETE todos/<id>
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            repo.Remove(id);
            return NoContent();
        }

        [HttpPut("{id}/move")]
        public ActionResult MoveItem([FromRoute] int id, [FromBody] TodoItem item)
        {
            if (id != item.ID) return BadRequest();
            bool success = repo.MoveItem(item);
            return success ? Ok() : BadRequest();
        }
    }
}
