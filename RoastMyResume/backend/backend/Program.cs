using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Roast My Resume API",
        Version = "v1",
        Description = "Uploads resumes and roasts them using LLMs from Groq"
    });

    // Ensure correct rendering for file uploads
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // Enable annotation support
    c.EnableAnnotations();
});

builder.WebHost.UseUrls("http://0.0.0.0:80");


var app = builder.Build();

app.UseCors("AllowAngularDevClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // This is key: serve Swagger UI with explicit config
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Roast My Resume API v1");
        c.RoutePrefix = "swagger"; // Makes /swagger the default Swagger UI route
        c.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
        c.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
