using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        // Primary Key
        builder.HasKey(l => l.Id);

        // Properties
        builder.Property(l => l.Reason)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.Notes)
            .HasMaxLength(2000);

        builder.Property(l => l.DocumentPath)
            .HasMaxLength(500);

        builder.Property(l => l.Status)
            .HasConversion<int>()
            .HasDefaultValue(LeaveStatus.Pending);

        builder.Property(l => l.RequestDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(l => l.ApprovalNotes)
            .HasMaxLength(1000);

        builder.Property(l => l.RejectionReason)
            .HasMaxLength(1000);

        builder.Property(l => l.EmergencyContact)
            .HasMaxLength(100);

        builder.Property(l => l.EmergencyPhone)
            .HasMaxLength(20);

        builder.Property(l => l.HandoverNotes)
            .HasMaxLength(2000);

        // Relationships
        builder.HasOne(l => l.Person)
            .WithMany(p => p.Leaves)
            .HasForeignKey(l => l.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.LeaveType)
            .WithMany(lt => lt.Leaves)
            .HasForeignKey(l => l.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.ApprovedBy)
            .WithMany(p => p.ApprovedLeaves)
            .HasForeignKey(l => l.ApprovedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.HandoverToPerson)
            .WithMany(p => p.HandoverLeaves)
            .HasForeignKey(l => l.HandoverToPersonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        // Indexes
        builder.HasIndex(l => l.PersonId);
        builder.HasIndex(l => l.LeaveTypeId);
        builder.HasIndex(l => new { l.StartDate, l.EndDate });
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.RequestDate);

        // Constraints
        builder.ToTable(t => {
            t.HasCheckConstraint("CK_Leave_DateRange", "EndDate >= StartDate");
            t.HasCheckConstraint("CK_Leave_TotalDays", "TotalDays > 0");
        });
    }
}
