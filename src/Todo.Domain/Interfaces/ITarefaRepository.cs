using System;
using Todo.Domain.Entities;

namespace Todo.Domain.Interfaces;

public interface ITarefaRepository
{
    Task AdicionarAsync(Tarefa tarefa);
    Task<Tarefa?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Tarefa>> ListarTodasAsync();
    Task AtualizarAsync(Tarefa tarefa);
}