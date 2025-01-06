var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Portfolio_ToDo_GRPC>("portfolio-todo-grpc");

builder.AddProject<Projects.Portfolio_ToDo_Web>("portfolio-todo-web");

builder.AddProject<Projects.Portfolio_ToDo_Auth>("portfolio-todo-auth");

builder.Build().Run();
