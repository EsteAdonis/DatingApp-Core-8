using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();

// DJ: The framework is responsible to create a new instance the DataContext Class and then disposing of that class that we inject after we finished using it.


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();
// Configure the HTTP request pipeline.


// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// using (var scope = app.Services.CreateScope())
// {
//   var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//   context.Database.Migrate();
// }


app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(
  x => x.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200", "http://localhost:4200")
  );
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.SeedUsers(context);
} catch(Exception ex)
{
  var logger = services.GetRequiredService<ILogger<Program>>();
  logger.LogError(ex, "An error ocurred during migration");
}

app.Run();