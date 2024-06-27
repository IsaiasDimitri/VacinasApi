## README - API de Gerenciamento de Postos de Vacinação
Esta API foi desenvolvida para gerenciar postos de vacinação e as vacinas disponíveis em cada posto. A API permite criar, editar, e remover registros de postos e vacinas, garantindo a integridade dos dados e seguindo as regras de negócio definidas.

### Funcionalidades Principais  
Postos de Vacinação:  
Criação, atualização, e remoção de postos de vacinação.
Listagem de todos os postos cadastrados.
Associação de vacinas a um posto específico.  

Vacinas:  
Criação, atualização, e remoção de registros de vacinas.
Listagem de todas as vacinas cadastradas.
Verificação de disponibilidade de vacinas em cada posto.  
Rotas Disponíveis  

### Postos de Vacinação  

GET /v1/postos
Retorna uma lista de todos os postos de vacinação cadastrados.  

GET /v1/postos/{id}
Retorna detalhes de um posto de vacinação específico pelo ID.  

POST /v1/postos
Cria um novo posto de vacinação.  Exemplo:
```
{
  "nome": "string",
  "vacinas": []
}
```

PUT /v1/postos/{id} 
Atualiza as informações de um posto de vacinação existente pelo ID.  

DELETE /v1/postos/{id}
Remove um posto de vacinação existente pelo ID.  

PUT /v1/postos/{postoId}/vacinas
Associa uma vacina existente a um posto de vacinação pelo ID do posto. Exemplo:  
```
{"3fa85f64-5717-4562-b3fc-2c963f66afa6"}
```

### Vacinas  

GET /v1/vacinas
Retorna uma lista de todas as vacinas cadastradas. 

GET /v1/vacinas/{id}
Retorna detalhes de uma vacina específica pelo ID.  

POST /v1/vacinas
Cria uma nova vacina.  Ex:
```
{
  "nome": "gripe",
  "fabricante": "pfizer",
  "lote": "abcd1234",
  "quantidade": 1000,
  "validade": "2026-06-27T08:35:33.678Z"
}
```

PUT /v1/vacinas/{id}
Atualiza as informações de uma vacina existente pelo ID.  

DELETE /v1/vacinas/{id}
Remove uma vacina existente pelo ID.  

### Tecnologias Utilizadas
C# e .NET Core para o desenvolvimento da aplicação backend.
Entity Framework Core para a camada de acesso a dados e gerenciamento do banco de dados.
Swagger para documentação e testes de API.  

### Pré-requisitos
SDK do .NET Core 8.0 instalado.
Banco de dados SQLite para armazenamento dos dados.
Ferramenta de desenvolvimento de sua escolha (Visual Studio, Visual Studio Code, etc.).  

### Configuração e Execução
Para configurar e executar a API, siga os passos abaixo:

Clone o repositório:

```
git clone https://github.com/seu-usuario/seu-repositorio.git
```

Abra o projeto em sua IDE de desenvolvimento.

Instale as dependências e execute os comandos:  

```
dotnet restore
dotnet ef migrations add Initial # primeira migração
dotnet ef database update # gera o banco de dados ou atualiza caso já exista
```
Execute a aplicação. O navegador deve abrir permitindo você visualizar e testar a API utilizando o Swagger.

Primeiro cadastre separadamente os Postos e as Vacinas que você queira. Depois, utilize a rota  /v1/postos/{postoId}/vacinas 
para atrelar uma vacina a um posto, utilizando seus Id's.

### Pontos de Melhoria
- é preciso passar uma lista de vacinas vazia na criação de um novo posto
- é preciso criar tanto o posto como a vacina separados, e depois uni-los via rota /v1/postos/{postoId}/vacinas
- só é possível adicionar uma vacina por vez ao posto
