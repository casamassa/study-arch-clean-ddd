using System;
using Moq;
using Todo.Application.DTOs;
using Todo.Application.Services;
using Todo.Domain.Entities;
using Todo.Domain.Enums;
using Todo.Domain.Interfaces;

namespace Todo.UnitTests.Application;

public class TarefaServiceTests
{
    [Fact]
    public async Task CriarTarefa_DadosValidos_DeveChamarAdicionarNoRepositorio()
    {
        // Arrange
        var repoMock = new Mock<ITarefaRepository>();
        var service = new TarefaService(repoMock.Object);
        var request = new CriarTarefaRequest("Lavar louça", "Urgente", Prioridade.Media);

        // Act
        var result = await service.CriarTarefaAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Once);
    }
}
