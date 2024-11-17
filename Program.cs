using Microsoft.EntityFrameworkCore;
using FirstReadAPI.Models;
using FirstReadAPI.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FirstRead API", Version = "v1" });
});

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allow requests from React app (running on localhost:3000)
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader()   // Allow any headers
              .AllowAnyMethod();  // Allow any HTTP method (GET, POST, etc.)
    });
});

var app = builder.Build();

// Enable Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FirstRead API v1");
    });
}

// Use CORS policy for the app
app.UseCors("AllowFrontend");  // Apply the CORS policy defined above

app.UseHttpsRedirection();

// Map API routes
app.MapGet("/api/books", async (ApplicationDbContext db) =>
{
    return await db.Books.ToListAsync();
})
.WithName("GetBooks");

app.MapPost("/api/books", async (ApplicationDbContext db, Book book) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/api/books/{book.BookId}", book);
})
.WithName("AddBook");

app.Run();
