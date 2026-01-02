using Serilog;
using Serilog.Events;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//koya commented
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MyApp")
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.File(
        formatter: new Serilog.Formatting.Json.JsonFormatter(),
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

// Use Serilog for logging
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorchingg"
};

app.MapGet("api/v2", (ILogger<Program> logger) =>
{
    logger.LogInformation("Home Page Warning");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1>Weather Forecast Service Home Page </h1>
    <p>The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("api/v2/continents", (ILogger<Program> logger) =>
{
    
    logger.LogInformation("continents page");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Continents Weather Forecast  </h1>
    <p>The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");
    
app.MapGet("api/v2/countries", (ILogger<Program> logger) =>
{
    logger.LogInformation("countries page");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Countries Weatherforecast at {now.ToUniversalTime().ToString()}  </h1>
    <p>The time now in UTC is  </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("api/v2/error", (ILogger<Program> logger) =>
{
    logger.LogError("the page warning");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1>  Log at {now.ToUniversalTime().ToString()}  </h1>
    <p>The time now in UTC is  </p>
    </body>
    </html>
    ", "text/html");
});

app.MapGet("api/v2/test-exception", (ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Attempting a risky operation...");

        // Simulate an actual exception (DivideByZero)
        int numerator = 10;
        int denominator = 0;
        int result = numerator / denominator; 

        return Results.Ok(new { Result = result });
    }
    catch (Exception ex)
    {
        // Log the full Exception object (includes Stack Trace)
        logger.LogError(ex, "An unhandled exception occurred during the test-exception request.");

        var now = DateTime.UtcNow;
        return Results.Content(@$"
            <html>
            <head><link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'></head>
            <body>
                <h1 style='color: #d9534f;'>500 - Server Error</h1>
                <p><strong>Logged at:</strong> {now:u}</p>
                <p><strong>Error Message:</strong> {ex.Message}</p>
                <hr>
                <p>The error has been recorded in the application logs.</p>
            </body>
            </html>", "text/html");
    }
});

app.MapGet("api/v2/null-error", (ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Processing request that will trigger a NullReferenceException.");

        // 1. Create a null object
        List<string>? myData = null;

        // 2. Attempt to access a property on it (this triggers the exception)
        int count = myData.Count; 

        return Results.Ok(new { Total = count });
    }
    catch (NullReferenceException ex)
    {
        // Log specifically that a null was encountered
        logger.LogError(ex, ex.Message);

        var now = DateTime.UtcNow;
        return Results.Content(@$"
            <html>
            <head><link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'></head>
            <body>
                <h1 style='color: #d9534f;'>Null Reference Detected</h1>
                <p><strong>Time (UTC):</strong> {now:u}</p>
                <p><strong>Technical Detail:</strong> {ex.Message}</p>
                <p>Check your console/logs to see the <code>StackTrace</code>.</p>
                <a href='/'>Return Home</a>
            </body>
            </html>", "text/html");
    }
});
        
app.MapGet("api/v2/warning", (ILogger<Program> logger) =>
{
    logger.LogWarning("the page");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1>  Log at {now.ToUniversalTime().ToString()}  </h1>
    <p>The time now in UTC is  </p>
    </body>
    </html>
    ", "text/html");
});


app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGet("api/v2/random", (ILogger<Program> logger) =>
{
    logger.LogInformation("random page");
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    var random = new Random();

    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Random Forecast  </h1>
    <pre>  {forecast[random.Next(1, summaries.Length)]} <pre>                    
    </body>
    </html>
    ", "text/html");
})
.WithName("GetWeatherForecast");

app.MapGet("api/v2/read-file", (string? key, string? type, ILogger<Program> logger) =>
{
    // Handle missing query params
    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(type))
    {
        logger.LogInformation("No query params passed. Showing default page.");
        return Results.Text(BuildDefaultPage(), "text/html");
    }

    var filePath = Environment.GetEnvironmentVariable(key);

    if (string.IsNullOrEmpty(filePath))
    {
        logger.LogWarning("No environment variable found for key '{Key}'", key);
        return Results.BadRequest($"No file path found for environment variable '{key}'");
    }

    if (!File.Exists(filePath))
    {
        logger.LogWarning("File not found at path: {FilePath}", filePath);
        return Results.BadRequest($"No file found at path '{filePath}'");
    }

    logger.LogInformation("Loading file from path: {FilePath}", filePath);

    try
    {
        return type.ToLower() switch
        {
            "json" => Results.Text(BuildJsonPage(filePath, key), "text/html"),
            "image" => Results.Text(BuildImagePage(filePath, key), "text/html"),
            "pdf" => Results.Text(BuildPdfPage(filePath, key), "text/html"),
            "csv" =>  Results.Text(BuildCSVPage(filePath, key), "text/html"),
            _ => Results.BadRequest("Unsupported file type. Use 'json', 'image', csv, or 'pdf'.")
        };
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing file {FilePath}", filePath);
        return Results.BadRequest("Failed to process the file.");
    }
})
.WithName("GetFile");


// ---------------- Helpers ---------------- //

static string BuildDefaultPage() => @"
<html>
<head>
  <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
  <h1> File Viewer </h1>
  <p>Use query parameters <code>?key=&lt;ENV_KEY&gt;&type=json|image|pdf|csv</code></p>
  <p>Example: <code>/api/v2/file?key=MY_JSON_FILE&type=json</code></p>
</body>
</html>";

static string BuildJsonPage(string filePath, string key)
{
    var jsonContent = File.ReadAllText(filePath);
    using var doc = JsonDocument.Parse(jsonContent);
    var prettyJson = JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });

    return $@"
<html>
<head>
  <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
  <h1> JSON File from {key} </h1>
  <pre>{System.Net.WebUtility.HtmlEncode(prettyJson)}</pre>
</body>
</html>";
}

static string BuildImagePage(string filePath, string key)
{
    var bytes = File.ReadAllBytes(filePath);
    var base64 = Convert.ToBase64String(bytes);

    return $@"
<html>
<head>
  <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
  <h1> Image from {key} </h1>
  <img src='data:image/png;base64,{base64}' alt='Image from {filePath}' style='max-width:600px;' />
</body>
</html>";
}

static string BuildPdfPage(string filePath, string key)
{
    var bytes = File.ReadAllBytes(filePath);
    var base64 = Convert.ToBase64String(bytes);

    return $@"
<html>
<head>
  <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
  <h1> PDF from {key} </h1>
  <iframe src='data:application/pdf;base64,{base64}' width='100%' height='600px'></iframe>
</body>
</html>";
}

static string BuildCSVPage(string filePath, string key)
{
    var bytes = File.ReadAllBytes(filePath);
    var csvText = System.Text.Encoding.UTF8.GetString(bytes);

    return $@"
<html>
<head>
  <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
  <h1>CSV from {key}</h1>
  <pre style='white-space: pre-wrap; word-wrap: break-word;'>
{System.Net.WebUtility.HtmlEncode(csvText)}
  </pre>
</body>
</html>";
}

app.Run("http://0.0.0.0:8080");

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
