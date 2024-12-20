using Microsoft.EntityFrameworkCore;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.Data
{
    public class ToDoRepository(ApplicationDbContext applicationDbContext) : IToDoRepository
    {
        public async Task<IToDoItem> GetItemByIdAsync(Guid id)
            => await applicationDbContext.ToDoItems.FindAsync(id) ?? throw new KeyNotFoundException();

        public async Task<IQueryable<IToDoItem>> GetItemListAsync()
            => await Task.FromResult(applicationDbContext.ToDoItems.AsQueryable());

        public async Task<IToDoItem> SaveItemAsync(IToDoItem item)
        {
            try
            {
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                    await applicationDbContext.ToDoItems.AddAsync(item as ToDoItem ?? throw new InvalidCastException());
                }
                else
                {
                    applicationDbContext.ToDoItems.Update(item as ToDoItem ?? throw new InvalidCastException());
                }

                await applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return item;
        }

        public async Task<bool> DeleteItemByIdAsync(Guid id)
        {
            try
            {
                IToDoItem toDoItem = await GetItemByIdAsync(id);
                applicationDbContext.ToDoItems.Remove(toDoItem as ToDoItem ?? throw new InvalidCastException());
                await applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
