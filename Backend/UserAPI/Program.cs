using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using UserAPI.Context;

var builder = WebApplication.CreateBuilder(args);
var urlCors = builder.Configuration.GetSection("AppSettings:Cors").Value!;

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<UserDataContext>(
       options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins", builder =>
    {
        builder.WithOrigins(urlCors).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
