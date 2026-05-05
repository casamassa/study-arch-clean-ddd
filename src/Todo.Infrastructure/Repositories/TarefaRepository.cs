using System;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Infrastructure.Context;

namespace Todo.Infrastructure.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly TodoContext _context;

    public TarefaRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Tarefa tarefa)
    {
        await _context.Tarefas.AddAsync(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task<Tarefa?> ObterPorIdAsync(Guid id)
    {
        return await _context.Tarefas.FindAsync(id);
    }

    public async Task<IEnumerable<Tarefa>> ListarTodasAsync()
    {
        return await _context.Tarefas.ToListAsync();
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }
}