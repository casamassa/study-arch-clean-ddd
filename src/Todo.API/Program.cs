using Microsoft.EntityFrameworkCore;
using Todo.Application.Services;
using Todo.Domain.Interfaces;
using Todo.Infrastructure.Context;
using Todo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura o Banco de Dados em Memória
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoDb"));

// 2. REGISTRO DE DI (A mágica acontece aqui)
// "Quando pedirem ITarefaRepository, entregue TarefaRepository"
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

// "Quando pedirem o serviço de aplicação, entregue TarefaService"
builder.Services.AddScoped<TarefaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

