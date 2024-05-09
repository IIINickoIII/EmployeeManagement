using EmployeeManagement.Api.Extensions;
using EmployeeManagement.Api.Options;
using EmployeeManagement.Api.Repositories;
using EmployeeManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseDatabaseInitializer();

app.Run();
