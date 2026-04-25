# 📦 InventoryManager API

API RESTful para gestão profissional de inventários de TI, desenvolvida com **.NET 10** focada em alta disponibilidade, segurança e conteinerização.

---

## 📖 Sobre o Projeto e Motivação

O **InventoryManager** nasceu da necessidade de uma ferramenta leve para gestão pessoal de ativos de TI, porém robusta, que centralizasse o controle de inventário. O objetivo deste projeto é demonstrar a aplicação de padrões de arquitetura corporativa (Clean Code, DTOs, Segregação de Responsabilidades) em um ecossistema conteinerizado, garantindo que o rastreio de ativos seja seguro, auditável e performático.

---

## 📋 Índice

- [Tecnologias](#-tecnologias)
- [Arquitetura](#-arquitetura)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração do Ambiente](#-configuração-do-ambiente)
- [Executando a Aplicação](#-executando-a-aplicação)
- [Endpoints da API](#-endpoints-da-api)
- [Regras de Negócio](#-regras-de-negócio)
- [Segurança](#-segurança)

---

## 🚀 Tecnologias

- **.NET 10** (Minimal APIs)
- **PostgreSQL 15** (Alpine)
- **Entity Framework Core 10**
- **JWT Bearer Authentication**
- **FluentValidation**
- **Swagger/OpenAPI**
- **Docker** & **Docker Compose**

---

## 🏗️ Arquitetura

O projeto utiliza uma estrutura moderna e enxuta, organizada para garantir a separação de responsabilidades e escalabilidade:

```
deLuca.InventoryManager/
├── deLuca.InventoryManager.Api/   # Camada de Apresentação e Regras
│   ├── Context/                   # Persistência e configuração do EF Core
│   ├── DTOs/                      # Objetos de Transferência de Dados para entrada e saída
│   ├── Models/                    # Entidades de Domínio e modelos de banco de dados
│   ├── Validations/               # Regras de validação com FluentValidation
│   └── Program.cs                 # Configuração de serviços e definição de endpoints
├── .env                           # Ficheiro local para gestão de segredos e variáveis
└── docker-compose.yml             # Orquestração completa da infraestrutura de containers
```

**Diferenciais implementados:**
- ✅ **Soft Delete**: Implementação de exclusão lógica para preservação de integridade histórica.
- ✅ **Pagination**: Listagem otimizada de recursos para suportar grandes volumes de dados sem perda de performance.
- ✅ **Repository/DTO Pattern**: Blindagem das entidades de domínio contra exposição direta na API.
- ✅ **Connection Resiliency**: Configuração de padrões de tentativa (Retry Pattern) para conexão robusta com o banco de dados em ambiente de containers.

---

## 📦 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

---

## ⚙️ Configuração do Ambiente

### 1️⃣ Clone o repositório

```bash
git clone [https://github.com/seu-usuario/deLuca.InventoryManager.git](https://github.com/seu-usuario/deLuca.InventoryManager.git)
cd deLuca.InventoryManager
```

### 2️⃣ Configurar variáveis de ambiente

O projeto utiliza um arquivo `.env` para proteger dados sensíveis. Utilize o arquivo de exemplo como base:

```bash
# Copie o exemplo para criar seu arquivo local
cp .env.example .env
```

Edite o arquivo `.env` e defina suas credenciais de desenvolvimento:
```env
DB_PASSWORD=SUA_PASSWORD
ADMIN_EMAIL=SEU_EMAIL
ADMIN_PASSWORD=SUA_PASSWORD
JWT_KEY=SuaChaveSecretaDePeloMenos32Caracteres!
```

### 3️⃣ Subir a infraestrutura com Docker

A orquestração via Docker Compose garante que o banco de dados e a API subam em sincronia:

```bash
docker-compose up -d --build
```

---

## 🎯 Executando a Aplicação

A API estará pronta para uso assim que as migrações automáticas do banco de dados forem aplicadas no startup:
- 🌐 **Swagger UI**: `http://localhost:8080/swagger`
- 🩺 **Resiliência**: A API aguardará a prontidão do banco de dados antes de falhar a inicialização.

---

## 📡 Endpoints da API

### **Autenticação**
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/auth/login` | Autentica o utilizador e gera o Token JWT |

### **Categorias**
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET`  | `/api/categorias` | Lista as categorias disponíveis com suporte a paginação |
| `POST` | `/api/categorias` | Cria uma nova categoria (Requer autenticação) |
| `DELETE` | `/api/categorias/{id}` | Realiza o soft delete de uma categoria específica |

### **Subcategorias**
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET`  | `/api/subcategorias` | Lista subcategorias vinculadas às categorias pai |
| `POST` | `/api/subcategorias` | Adiciona uma nova subcategoria (Requer autenticação) |
| `PUT`  | `/api/subcategorias/{id}` | Atualiza os dados de uma subcategoria existente |
| `DELETE` | `/api/subcategorias/{id}` | Realiza o soft delete de uma subcategoria |

### **Itens (Inventário)**
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET`  | `/api/itens` | Busca completa de ativos com suporte a filtros e paginação |
| `GET`  | `/api/itens/{id}` | Obtém os detalhes técnicos de um item específico |
| `POST` | `/api/itens` | Regista um novo hardware ou periférico no stock |
| `PUT`  | `/api/itens/{id}` | Atualiza informações (Status, Localização, Atribuição) |
| `DELETE` | `/api/itens/{id}` | Remove logicamente um item do inventário ativo |

> **Nota:** Todos os endpoints de escrita (`POST`, `PUT`, `DELETE`) exigem o Header `Authorization: Bearer <token>` obtido através do login.

---

## 💰 Regras de Negócio

### 📝 Validações de Inventário
A API aplica regras de negócio customizadas para manter a qualidade dos dados:

- **Padronização de Nomenclatura**: As categorias devem respeitar prefixos específicos conforme a unidade de negócio definida nas regras de validação.
- **Integridade Referencial**: O sistema impede a exclusão de categorias que possuam produtos ativos vinculados, garantindo a consistência do inventário.
- **Controle de Acesso**: Operações que alteram o estado do banco (escrita, atualização ou deleção) são protegidas e exigem um token JWT válido.

---

## 🔐 Segurança

### **Autenticação JWT**
O acesso é controlado através de **JSON Web Tokens**:
1. Realize o login no endpoint de autenticação para receber o seu token.
2. Inclua o token no header de todas as requisições subsequentes:
   `Authorization: Bearer <seu_token>`.

### **Proteção de Segredos**
- ✅ **Clean Code**: Nenhuma senha ou e-mail de administrador está chumbado (hardcoded) no código C#.
- ✅ **Configuração Dinâmica**: As credenciais são injetadas em tempo de execução via variáveis de ambiente pelo Docker.
- ✅ **Git Protection**: O arquivo `.env` está explicitamente ignorado no `.gitignore` para evitar vazamentos acidentais.

---

## 🛠️ Comandos Úteis

### Docker Compose
Utilize estes comandos para gerenciar sua infraestrutura local:

```bash
# Reiniciar apenas a aplicação API após mudanças no código
docker-compose restart api

# Visualizar logs em tempo real para depuração
docker-compose logs -f

# Derrubar toda a infraestrutura e remover volumes
docker-compose down -v
```

---

## 📄 Licença

Este projeto está licenciado sob a Licença MIT.

---

## 👨‍💻 Autor

**Desenvolvido por Dalto de Luca**
