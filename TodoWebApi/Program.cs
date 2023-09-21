using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Repositories;
using TodoWebApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                          });
});

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    var context = services.GetRequiredService<TodoContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "Something wrong happened during migration");

}

app.Run();