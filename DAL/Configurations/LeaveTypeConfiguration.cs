using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        // Primary Key
        builder.HasKey(lt => lt.Id);

        // Properties
        builder.Property(lt => lt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(lt => lt.Description)
            .HasMaxLength(500);

        builder.Property(lt => lt.Color)
            .HasMaxLength(7) // For hex color codes
            .HasDefaultValue("#007bff");

        builder.Property(lt => lt.MaxDaysPerYear)
            .HasDefaultValue(0);

        builder.Property(lt => lt.RequiresApproval)
            .HasDefaultValue(true);

        builder.Property(lt => lt.RequiresDocument)
            .HasDefaultValue(false);

        builder.Property(lt => lt.IsPaid)
            .HasDefaultValue(true);

        builder.Property(lt => lt.CanCarryOver)
            .HasDefaultValue(false);

        builder.Property(lt => lt.MaxCarryOverDays)
            .HasDefaultValue(0);

        builder.Property(lt => lt.NotificationDays)
            .HasDefaultValue(2);

        // Relationships
        builder.HasMany(lt => lt.Leaves)
            .WithOne(l => l.LeaveType)
            .HasForeignKey(l => l.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(lt => lt.LeaveBalances)
            .WithOne(lb => lb.LeaveType)
            .HasForeignKey(lb => lb.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(lt => lt.Name).IsUnique();
        builder.HasIndex(lt => lt.IsActive);
    }
}
