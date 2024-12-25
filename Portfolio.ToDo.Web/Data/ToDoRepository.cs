using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.Web.Data
{
    public class ToDoRepository(ToDoService.ToDoServiceClient client) : IToDoRepository
    {
        public async Task<IToDoItem> GetItemByIdAsync(Guid id)
        {
            try
            {
                ToDoIdRequest request = new() { Id = id.ToString() };
                ToDoItemProto response = await client.GetByIdAsync(request);

                return ProtoToToDoItem(response);
            }
            catch (RpcException e)
            {
                throw new Exception($"gRPC error: {e.Status.Detail}");
            }
        }

        public async Task<IQueryable<IToDoItem>> GetItemListAsync()
        {
            try
            {
                ToDoListResponse response = await client.GetAllAsync(new Empty());

                return response.Items.Select(ProtoToToDoItem).AsQueryable();
            }
            catch (RpcException e)
            {
                throw new Exception($"gRPC error: {e.Status.Detail}");
            }
        }

        public async Task<IToDoItem> SaveItemAsync(IToDoItem item)
        {
            try
            {
                ToDoItemProto request = new()
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Description = item.Description,
                    IsComplete = item.IsComplete,
                    SortOrder = item.SortOrder
                };
                ToDoItemProto response = await client.PostAsync(request);

                return ProtoToToDoItem(response);
            }
            catch (RpcException e)
            {
                throw new Exception($"gRPC error: {e.Status.Detail}");
            }
        }

        public async Task<bool> DeleteItemByIdAsync(Guid id)
        {
            try
            {
                ToDoIdRequest request = new() { Id = id.ToString() };
                ToDoDeleteResponse response = await client.DeleteAsync(request);

                return response.Complete;
            }
            catch (RpcException e)
            {
                throw new Exception($"gRPC error: {e.Status.Detail}");
            }
        }

        private IToDoItem ProtoToToDoItem(ToDoItemProto item) =>
            new ToDoItem
            {
                Id = Guid.Parse(item.Id),
                Title = item.Title,
                Description = item.Description,
                IsComplete = item.IsComplete,
                SortOrder = item.SortOrder
            };
    }
}
