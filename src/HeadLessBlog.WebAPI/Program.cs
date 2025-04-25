using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using HeadLessBlog.Application.Users.Commands.CreateUser;
using HeadLessBlog.Infrastructure;
using HeadLessBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
// ─── MediatR v12 ────────────────────────────────────────────────────────────────
// REGISTRA TODOS LOS HANDLERS que hayas definido en tu capa Application
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly);
});


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HeadLessBlogDbContext>();
    dbContext.Database.Migrate();
}


app.Run();
