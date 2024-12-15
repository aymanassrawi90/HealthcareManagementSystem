// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure;
using HealthcareManagementSystem.Application.Services;
using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain;
using System.Reflection;
using System;
using HealthcareManagementSystem.Infrastructure.Repositories;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using HealthcareManagementSystem.ConsoleApp;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configure DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("HealthcareManagementSystemDB")); // Using InMemoryDatabase for simplicity

        // Register services for Dependency Injection
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IHealthCenterService, HealthCenterService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IDiagnosisService, DiagnosisService>();

        // Register Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IHelper, Helper>();
        // Register any other dependencies
    })
    .Build();

// Initialize the application and start

var app = new MedicalSystemApp(host.Services);
await app.InitializeAppAsync();



//static async Task InitializeAppAsync(IServiceProvider services)
//{




//    var patientService = services.GetRequiredService<IPatientService>();

//    MedicalCenterManagement.Initialize(services.GetRequiredService<IHealthCenterService>());
//    var _patientManagement = new PatientManagement(patientService);
//    var _visitManagement = new VisitManagement(services.GetRequiredService<IVisitService>(), patientService, services.GetRequiredService<IHealthCenterService>());
//    var _medicalRecordManagement = new MedicalRecordManagement(services.GetRequiredService<IMedicalRecordService>(),services.GetRequiredService<IVisitService>(), patientService);
//    var _diagnosisManagement = new DiagnosisManagement(services.GetRequiredService<IDiagnosisService>(), services.GetRequiredService<IHealthCenterService>());
//    Console.WriteLine("Welcome to the Medical System!");

//    while (true)
//    {
//        Console.WriteLine("Select an option:");
//        Console.WriteLine("P. Patient Management");
//        Console.WriteLine("M. Medical Center Management");
//        Console.WriteLine("V. Visit Management");
//        Console.WriteLine("R. Medical Record Management");
//        Console.WriteLine("D. Diagnosis Management");
//        Console.WriteLine("0. Exit");
//        var choice = Console.ReadLine();
//        switch (choice.ToLower())
//        {
//            case "p":
//              await _patientManagement.ProcessAsync();

//                break;

//            case "m":
//                await MedicalCenterManagement.Process();
//            break;


//            case "v":
//               await _visitManagement.ProcessAsync();
//                break;
//            case "r":
//                try
//                {
//                  await  _medicalRecordManagement.ProcessAsync();
//                }
//                catch(Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                }
//                break;
//            case "d":

//                try
//                {
//                    await _diagnosisManagement.ProcessAsync();
//                }
//                catch(Exception ex)
//                {
//                    Helper.DisplayError(ex.Message);
//                }

//                break;

//            case "0":
//                return;

//            default:
//                Console.WriteLine("Invalid option. Please try again.");
//                break;

//        }


//    }


//}


