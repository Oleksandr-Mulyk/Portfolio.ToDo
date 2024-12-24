using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Portfolio.ToDo.GRPC;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.GRPC.Services;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.UnitTest
{
    public class ToDoApiServiceTests
    {
        private readonly Mock<IToDoRepository> _mockRepository;
        private readonly ToDoApiService _service;
        private readonly ServerCallContext _context;

        public ToDoApiServiceTests()
        {
            _mockRepository = new Mock<IToDoRepository>();
            _service = new ToDoApiService(_mockRepository.Object);
            _context = new Mock<ServerCallContext>().Object;
        }

        [Fact]
        public async Task GetById_ReturnsToDoItemProto()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            ToDoItem toDoItem = new()
            {
                Id = id,
                Title = "Test Title",
                Description = "Test Description",
                IsComplete = false,
                SortOrder = 1
            };
            _mockRepository.Setup(repo => repo.GetItemByIdAsync(id)).ReturnsAsync(toDoItem);
            ToDoIdRequest request = new()
            { Id = id.ToString() };

            // Act
            var result = await _service.GetById(request, _context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(toDoItem.Id.ToString(), result.Id);
            Assert.Equal(toDoItem.Title, result.Title);
            Assert.Equal(toDoItem.Description, result.Description);
            Assert.Equal(toDoItem.IsComplete, result.IsComplete);
            Assert.Equal(toDoItem.SortOrder, result.SortOrder);
        }

        [Fact]
        public async Task GetAll_ReturnsToDoListResponse()
        {
            // Arrange
            IQueryable<IToDoItem> toDoItems = new List<IToDoItem>
        {
            new ToDoItem
            {
                Id = Guid.NewGuid(),
                Title = "Test Title 1",
                Description = "Test Description 1",
                IsComplete = false,
                SortOrder = 1
            },
            new ToDoItem
            {
                Id = Guid.NewGuid(),
                Title = "Test Title 2",
                Description = "Test Description 2",
                IsComplete = true,
                SortOrder = 2
            }
        }.AsQueryable();
            _mockRepository.Setup(repo => repo.GetItemListAsync()).ReturnsAsync(toDoItems);

            // Act
            ToDoListResponse result = await _service.GetAll(new Empty(), _context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task Post_SavesAndReturnsToDoItemProto()
        {
            // Arrange
            ToDoItemProto toDoItemProto = new()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Title",
                Description = "Test Description",
                IsComplete = false,
                SortOrder = 1
            };
            ToDoItem toDoItem = new()
            {
                Id = Guid.Parse(toDoItemProto.Id),
                Title = toDoItemProto.Title,
                Description = toDoItemProto.Description,
                IsComplete = toDoItemProto.IsComplete,
                SortOrder = toDoItemProto.SortOrder
            };
            _mockRepository.Setup(repo => repo.SaveItemAsync(It.IsAny<IToDoItem>())).ReturnsAsync(toDoItem);

            // Act
            var result = await _service.Post(toDoItemProto, _context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(toDoItemProto.Id, result.Id);
            Assert.Equal(toDoItemProto.Title, result.Title);
            Assert.Equal(toDoItemProto.Description, result.Description);
            Assert.Equal(toDoItemProto.IsComplete, result.IsComplete);
            Assert.Equal(toDoItemProto.SortOrder, result.SortOrder);
        }

        [Fact]
        public async Task Delete_RemovesItemAndReturnsResponse()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteItemByIdAsync(id)).ReturnsAsync(true);
            ToDoIdRequest request = new() { Id = id.ToString() };

            // Act
            ToDoDeleteResponse result = await _service.Delete(request, _context);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Complete);
        }
    }
}
