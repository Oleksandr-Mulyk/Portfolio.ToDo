using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Components.Pages
{
    public partial class Todolist(IToDoRepository repository)
    {
        private List<IToDoItem> _toDoItems = [.. repository.GetItemListAsync().Result];
    }
}
