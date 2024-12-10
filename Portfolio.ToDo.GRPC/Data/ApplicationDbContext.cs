using Microsoft.EntityFrameworkCore;

namespace Portfolio.ToDo.GRPC.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
