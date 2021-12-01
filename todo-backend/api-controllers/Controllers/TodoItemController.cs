using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data_layer;

namespace api_controllers.Controllers
{
    [Route("todos")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoItemRepository repo;

        public TodoItemController(TodoItemRepository context)
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
            try
            {
                var item = repo.GetItemById(id);
                return Ok(item);
            }
            catch (NoSuchRecordException)
            {
                return NotFound();
            }
        }

        // POST todos
        [HttpPost]
        public ActionResult<int> Post([FromBody] TodoItem newItem)
        {
            try
            {
                (int id, bool modified) = repo.Insert(newItem);
                return modified ? Created("todos/" + id, newItem) : new StatusCodeResult(500);
            }
            catch (NoSuchRecordException)
            {
                return BadRequest();
            }
        }

        // PUT todos/<id>
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] int id, [FromBody] TodoItem item) // todo: are there problems with properties without setters?
        {
            try
            {
                return repo.UpdateItem(item) ? Ok() : new StatusCodeResult(500);
            }
            catch (NoSuchRecordException)
            {
                return BadRequest();
            }
        }

        // DELETE todos/<id>
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            repo.Remove(id);
            return NoContent();
        }
    }
}
