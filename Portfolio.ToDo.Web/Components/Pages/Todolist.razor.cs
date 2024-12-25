using Microsoft.AspNetCore.Components;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Components.Pages
{
    public partial class Todolist(IToDoRepository repository) : ComponentBase
    {
        private List<IToDoItem> _toDoItems;

        protected override async Task OnInitializedAsync() =>
            _toDoItems = [.. (await repository.GetItemListAsync())];
    }
}
