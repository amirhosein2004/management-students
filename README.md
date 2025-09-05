# 🎓 Student Management API

A comprehensive C# ASP.NET Core Web API project for managing student information with both API endpoints and web interface. This project demonstrates best practices for building modern web applications with database integration, containerization, and clean architecture.

## ✨ Features

- **🌐 Web Interface**: Complete student management UI with Persian/Farsi support
- **🔗 REST API**: Full CRUD operations via RESTful endpoints
- **🗄️ SQL Server Integration**: Using Entity Framework Core with migrations
- **🏗️ Clean Architecture**: Repository and Service pattern implementation
- **📊 Data Transfer Objects (DTOs)**: Separation of domain and API models
- **🔄 AutoMapper**: Automatic object mapping between models
- **📚 Swagger Documentation**: Interactive API documentation and testing
- **🐳 Docker Support**: Fully containerized application and database
- **⚠️ Error Handling**: Comprehensive exception handling and logging
- **✅ Input Validation**: Request validation using data annotations
- **🌍 CORS Enabled**: Ready for front-end integration
- **🎨 Bootstrap UI**: Modern and responsive user interface

## Project Structure

```
StudentManagementAPI/
├── Controllers/         # API controllers
├── Models/             # Domain models
├── DTOs/               # Data Transfer Objects
├── Services/           # Business logic
├── Repositories/       # Data access
├── Data/               # Database context and migrations
├── Dockerfile          # Docker configuration for API
└── appsettings.json    # Application configuration
```

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) and Docker Compose
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development without Docker)

## Getting Started

### 🐳 Running with Docker (Recommended)

1. Clone the repository
2. Navigate to the project root directory
3. Run the following command:

```bash
docker-compose up -d
```

4. **Access Points:**
   - **🌐 Web Interface**: http://localhost:8080
   - **📚 API Documentation**: http://localhost:8080/swagger
   - **🔗 API Base URL**: http://localhost:8080/api
   - **🗄️ Database**: localhost:1433 (sa/YourStrong@Passw0rd)

### 💻 Running Locally (without Docker)

1. Clone the repository
2. Navigate to the project root directory
3. Update the connection string in `appsettings.Development.json` to point to your local SQL Server instance
4. Run the following commands:

```bash
dotnet restore
dotnet run
```

5. **Access Points:**
   - **🌐 Web Interface**: https://localhost:5001
   - **📚 API Documentation**: https://localhost:5001/swagger
   - **🔗 API Base URL**: https://localhost:5001/api

## 🔗 API Endpoints

### 📊 Students API

| Method | Endpoint             | Description               | Response Type |
|--------|----------------------|---------------------------|---------------|
| GET    | /api/students        | Get all students          | `StudentDTO[]` |
| GET    | /api/students/{id}   | Get a specific student    | `StudentDTO` |
| POST   | /api/students        | Create a new student      | `StudentDTO` |
| PUT    | /api/students/{id}   | Update an existing student| `StudentDTO` |
| DELETE | /api/students/{id}   | Delete a student          | `204 No Content` |

### 🌐 Web Interface Routes

| Route                    | Description                    | Features |
|--------------------------|--------------------------------|----------|
| `/`                      | Home page                      | Welcome page with navigation |
| `/StudentsView`          | Student management interface   | List, Create, Edit, Delete students |
| `/StudentsView/Create`   | Add new student form           | Form validation, Persian UI |
| `/StudentsView/Edit/{id}`| Edit student form              | Pre-populated form, validation |
| `/StudentsView/Details/{id}` | View student details       | Read-only student information |
| `/StudentsView/Delete/{id}`  | Delete confirmation page   | Safe deletion with confirmation |

## Sample API Usage

### Get All Students

```http
GET /api/students
```

### Get Student by ID

```http
GET /api/students/1
```

### Create a New Student

```http
POST /api/students
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "555-123-4567",
  "dateOfBirth": "1995-05-15",
  "address": "123 Main St, Anytown, USA",
  "enrollmentDate": "2023-09-01",
  "major": "Computer Science"
}
```

### Update a Student

```http
PUT /api/students/1
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Smith",
  "email": "john.smith@example.com"
}
```

### Delete a Student

```http
DELETE /api/students/1
```

## Database Schema

### Student

| Column         | Type         | Description                   |
|----------------|--------------|-------------------------------|
| Id             | int          | Primary key                   |
| FirstName      | string       | Student's first name          |
| LastName       | string       | Student's last name           |
| Email          | string       | Student's email address       |
| PhoneNumber    | string       | Student's phone number        |
| DateOfBirth    | DateTime     | Student's date of birth       |
| Address        | string       | Student's address             |
| EnrollmentDate | DateTime     | Date of enrollment            |
| Major          | string       | Student's major               |

## Docker Configuration

The project includes Docker support for both the API and SQL Server:

- **API Container**: Runs the ASP.NET Core application
- **SQL Server Container**: Runs SQL Server 2022
- **Persistent Volume**: Ensures database data is preserved between container restarts
- **Docker Network**: Allows containers to communicate with each other

## 🔧 Troubleshooting

### Common Issues

#### 1. Database Connection Issues
```bash
# Check if database container is running
docker-compose ps

# View database logs
docker-compose logs db

# Restart database container
docker-compose restart db
```

#### 2. API Container Not Starting
```bash
# Rebuild API container
docker-compose build api

# View API logs
docker-compose logs api

# Check for port conflicts
netstat -ano | findstr :8080
```

#### 3. Edit/Save Functionality Issues
- **Problem**: User edit form not saving properly
- **Solution**: Ensure AutoMapper configuration is correct and all required fields are mapped
- **Status**: ✅ Fixed in latest version

#### 4. Type Mismatch Errors
- **Problem**: `InvalidOperationException` with StudentDTO vs StudentDto
- **Solution**: Use proper DTO mapping in controllers
- **Status**: ✅ Fixed in latest version

### Performance Tips

- Use pagination for large datasets: `GET /api/students?page=1&size=10`
- Enable response caching for better performance
- Monitor database query performance using SQL Server Management Studio

### Development Tips

```bash
# Hot reload during development
dotnet watch run

# Run specific migration
dotnet ef database update

# Generate new migration
dotnet ef migrations add MigrationName
```

## 🚀 Future Enhancements

- **🔐 Authentication**: Add JWT-based authentication and authorization
- **📄 Pagination**: Implement pagination for large datasets
- **🧪 Testing**: Add comprehensive unit and integration tests
- **📱 Mobile App**: Develop mobile application using Xamarin/MAUI
- **🔄 CI/CD**: Add GitHub Actions pipeline configuration
- **📊 Analytics**: Add reporting and analytics dashboard
- **🔍 Search**: Implement advanced search and filtering
- **📧 Notifications**: Add email notifications for important events

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 📞 Support

For support and questions:
- 📧 Email: support@university.edu
- 🐛 Issues: [GitHub Issues](https://github.com/your-repo/issues)
- 📖 Documentation: Available at `/swagger` endpoint

**Made with ❤️ for educational purposes**