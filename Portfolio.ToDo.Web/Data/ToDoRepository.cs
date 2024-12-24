using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Data
{
    public class ToDoRepository : IToDoRepository
    {
        public Task<IToDoItem> GetItemByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<IToDoItem>> GetItemListAsync()
        {
            IQueryable<IToDoItem> items = new List<ToDoItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1", IsComplete = false, SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2", IsComplete = true, SortOrder = 2 },
            }.AsQueryable();

            return items;
        }

        public Task<IToDoItem> SaveItemAsync(IToDoItem item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
