using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.TcKimlikNo)
            .IsRequired()
            .HasMaxLength(11);
            
        builder.HasIndex(p => p.TcKimlikNo)
            .IsUnique();
            
        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(p => p.FatherName)
            .HasMaxLength(50);
            
        builder.Property(p => p.MotherName)
            .HasMaxLength(50);
            
        builder.Property(p => p.BirthPlace)
            .HasMaxLength(100);
            
        builder.Property(p => p.Email)
            .HasMaxLength(100);
            
        builder.Property(p => p.Phone)
            .HasMaxLength(20);
            
        builder.Property(p => p.Address)
            .HasMaxLength(500);
            
        builder.Property(p => p.EmployeeNumber)
            .HasMaxLength(20);
            

            
        builder.Property(p => p.MaritalStatus)
            .HasMaxLength(20);
            
        builder.Property(p => p.BloodType)
            .HasMaxLength(5);
            
        builder.Property(p => p.Religion)
            .HasMaxLength(50);
            
        builder.Property(p => p.MilitaryStatus)
            .HasMaxLength(50);
            
        builder.Property(p => p.DriverLicenseClass)
            .HasMaxLength(10);
            
        builder.Property(p => p.SskNumber)
            .HasMaxLength(20);
            
        builder.Property(p => p.Salary)
            .HasColumnType("decimal(18,2)");
            
        // Foreign Keys
        builder.HasOne(p => p.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Position)
            .WithMany(pos => pos.Persons)
            .HasForeignKey(p => p.PositionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
