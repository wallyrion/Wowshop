var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("hey", () => "hey");
app.MapGet("who", () => "me");
app.MapGet("is", () => "there");

app.Run();