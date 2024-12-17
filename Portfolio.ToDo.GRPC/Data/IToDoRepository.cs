using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.Data
{
    public interface IToDoRepository
    {
        Task<IToDoItem> GetItemByIdAsync(Guid id);

        Task<IEnumerable<IToDoItem>> GetItemListAsync();

        Task<IToDoItem> SaveItemAsync(IToDoItem item);

        Task<bool> DeleteItemByIdAsync(Guid id);
    }
}
