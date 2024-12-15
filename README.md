# HealthcareManagementSystem


This is a simple healthcare management system built using C# and .NET 8.0. The application manages patients, medical centers, visits, diagnoses, and medical records. Data is stored in memory using an in-memory database for simplicity.

## Prerequisites

Before running the application, ensure you have the following:

- [.NET 8.0 SDK] installed on your machine.
- A development environment like [Visual Studio] with .NET 8.0 support.

## How to Run the Application

Follow these steps to run the Healthcare Management System Console Application:
Navigate to the Project Directory
- cd HealthcareManagementSystem.ConsoleApp
- dotnet restore
- dotnet run

Once the application is running, you will see the following options:
Select an option:
P. Patient Management
M. Medical Center Management
V. Visit Management
R. Medical Record Management
D. Diagnosis Management
0. Exit
You can choose one of the options to manage patients, medical centers, visits, medical records, or diagnoses.

FYI:
The data in this application is stored in memory using an in-memory database (InMemoryDatabase). This means that all data will be lost once the application is closed. You can modify the application to use a persistent database if needed in the future.
  
