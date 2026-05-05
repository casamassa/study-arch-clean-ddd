# Estudo simples de CleanArch + DDD + DI/IoC

Um Sistema de Gestão de Tarefas (Todo List) com Prioridades.

## O Cenário: "Super ToDo"

Não é apenas um "check/uncheck". Contém uma regra de negócio:

1.  Uma tarefa tem Título, Descrição e Prioridade (Alta, Média, Baixa).
2.  Regra de Negócio (DDD): Uma tarefa com prioridade "Alta" não pode ser criada sem uma "Descrição".
3.  Ação (Application): Ao concluir uma tarefa, o sistema deve registrar a data de conclusão e, se for "Alta", enviar um log (simulando um e-mail).

## Passo 1: O Esqueleto da Clean Architecture

São 4 projetos que você dentro de uma Solution:

1.  Todo.Domain (Class Library)

- O que tem: A classe Tarefa (Entidade), o Enum Prioridade e as Interfaces dos Repositórios (contratos).
- Dependências: Nenhuma. É o centro de tudo.

2.  Todo.Application (Class Library)

- O que tem: Os DTOs (objetos que viajam entre UI e API) e o TarefaService (onde está a orquestração).
- Dependências: Depende apenas do Todo.Domain.

3.  Todo.Infrastructure (Class Library)

- O que tem: O contexto do Banco de Dados (EF Core) e a implementação real do Repositório que salva no banco.
- Dependências: Depende de Todo.Domain (para implementar as interfaces).

4.  Todo.API (ASP.NET Core Web API)

- O que tem: Os Controllers e o Program.cs onde a "mágica" da Injeção de Dependência acontece.
- Dependências: Depende de Todo.Application (para chamar os serviços) e Todo.Infrastructure (apenas para registrar a DI).

## Por que esse projeto é bom para aprender?

- DI/IoC: Ver como a API entrega o Repositório para o Serviço sem que o Serviço saiba que o Entity Framework existe.
- DDD: Colocar a regra da "Prioridade Alta" dentro da classe Tarefa, e não no banco ou na tela.
- Clean Arch: Se no futuro decidir trocar a API por um Console App ou WinForms, o projeto de Application e Domain continuará intacto.
