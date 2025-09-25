using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TaskItem>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)// ActionResult represents a general HTTP response or a TaskItem
    {
        var task = await _context.Tasks.FindAsync(id); // FindAsync() will not lock the thread (good when there are many requests)
        if (task == null)
        {
            return NotFound(); // if task doesn't exist return HTTP 404
        }
        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(); // SaveChangesAsync() will not lock the thread (good when there are many requests)

        // Returns 201 Created + location (url) of new task (standard REST practice)
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskItem task) // IActionResult represents a general HTTP response.
    {
        if (id != task.Id)
        {
            return BadRequest(); // in case of bad request return HTTP 400 (IDs don’t match)
        }

        // _context.Tasks.Attach(task) would be better and more efficient if we didn't want to update all fields
        // Updates the Task and marks them as dirty (to be updated)
        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (TaskExists(id))
            {
                return NotFound(); // Task no longer exists (someone else deleted it), return HTTP 404
            }
            else
            {
                throw; // some other concurrency issue, rethrow the exception
            }
        }

        return NoContent(); // 204(No Content) when update is successful
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound(); // if task doesn't exist return HTTP 404
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent(); // 204(No Content) when delete is successful
    }
    
    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }
}