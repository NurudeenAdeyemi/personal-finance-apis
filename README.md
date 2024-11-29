# This is a C# .NET-based API for managing customer accounts, including the ability to register new customers and migrate existing customers.

# The project follows the Clean Onion Architecture with CQRS and the Repository Pattern to separate concerns and ensure maintainability. It uses Fluent Validation and Serilog for logging. It uses Entity Framework Core (EF Core) for ORM (Object-Relational Mapping) to interact with the database.

# Features
Register a new customer account.
Migrate an existing customer account.
Verification of Mobile number and Email.
Terms and Conditions acceptance.
Creation of Pin and confirmation.
Enable biometric login or allow the option to enable later.
Logging using Serilog for structured logging.


# Tech Stack
.NET 8 (C#) for the backend
Entity Framework Core for database interactions
FluentValidation for input validation
Clean Onion Architecture for project structure
CQRS for separating read and write operations
MSSQL Server for database
Swagger for API documentation and testing 
Serilog for structured logging

## Architecture
# Clean Onion Architecture:
Domain Layer: Contains the domain models.
Application Layer: Implements the use cases and interacts with repositories.
Infrastructure Layer: Handles database access and logging.
API Layer: Exposes the endpoints to the clients.
CQRS (Command Query Responsibility Segregation):

# FluentValidation: 
Validates incoming request data before passing it to the application layer.

# Logging:
ILogger is used throughout the application for logging various activities.
Serilog is configured for structured logging to enhance observability and debugging.
Serilog is configured to log application activity to both the console and a file stored in the logs directory.
Logs contain structured information such as timestamps, log levels, and exception details.


Logs are written to both the console and log files 