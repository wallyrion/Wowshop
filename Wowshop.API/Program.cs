var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("goodbye", () => "Goodbye");
app.MapGet("goodbye2", () => "Goodbye");

app.Run();