using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
{
    public void Configure(EntityTypeBuilder<LeaveBalance> builder)
    {
        // Primary Key
        builder.HasKey(lb => lb.Id);

        // Properties
        builder.Property(lb => lb.AllocatedDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.UsedDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.PendingDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.CarriedOverDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.MonthlyAccrual)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.AccruedToDate)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.ManualAdjustment)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.AdjustmentReason)
            .HasMaxLength(500);

        builder.Property(lb => lb.Year)
            .HasDefaultValue(DateTime.Now.Year);

        builder.Property(lb => lb.LastAccrualDate)
            .HasDefaultValueSql("GETDATE()");

        // Computed columns for reporting purposes
        builder.Property(lb => lb.AvailableDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(lb => lb.RemainingDays)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        // Relationships
        builder.HasOne(lb => lb.Person)
            .WithMany(p => p.LeaveBalances)
            .HasForeignKey(lb => lb.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lb => lb.LeaveType)
            .WithMany(lt => lt.LeaveBalances)
            .HasForeignKey(lb => lb.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Temporarily commented out to avoid FK constraint issues
        // builder.HasOne(lb => lb.AdjustedBy)
        //     .WithMany()
        //     .HasForeignKey(lb => lb.AdjustedById)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(lb => lb.PersonId);
        builder.HasIndex(lb => lb.LeaveTypeId);
        builder.HasIndex(lb => new { lb.PersonId, lb.LeaveTypeId, lb.Year }).IsUnique();
        builder.HasIndex(lb => lb.Year);

        // Constraints
        builder.ToTable(t => {
            t.HasCheckConstraint("CK_LeaveBalance_AllocatedDays", "AllocatedDays >= 0");
            t.HasCheckConstraint("CK_LeaveBalance_UsedDays", "UsedDays >= 0");
            t.HasCheckConstraint("CK_LeaveBalance_PendingDays", "PendingDays >= 0");
            t.HasCheckConstraint("CK_LeaveBalance_CarriedOverDays", "CarriedOverDays >= 0");
        });
    }
}
