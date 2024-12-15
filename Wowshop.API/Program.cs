var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    Console.WriteLine($"Hello World!. Time: {DateTime.Now}");
    
    return "Hello World!";
});
app.MapGet("goodbye", () => "Goodbye");

app.MapGet("/health", (IConfiguration configuration) => new
{
    ImageTag = configuration["CurrentImageTag"],
});

app.Run();