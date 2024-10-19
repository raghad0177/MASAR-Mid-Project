# MASAR Platform API
This is the API for the MASAR Platform, designed to manage various entities such as users, drivers, students, buses, and maintenance requests. The platform supports real-time bus tracking, role-based access, and user management through a secure API built with ASP.NET Core and JWT authentication.

# Features
Role-based Authorization: Admin, Driver, Student, and general User roles, each with different permissions.
User Management: Admins can create, update, and delete users and manage roles.
Bus Management: Real-time bus location updates and retrieval.
Maintenance Requests: Drivers can submit maintenance requests, and admins can approve or reject them.
Announcements: Admins can create announcements, and users can view them.

# JWT Authentication: Secure API access through token-based authentication.
Swagger Documentation: Integrated Swagger UI for API exploration and testing.
Technologies Used
ASP.NET Core 6.0: Framework for building the API.
Entity Framework Core: ORM for database interaction.
SQL Server: Database used for storing user, bus, and maintenance data.
JWT (JSON Web Token): For secure user authentication.
Swagger: API documentation and testing interface.
Identity Framework: For managing user roles and authentication.
Prerequisites

# Before running this application, ensure you have the following installed:
.NET 6 SDK
SQL Server
Visual Studio (or any IDE that supports .NET Core)
Postman (optional, for API testing)

# JWT Authentication and Authorization
This API uses JWT for secure authentication. After successful login, users are issued a JWT token, which must be included in the Authorization header for subsequent requests.

# Controllers
Admin: Full control over drivers, buses, and maintenance requests.
Driver: Can view announcements, submit maintenance requests, and update their profile.
Student: Can view announcements and register.
General Users: Login and logout functionality, with access to public announcements.

# API Endpoints

## AdminController
GET /api/Admin/getAllUsers/TEST: Retrieve all users.
GET /api/Admin/getAllDriversByAdmin: Retrieve all drivers (Admin-only access).
GET /api/Admin/getDriverByEmail/{email}: Retrieve driver by email.
POST /api/Admin/CreateDriverByAdmin: Create a new driver.
DELETE /api/Admin/DeleteDriverByAdmin/{email}: Delete a driver by email.
## BusesController
POST /api/Buses/CreateBusByAdmin: Create a new bus.
POST /api/Buses/updateBusLocation: Update bus location.
GET /api/Buses/getBusLocation: Retrieve current bus location by bus ID.
## DriverController
GET /api/Driver/ViewAllAnnouncement: View all announcements (Driver-only access).
POST /api/Driver/MaintenanceRequest: Submit a maintenance request (Driver-only access).
## StudentController
GET /api/Student/ViewAllAnnouncement: View all announcements (Student-only access).
POST /api/Student/StudentRegister: Register as a new student.
## UsersController
POST /api/Users/Login: Login for any user.
POST /api/Users/Logout: Logout for any user.
GET /api/Users/ViewAllAnnouncement: View all announcements (User access).
PUT /api/Users/updateUserByEmail/{email}: Update user information (Admin or Student role access).

# Swagger UI
The MASAR API has Swagger documentation enabled for easier exploration and testing.

To access the Swagger UI:
Run the application.
Open your browser and navigate to: http://localhost:5000/api/MASARApi/swagger.json
Swagger UI provides interactive documentation of all available API endpoints.

# Authorization Policies
The application uses the following role-based policies:
RequireAdminRole: Allows access to Admin-only resources.
RequireDriverRole: Allows access to Driver-only resources.
RequireStudentRole: Allows access to Student-only resources.
RequireAdminStudentRole: Allows access to both Admin and Student resources.
RequireUserRole: Allows access to resources for all user roles (Admin, Driver, Student).

# License
This project is licensed under the MIT License.
