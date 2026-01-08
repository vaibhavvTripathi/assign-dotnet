# MyApi - ASP.NET Core Web API

## 🛠️ Setup Instructions

### 1. Clone & Restore
```bash
git clone <repository-url>
cd MyApi
dotnet restore
```

### 2. Run the Application

```bash
docker-compose up -d --build
```
API documentation available at: `http://localhost:5050/swagger/index.html`

## 📋 API Endpoints

### Authentication (`http://localhost:5050/api/auth`)
- `POST http://localhost:5050/api/auth/register` - Register a new user
- `POST http://localhost:5050/api/auth/login` - Login user

### Tasks (`http://localhost:5050/api/tasks`) - *Requires Authentication*
- `GET http://localhost:5050/api/tasks` - Get all tasks (optional query params: `status`, `priority`, `assignedToId`)
- `GET http://localhost:5050/api/tasks/{id}` - Get task by ID
- `POST http://localhost:5050/api/tasks` - Create new task
- `PUT http://localhost:5050/api/tasks/{id}` - Update task
- `PATCH http://localhost:5050/api/tasks/{id}/status` - Update task status
- `DELETE http://localhost:5050/api/tasks/{id}` - Delete task

### Users (`/api/users`) - *Requires Authentication*
- `GET http://localhost:5050/api/users` - Get all users (optional query params: `page`, `pageSize`)
- `GET http://localhost:5050/api/users/{id}` - Get user by ID
- `POST http://localhost:5050/api/users` - Create new user
- `DELETE http://localhost:5050/api/users/{id}` - Delete user

## 📮 Postman Collection

**Collection URL:** [Check Postman Collection](https://www.postman.com/red-firefly-802924/workspace/my-workspace/collection/50190626-c13dcdcf-fb31-4e53-90e8-6e2ff410621b?action=share&creator=50190626&active-environment=50190626-0c20de25-6f83-4560-84b5-181ddfd182a9)

---

**Built with ASP.NET Core 8.0**
