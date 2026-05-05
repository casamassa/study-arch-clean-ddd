# Estudo simples de Clean Architecture + DDD + DI/IoC

Um Sistema de Gestão de Tarefas (Todo List) com Prioridades.

## O Cenário: "Super ToDo"

Não é apenas um "check/uncheck". Contém uma regra de negócio:

1.  Uma tarefa tem Título, Descrição e Prioridade (Alta, Média, Baixa).
2.  Regra de Negócio (DDD): Uma tarefa com prioridade "Alta" não pode ser criada sem uma "Descrição".
3.  Ação (Application): Ao concluir uma tarefa, o sistema deve registrar a data de conclusão e, se for "Alta", enviar um log (simulando um e-mail).

## O Esqueleto do Clean Architecture

Obs. Esse mesmo projeto foi refatorado em outro repositório (https://github.com/casamassa/study-vsa) na arquitetura VSA ao invés de Clean Arch, lae a pena comparar e decidir qual adequa mais a necessidade.

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

## Como executar:

1. No terminal, entre na pasta da API:

```bash
cd Todo.API
```

2. Execute:

```bash
dotnet run
```

3. Abra o navegador no endereço do Swagger (geralmente http://localhost:5xxx/swagger).
4. Tente criar uma tarefa com Prioridade 2 (Alta) e sem descrição. Veja o erro acontecer!

## Como testar (testes unitários):

1. No terminal a partir da pasta raiz e execute:

```bash
dotnet test
```

## Notas:

### Camada de Domínio

- Coração do sistema. No Clean Architecture e no DDD, sempre começe pelo Domínio, pois ele não depende de ninguém.
- Encapsulamento: Você não consegue criar uma Tarefa inválida (sem título ou alta sem descrição). A regra "mora" na entidade.
- Independência: Este projeto não sabe que o Entity Framework ou a WebAPI existem.

### Camada de Aplicação

- É aqui que o "maestro" reside: ele recebe os dados da API, usa a lógica do Domínio e manda a Infraestrutura salvar.
- TarefaService.cs: Este serviço vai orquestrar a criação. Observe que ele depende da Interface (ITarefaRepository), e não de uma classe concreta. Isso é Inversão de Dependência.
- Orquestração: O serviço não contém as regras (como "precisa de descrição"), ele apenas chama Tarefa.Criar() onde a regra mora.
- Desacoplamento: Se amanhã trocarmos o Entity Framework pelo Dapper ou por um arquivo TXT, este código da Application não muda nada, porque ele só conhece a ITarefaRepository.

### Camada de Infraestrutura

- A camada que realmente "suja as mãos" com tecnologia. No Clean Architecture, a Infrastructure é onde implementamos os detalhes técnicos que o Domínio e a Application apenas mencionaram como interfaces.
- Por que separamos isso em um projeto diferente? Se amanhã quiser trocar o EF Core pelo Dapper ou por uma chamada de API externa, cria-se um novo projeto de Infrastructure ou altera apenas este arquivo. O resto do sistema (Domínio e Application) sequer perceberá a mudança, pois eles continuam esperando alguém que assine o contrato ITarefaRepository.

### Camada de API

- Finalizando o ciclo da Clean Architecture com umaa Web API, que servirá como a "porta de entrada" (UI) do sistema. Nela, fiz a Injeção de Dependência (DI) para conectar todas as peças que criadas anteriormente.
- Program.cs: Aqui é onde aplica-se o conceito de Inversão de Controle. Note como foi registrado os diferentes tempos de vida (Scoped).
- Controller: O Controller deve ser "magro". Ele não conhece regras de negócio, apenas recebe a requisição e repassa para o TarefaService na camada de Application.

O que é construido aqui?

1. Independência de UI: Se você quiser voltar para o WinForms, basta referenciar os projetos Application e Infrastructure nele e usar a mesma lógica.
2. Testabilidade: Você pode testar o TarefaService sem precisar de um banco de dados real ou de uma API rodando.3. Domínio Rico: Se você tentar criar uma tarefa Alta sem Descrição, a sua API retornará o erro que definimos lá na Entidade Tarefa, protegendo o negócio.

### Testes Unitários:

1. O foco principal dos testes unitários na Clean Arch deve ser o Domínio (regras de negócio) e a Application (orquestração). Como a Application depende de interfaces, usei Mocks para simular o banco de dados.

2. Domínio: Como a classe Tarefa é uma "Pure Old C# Class", você não precisa de banco ou API para testar a lógica.

3. Application: Como o TarefaService recebe uma Interface no construtor (DI), o Moq consegue criar um "dublê" do repositório em um segundo.
