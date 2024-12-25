using Grpc.Net.Client;
using Portfolio.ToDo.GRPC.Data;
using Portfolio.ToDo.Web;
using Portfolio.ToDo.Web.Components;
using Portfolio.ToDo.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string? grpcAddress = builder.Configuration.GetValue<string>("GrpcSettings:Address");
using GrpcChannel channel = GrpcChannel.ForAddress(grpcAddress ?? throw new Exception("gRPC address is not set"));
ToDoService.ToDoServiceClient client = new(channel);

builder.Services.AddTransient<IToDoRepository, ToDoRepository>(provider => new ToDoRepository(client));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
