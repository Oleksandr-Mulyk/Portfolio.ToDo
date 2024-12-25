using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Portfolio.ToDo.ToDoList;
using Portfolio.ToDo.Web.Data;

namespace Portfolio.ToDo.Web.UnitTest
{
    public class ToDoRepositoryTests
    {
        private readonly Mock<ToDoService.ToDoServiceClient> _mockClient;
        private readonly ToDoRepository _repository;

        public ToDoRepositoryTests()
        {
            _mockClient = new Mock<ToDoService.ToDoServiceClient>();
            _repository = new ToDoRepository(_mockClient.Object);
        }

        [Fact]
        public async Task GetItemByIdAsync_ReturnsItem()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            ToDoIdRequest request = new() { Id = id.ToString() };
            ToDoItemProto response = new()
            {
                Id = id.ToString(),
                Title = "Test Title",
                Description = "Test Description",
                IsComplete = false,
                SortOrder = 1
            };

            AsyncUnaryCall<ToDoItemProto> asyncUnaryCall = new(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { });

            _mockClient.Setup(client => client.GetByIdAsync(It.IsAny<ToDoIdRequest>(), null, null, default))
                       .Returns(asyncUnaryCall);

            // Act
            IToDoItem result = await _repository.GetItemByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test Description", result.Description);
            Assert.False(result.IsComplete);
            Assert.Equal(1, result.SortOrder);
        }

        [Fact]
        public async Task GetItemListAsync_ReturnsItems()
        {
            // Arrange
            ToDoListResponse response = new()
            {
                Items = { new ToDoItemProto
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Test Title",
                    Description = "Test Description",
                    IsComplete = false,
                    SortOrder = 1
                }}
            };

            AsyncUnaryCall<ToDoListResponse> asyncUnaryCall = new(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { });

            _mockClient.Setup(client => client.GetAllAsync(It.IsAny<Empty>(), null, null, default))
                       .Returns(asyncUnaryCall);

            // Act
            IQueryable<IToDoItem> result = await _repository.GetItemListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task SaveItemAsync_SavesItem()
        {
            // Arrange
            ToDoItem item = new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                Description = "Test Description",
                IsComplete = false,
                SortOrder = 1
            };

            ToDoItemProto response = new()
            {
                Id = item.Id.ToString(),
                Title = item.Title,
                Description = item.Description,
                IsComplete = item.IsComplete,
                SortOrder = item.SortOrder
            };

            AsyncUnaryCall<ToDoItemProto> asyncUnaryCall = new(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { });

            _mockClient.Setup(client => client.PostAsync(It.IsAny<ToDoItemProto>(), null, null, default))
                       .Returns(asyncUnaryCall);

            // Act
            IToDoItem result = await _repository.SaveItemAsync(item);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Title, result.Title);
            Assert.Equal(item.Description, result.Description);
            Assert.Equal(item.IsComplete, result.IsComplete);
            Assert.Equal(item.SortOrder, result.SortOrder);
        }

        [Fact]
        public async Task DeleteItemByIdAsync_DeletesItem()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            ToDoIdRequest request = new() { Id = id.ToString() };
            ToDoDeleteResponse response = new() { Complete = true };

            AsyncUnaryCall<ToDoDeleteResponse> asyncUnaryCall = new(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { });

            _mockClient.Setup(client => client.DeleteAsync(It.IsAny<ToDoIdRequest>(), null, null, default))
                       .Returns(asyncUnaryCall);

            // Act
            bool result = await _repository.DeleteItemByIdAsync(id);

            // Assert
            Assert.True(result);
        }
    }
}
