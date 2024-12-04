var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Portfolio_ToDo_GRPC>("portfolio-todo-grpc");

builder.Build().Run();
