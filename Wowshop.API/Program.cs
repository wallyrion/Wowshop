var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/hey", () => "hey");
app.MapGet("/whois", () => "me");

app.Run();