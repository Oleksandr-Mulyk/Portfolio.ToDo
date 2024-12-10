using OriginToDoItem = Portfolio.ToDo.ToDoList.ToDoItem;

namespace Portfolio.ToDo.GRPC.Data
{
    public class ToDoItem : OriginToDoItem
    {
        public Guid Id { get; set; }
    }
}
