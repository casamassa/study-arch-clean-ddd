using System;
using Todo.Domain.Enums;

namespace Todo.Application.DTOs;

public record CriarTarefaRequest(string Titulo, string Descricao, Prioridade Prioridade);