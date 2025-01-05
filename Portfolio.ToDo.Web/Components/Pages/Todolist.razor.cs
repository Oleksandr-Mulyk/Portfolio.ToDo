using Microsoft.AspNetCore.Components;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Components.Pages
{
    public partial class Todolist(IToDoRepository repository, IConfiguration configuration) : ComponentBase
    {
        const string TODO_ITEM_PER_PAGE_CONFIG_KEY = "AppSettings:ToDoItemsPerPage";

        private List<IToDoItem> _toDoItems = [];

        private Guid? _expandedItemId;

        private bool _showAddForm = false;

        private ToDoItem _newItem = new();

        private Guid? _editItemId;

        private bool _isEditingTitle = false;

        private bool _isEditingDescription = false;

        private int CurrentPage { get; set; } = 1;

        private int ItemsPerPage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ItemsPerPage = configuration.GetValue<int>(TODO_ITEM_PER_PAGE_CONFIG_KEY);
            _toDoItems = [.. (await repository.GetItemListAsync())];
        }

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
            _newItem.SortOrder = _toDoItems.OrderByDescending(i => i.SortOrder).FirstOrDefault()?.SortOrder + 1 ?? 0;
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

        private async Task MoveItemUp(IToDoItem item)
        {
            int index = _toDoItems.IndexOf(item);
            if (index > 0)
            {
                _toDoItems.RemoveAt(index);
                _toDoItems.Insert(index - 1, item);
                await UpdateSortOrder();
            }
        }

        private async Task MoveItemDown(IToDoItem item)
        {
            int index = _toDoItems.IndexOf(item);
            if (index < _toDoItems.Count - 1)
            {
                _toDoItems.RemoveAt(index);
                _toDoItems.Insert(index + 1, item);
                await UpdateSortOrder();
            }
        }

        private async Task UpdateSortOrder()
        {
            for (int i = 0; i < _toDoItems.Count; i++)
            {
                _toDoItems[i].SortOrder = i;
                await repository.SaveItemAsync(_toDoItems[i]);
            }
            _toDoItems = [.. (await repository.GetItemListAsync())];
        }

        private IEnumerable<IToDoItem> PagedItems => _toDoItems.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);

        private void OnPageChanged(int page) => CurrentPage = page;
    }
}