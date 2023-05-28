using TimeService.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
