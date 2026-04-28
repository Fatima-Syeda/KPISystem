# 📌 KPI Event Processing System

## 📖 Overview
This project is a **.NET-based event-driven system** that processes KPI data using a microservices architecture.
Built to simulate a real-world KPI processing pipeline using event-driven architecture.

It consists of:
- **API Service** → receives events via HTTP
- **RabbitMQ** → message broker for decoupling services
- **Worker Service** → consumes events and processes them
- **PostgreSQL** → stores processed event data

---

## 🏗️ Architecture
Client → API → RabbitMQ → Worker → PostgreSQL → Client

- The API publishes events to a queue
- The Worker listens to the queue and saves events to the database
- This design improves scalability and reliability

---

## 🚀 Features
- REST API with Swagger UI
- Asynchronous message processing with RabbitMQ
- Background worker using .NET Hosted Services
- PostgreSQL integration using Entity Framework Core
- Fully containerized with Docker & Docker Compose

---

## 🛠️ Tech Stack
- .NET (ASP.NET Core Minimal API)
- C#
- RabbitMQ
- PostgreSQL
- Entity Framework Core
- Docker & Docker Compose

---

## ▶️ Getting Started

### 1. Clone the repo
```bash
git clone <your-repo-url>
cd kpi-system
```
### 2. Run with Docker
```bash
docker compose up --build
```
## 🌐 Services & Ports
| Service     | URL                                                            |
| ----------- | -------------------------------------------------------------- |
| API         | [http://localhost:5000](http://localhost:5000)                 |
| Swagger     | [http://localhost:5000/swagger](http://localhost:5000/swagger) |
| RabbitMQ UI | [http://localhost:15672](http://localhost:15672)               |
| PostgreSQL  | localhost:5432                                                 |


## 🧪 Testing the System
### 1. Send an event

Use Swagger or curl:
```bash
curl -X POST http://localhost:5000/events \
-H "Content-Type: application/json" \
-d '{
  "type": "temperature",
  "value": 25.5,
  "timestamp": "2026-04-27T00:00:00Z"
}'
```
### 2. Retrieve events
```bash
curl http://localhost:5000/events
```

## 📂 Project Structure
- **/Api**       → ASP.NET Core API
- **/Worker**     → Background service (RabbitMQ consumer)
- **/docker-compose.yml**

## ⚙️ How It Works
1. API receives an event
2. Event is published to RabbitMQ
3. Worker consumes the message
4. Worker stores the event in PostgreSQL
5. API retrieves stored events via GET endpoint

