using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniToDo.Data;
using MiniToDo.Models;
using MiniToDo.ViewModels;
using System.Security.Claims;

namespace MiniToDo.Controllers;

[Authorize]
public class TaskController : Controller
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasks = await _context.Tasks.Where(t => t.UserId ==  userId).ToListAsync();
        return View(tasks);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(TaskItem task)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        task.UserId = userId;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Complete()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();

        var model = new CompleteTaskViewModel { Tasks = tasks };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CompleteById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task != null)
        {
            task.IsCompleted = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCompletedTasks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasksToDelete = await _context.Tasks
            .Where(t => t.UserId == userId && t.IsCompleted)
            .ToListAsync();

        _context.Tasks.RemoveRange(tasksToDelete);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}