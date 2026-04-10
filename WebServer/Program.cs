var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path}");
    await next(context);
    Console.WriteLine($"[LOG] Ответ отправлен: {context.Response.StatusCode}");
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Powered-By", "ASP.NET Core Lab27");
    await next(context);
});

app.Use(async (context, next) =>
{
    var key = context.Request.Query["key"];
    if (key != "secret")
    {
        context.Response.StatusCode = 401;
        return;
    }
    await next(context);
});
app.MapGet("/", () => "Добро пожаловать на сервер!");
app.MapGet("/about", () => "Это мой первый ASP.NET Core сервер");
app.MapGet("/time", () => $"Время на сервере: {DateTime.Now}");
app.MapGet("/hello/{name}", (string name) => $"Привет, {name}");
app.MapGet("/sum/{a}/{b}", (int a, int b) => $" Сумма: {a + b}");

app.MapGet("/student", () => new
{
    Name = "Иван Иванов",
    Group = "ИСП-231",
    Year = 3,
    IsActive = true
});

app.MapGet("/subjects", () => new[]
{
    "РПМ",
    "РМП",
    "ИСРПО",
    "СП",
});

app.MapGet("/product/{id}", (int id) => new Product(Id: id,
Name: $"Товар #{id}",
Price: id * 99.99m,
InStock: id % 2 == 0
));
app.Run();

record Product(int Id, string Name, decimal Price, bool InStock);