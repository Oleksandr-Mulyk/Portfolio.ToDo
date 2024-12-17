namespace Portfolio.ToDo.ToDoList
{
    public class ToDoItem : IToDoItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsComplete { get; set; }

        public int SortOrder { get; set; }
    }
}
