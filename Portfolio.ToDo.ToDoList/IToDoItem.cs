namespace Portfolio.ToDo.ToDoList
{
    public interface IToDoItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }

        public int SortOrder { get; set; }
    }
}
