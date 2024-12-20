using Microsoft.EntityFrameworkCore;
using Moq;
using MockQueryable.Moq;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.UnitTest
{
    public class ToDoRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly ToDoRepository _repository;

        public ToDoRepositoryTests()
        {
            DbContextOptions<ApplicationDbContext> options = new();
            _mockContext = new Mock<ApplicationDbContext>(options);
            _repository = new ToDoRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetItemByIdAsync_ShouldReturnItem_WhenItemExists()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            ToDoItem toDoItem = new() { Id = id };
            _mockContext.Setup(c => c.ToDoItems.FindAsync(id)).ReturnsAsync(toDoItem);

            // Act
            IToDoItem result = await _repository.GetItemByIdAsync(id);

            // Assert
            Assert.Equal(toDoItem, result);
        }

        [Fact]
        public async Task GetItemByIdAsync_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            _mockContext.Setup(c => c.ToDoItems.FindAsync(id)).ReturnsAsync((ToDoItem?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetItemByIdAsync(id));
        }

        [Fact]
        public async Task GetItemListAsync_ShouldReturnAllItems()
        {
            // Arrange
            IQueryable<ToDoItem> toDoItems = new List<ToDoItem>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            }
            .AsQueryable();

            Mock<DbSet<ToDoItem>> mockDbSet = toDoItems.BuildMockDbSet();

            _mockContext.Setup(c => c.ToDoItems).Returns(mockDbSet.Object);

            // Act
            IQueryable<IToDoItem> result = await _repository.GetItemListAsync();

            // Assert
            Assert.Equal(toDoItems, result);
        }

        [Fact]
        public async Task SaveItemAsync_ShouldAddNewItem_WhenItemIsNew()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = Guid.Empty };
            _mockContext.Setup(c => c.ToDoItems.AddAsync(toDoItem, default))
                .ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ToDoItem>?)null);

            // Act
            IToDoItem result = await _repository.SaveItemAsync(toDoItem);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            _mockContext.Verify(c => c.ToDoItems.AddAsync(toDoItem, default), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SaveItemAsync_ShouldUpdateExistingItem_WhenItemExists()
        {
            // Arrange
            ToDoItem toDoItem = new() { Id = Guid.NewGuid() };
            _mockContext.Setup(c => c.ToDoItems.Update(toDoItem));

            // Act
            var result = await _repository.SaveItemAsync(toDoItem);

            // Assert
            Assert.Equal(toDoItem.Id, result.Id);
            _mockContext.Verify(c => c.ToDoItems.Update(toDoItem), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteItemByIdAsync_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            ToDoItem toDoItem = new() { Id = id };
            _mockContext.Setup(c => c.ToDoItems.FindAsync(id)).ReturnsAsync(toDoItem);
            _mockContext.Setup(c => c.ToDoItems.Remove(toDoItem));

            // Act
            bool result = await _repository.DeleteItemByIdAsync(id);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.ToDoItems.Remove(toDoItem), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteItemByIdAsync_ShouldThrowException_WhenItemDoesNotExist()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            _mockContext.Setup(c => c.ToDoItems.FindAsync(id)).ReturnsAsync((ToDoItem?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteItemByIdAsync(id));
        }
    }
}
