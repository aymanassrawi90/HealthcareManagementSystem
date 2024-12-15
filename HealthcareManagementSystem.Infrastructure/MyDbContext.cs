using HealthcareManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<HealthCenter> HealthCenters { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the auto-incrementing Id for the Diagnosis entity without modifying the class
            modelBuilder.Entity<Diagnosis>()
                .Property<int>("Id")  // Defining a new 'Id' property
                .ValueGeneratedOnAdd() // Make it auto-incrementing
               // .HasColumnName("Id") // Optional: specify the column name (default is 'Id')
                .IsRequired(); // Mark it as required

            // Optionally, if you want to configure other columns, you can do so here as well
            modelBuilder.Entity<Diagnosis>().HasKey("Id");

            // Example of the relationship configuration (if needed)
            //modelBuilder.Entity<MedicalRecord>()
            //    .HasOne(m => m.Diagnosis)
            //    .WithMany()
            //    .HasForeignKey("Id");

            base.OnModelCreating(modelBuilder);

        }

    }
}
