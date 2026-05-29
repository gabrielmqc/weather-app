# WeatherApp - Desafio C# (.NET 8 + Vue 3)

Aplicação full-stack permitindo registrar e consultar informações climáticas por cidade ou coordenadas geográficas, com histórico persistido em banco relacional.

---

# 🚀 Como executar a aplicação

## 🐳 Pré-requisitos
- Docker
- Docker Compose

---

## ▶️ Subir toda a aplicação

# Variáveis de ambiente
Antes de iniciar o composer crie um .env na raiz do repositorio com os campos que estão sendo exemplificados no .env.example, com uma chave de API valida do OpenWeather, e passando  seu provider desejado, as opçoes são mock, e openweather

docker-compose up --build

🌐 Acessos após subir
- Frontend: http://localhost:8081
- API (via proxy Nginx): http://localhost:8081/backend
- Swagger: http://localhost:8081/backend/swagger/index.html
- Health Check: http://localhost:8081/health

🐳 Rodar apenas a API (Docker Hub)
```bash
docker run -d \
  --name desafio-csharp \
  -p 5000:5000 \
  -e ConnectionStrings__DefaultConnection="Host=localhost;Database=WeatherDB;Username=postgres;Password=postgres" \
  -e OpenWeather__ApiKey="SUA_API_KEY" \
  gabrielmqc/desafio-csharp
```
⚙️ Como a aplicação funciona

🌍 Arquitetura geral

Frontend (Vue 3) → Nginx (Reverse Proxy) → Backend (.NET 8) → PostgreSQL

O Nginx atua como ponto único de entrada, roteando requisições para frontend e backend.

🌦️ Registro de clima por cidade
Usuário informa uma cidade
Backend consulta provedor externo (OpenWeather)
Temperatura atual é retornada
Registro é salvo no banco
Resultado é exibido no frontend


📍 Registro de clima por coordenadas
Usuário informa latitude e longitude
Backend consulta provedor externo
Temperatura atual é retornada
Registro é persistido no banco
Resultado é exibido no frontend

📊 Histórico climático
Usuário consulta por cidade ou coordenadas
Backend retorna registros dos últimos 30 dias
Dados são ordenados do mais recente para o mais antigo
Frontend exibe histórico

🧪 Health Check
/health

Verifica status da API e conexão com banco de dados.

📘 Swagger
/swagger

Documentação interativa da API.

🧱 Tecnologias utilizadas
.NET 8 (ASP.NET Core)
Vue 3 + TypeScript
PostgreSQL
Nginx (Reverse Proxy)
Docker + Docker Compose
Clean Architecture

🧠 Arquitetura

Frontend → Nginx → API → PostgreSQL

Frontend servido via Nginx
Backend isolado em container
Banco de dados em container separado
Comunicação via rede Docker interna

📌 Observações
Aplicação totalmente containerizada
Suporte a docker-compose e execução standalone via Docker Hub
Configuração via variáveis de ambiente
Migrations aplicadas automaticamente no startup
É possível alterar o WeatherProvider utilizando Flags no appsetings.json, basta trocar o UseMockProvider de false para true
