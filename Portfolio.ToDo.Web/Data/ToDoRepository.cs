using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Data
{
    public class ToDoRepository(ToDoService.ToDoServiceClient client) : IToDoRepository
    {
        public Task<IToDoItem> GetItemByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<IToDoItem>> GetItemListAsync()
        {
            try
            {
                ToDoListResponse response = await client.GetAllAsync(new Empty());

                IQueryable<IToDoItem> items = response.Items.Select(item => new ToDoItem
                {
                    Id = Guid.Parse(item.Id),
                    Title = item.Title,
                    Description = item.Description,
                    IsComplete = item.IsComplete,
                    SortOrder = item.SortOrder
                }).AsQueryable<IToDoItem>();

                return items;
            }
            catch (RpcException e)
            {
                throw new Exception($"gRPC error: {e.Status.Detail}");
            }
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
