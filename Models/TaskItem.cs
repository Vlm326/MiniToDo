namespace MiniToDo.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public bool IsCompleted { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public TaskItem(){} 
    public TaskItem(string title, string category, bool isCompleted, string userId)
    {
        Title = title;
        Category = category;
        IsCompleted = isCompleted;
        UserId = userId;
    }
}