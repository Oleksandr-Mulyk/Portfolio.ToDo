using Microsoft.AspNetCore.Components;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Components.Pages
{
    public partial class Todolist(IToDoRepository repository) : ComponentBase
    {
        private List<IToDoItem> _toDoItems = [];

        private Guid? _expandedItemId;

        private bool _showAddForm = false;

        private ToDoItem _newItem = new();

        private Guid? _editItemId;

        private bool _isEditingTitle = false;

        private bool _isEditingDescription = false;

        protected override async Task OnInitializedAsync() => _toDoItems = [.. (await repository.GetItemListAsync())];

        private void ToggleDetails(Guid itemId) => _expandedItemId = _expandedItemId == itemId ? null : itemId;

        private async Task DeleteItem(Guid itemId)
        {
            await repository.DeleteItemByIdAsync(itemId);
            _toDoItems = [.. (await repository.GetItemListAsync())];
        }

        private void ShowAddForm()
        {
            _showAddForm = true;
            _newItem = new ToDoItem();
        }

        private void HideAddForm() => _showAddForm = false;

        private async Task AddItem()
        {
            await repository.SaveItemAsync(_newItem);
            _toDoItems = [.. (await repository.GetItemListAsync())];
            _showAddForm = false;
        }

        private void EditItem(Guid itemId, bool isTitle)
        {
            _editItemId = itemId;
            _isEditingTitle = isTitle;
            _isEditingDescription = !isTitle;
        }

        private async Task SaveEditItem(IToDoItem item)
        {
            await repository.SaveItemAsync(item);
            _editItemId = null;
            _isEditingTitle = false;
            _isEditingDescription = false;
            _toDoItems = [.. (await repository.GetItemListAsync())];
        }

        private async Task OnCompleteChanged(IToDoItem item, ChangeEventArgs e)
        {
            item.IsComplete = (bool)(e.Value ?? false);
            await SaveEditItem(item);
        }
    }
}