namespace MiniToDo.Models
{
    public class CompleteTaskViewModel
    {
        public List<TaskItem> Tasks { get; set; }
        public int TaskIdToComplete { get; set; }
    }
}
