var builder = WebApplication.CreateBuilder(args);

//koya commented
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
// v2 wetherforcast  
app.MapGet("api/v2", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1>Hello from Dotnet on Salus during Demo V2 </h1>
    <p> Weatherforecast Version Two </p>
    <p>The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("api/v2/weatherforecast/france", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1>The weather is {Random.Shared.Next(-20, 55)} Degree Celcius in france.  </h1>
    <p>Version Two. The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

    
app.MapGet("api/v2/weatherforecast/germany", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
      <h1>The weather is {Random.Shared.Next(-20, 55)} Degree Celcius in Germany.</h1>
      <p>Version Two. The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");
   
app.MapGet("api/v2/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");



//test route
app.MapGet("test", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Welcome to Salus Test Service </h1>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("test/football-clubs", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Names  </h1>
    <p> Man City </p>
    <p> Arsenal <p>
    <p> Others </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

    
app.MapGet("api/v2/weatherforecast/germany", () =>
{
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
      <h1>The weather is {Random.Shared.Next(-20, 55)} Degree Celcius in Germany.</h1>
      <p>Version Two. The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");


app.Run("http://0.0.0.0:8080");

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
