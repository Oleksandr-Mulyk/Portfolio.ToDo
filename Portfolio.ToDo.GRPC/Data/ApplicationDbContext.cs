using Microsoft.EntityFrameworkCore;
using Portfolio.ToDo.ToDoList;

namespace Portfolio.ToDo.GRPC.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public virtual required DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
