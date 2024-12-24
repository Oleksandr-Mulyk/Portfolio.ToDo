using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Portfolio.ToDo.GRPC;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.Services
{
    public class ToDoApiService(IToDoRepository toDoRepository) : ToDoService.ToDoServiceBase
    {
        public override async Task<ToDoItemProto> GetById(ToDoIdRequest toDoIdRequest, ServerCallContext context)
        {
            Guid id = Guid.Parse(toDoIdRequest.Id);
            IToDoItem toDoItem = await toDoRepository.GetItemByIdAsync(id);

            return await Task.FromResult(ToProto(toDoItem));
        }

        public override async Task<ToDoListResponse> GetAll(Empty emptyRequest, ServerCallContext context)
        {
            ToDoListResponse response = new();

            IQueryable<IToDoItem> toDoItems = await toDoRepository.GetItemListAsync();

            List<ToDoItemProto> toDoItemList = [.. toDoItems.Select(toDoItem => ToProto(toDoItem))];

            response.Items.AddRange(toDoItemList);

            return await Task.FromResult(response);
        }

        public override async Task<ToDoItemProto> Post(ToDoItemProto toDoItemProto, ServerCallContext context)
        {

            IToDoItem toDoItem = new ToDoItem
            {
                Id = toDoItemProto is null ? Guid.Parse(toDoItemProto!.Id) : Guid.Empty,
                Title = toDoItemProto.Title,
                Description = toDoItemProto.Description,
                IsComplete = toDoItemProto.IsComplete,
                SortOrder = toDoItemProto.SortOrder
            };

            IToDoItem result = await toDoRepository.SaveItemAsync(toDoItem);

            return ToProto(result);
        }

        public override async Task<ToDoDeleteResponse> Delete(ToDoIdRequest toDoIdRequest, ServerCallContext context)
        {
            Guid id = Guid.Parse(toDoIdRequest.Id);
            bool result = await toDoRepository.DeleteItemByIdAsync(id);

            return new ToDoDeleteResponse() { Complete = result };
        }

        private static ToDoItemProto ToProto(IToDoItem toDoItem)
        {
            return new ToDoItemProto
            {
                Id = toDoItem.Id.ToString(),
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                IsComplete = toDoItem.IsComplete,
                SortOrder = toDoItem.SortOrder
            };
        }
    }
}
