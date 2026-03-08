using FreelanceManager.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddDbContext<ApplicationDbContext>(options=>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

app.UseHttpsRedirection();


app.MapControllers();
app.Run();
