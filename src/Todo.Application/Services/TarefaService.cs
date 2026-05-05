using System;
using Todo.Application.DTOs;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;

namespace Todo.Application.Services;

public class TarefaService
{
    private readonly ITarefaRepository _repository;

    // O serviço recebe o repositório por Injeção de Dependência
    public TarefaService(ITarefaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CriarTarefaAsync(CriarTarefaRequest request)
    {
        // 1. Usa a regra de negócio do Domínio para criar o objeto
        var tarefa = Tarefa.Criar(request.Titulo, request.Descricao, request.Prioridade);

        // 2. Manda o repositório persistir (sem saber se é SQL, NoSQL ou memória)
        await _repository.AdicionarAsync(tarefa);

        return tarefa.Id;
    }

    public async Task ConcluirTarefaAsync(Guid id)
    {
        var tarefa = await _repository.ObterPorIdAsync(id);
        if (tarefa == null) throw new Exception("Tarefa não encontrada!");

        tarefa.Concluir();
        await _repository.AtualizarAsync(tarefa);
    }
}
