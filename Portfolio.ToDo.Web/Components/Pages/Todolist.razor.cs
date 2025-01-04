using Microsoft.AspNetCore.Components;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Components.Pages
{
    public partial class Todolist(IToDoRepository repository) : ComponentBase
    {
        private List<IToDoItem> _toDoItems = [];

        private Guid? _expandedItemId;

        protected override async Task OnInitializedAsync() =>
            _toDoItems = [.. (await repository.GetItemListAsync())];

        private void ToggleDetails(Guid itemId)
        {
            _expandedItemId = _expandedItemId == itemId ? null : itemId;
            StateHasChanged();
        }
    }
}