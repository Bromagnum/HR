using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class WorkLogConfiguration : IEntityTypeConfiguration<WorkLog>
    {
        public void Configure(EntityTypeBuilder<WorkLog> builder)
        {
            // Table name
            builder.ToTable("WorkLogs");

            // Primary key
            builder.HasKey(w => w.Id);

            // Properties
            builder.Property(w => w.Date)
                   .IsRequired()
                   .HasColumnType("date");

            builder.Property(w => w.StartTime)
                   .IsRequired()
                   .HasColumnType("time");

            builder.Property(w => w.EndTime)
                   .HasColumnType("time");

            builder.Property(w => w.BreakStartTime)
                   .HasColumnType("time");

            builder.Property(w => w.BreakEndTime)
                   .HasColumnType("time");

            builder.Property(w => w.BreakDurationMinutes)
                   .HasColumnType("decimal(5,2)")
                   .HasDefaultValue(0);

            builder.Property(w => w.TotalHours)
                   .HasColumnType("decimal(5,2)")
                   .HasDefaultValue(0);

            builder.Property(w => w.RegularHours)
                   .HasColumnType("decimal(5,2)")
                   .HasDefaultValue(0);

            builder.Property(w => w.OvertimeHours)
                   .HasColumnType("decimal(5,2)")
                   .HasDefaultValue(0);

            builder.Property(w => w.Status)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("Active");

            builder.Property(w => w.WorkType)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("Office");

            builder.Property(w => w.Notes)
                   .HasMaxLength(500);

            builder.Property(w => w.TasksCompleted)
                   .HasMaxLength(1000);

            builder.Property(w => w.Location)
                   .HasMaxLength(200);

            builder.Property(w => w.ApprovalNotes)
                   .HasMaxLength(500);

            builder.Property(w => w.CheckInIP)
                   .HasMaxLength(50);

            builder.Property(w => w.CheckOutIP)
                   .HasMaxLength(50);

            builder.Property(w => w.IsLateArrival)
                   .HasDefaultValue(false);

            builder.Property(w => w.IsEarlyDeparture)
                   .HasDefaultValue(false);

            builder.Property(w => w.IsOvertime)
                   .HasDefaultValue(false);

            builder.Property(w => w.IsWeekend)
                   .HasDefaultValue(false);

            builder.Property(w => w.IsHoliday)
                   .HasDefaultValue(false);

            // Relationships
            builder.HasOne(w => w.Person)
                   .WithMany()
                   .HasForeignKey(w => w.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.ApprovedBy)
                   .WithMany()
                   .HasForeignKey(w => w.ApprovedById)
                   .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(w => w.PersonId);
            builder.HasIndex(w => w.Date);
            builder.HasIndex(w => new { w.PersonId, w.Date })
                   .IsUnique()
                   .HasDatabaseName("IX_WorkLogs_PersonId_Date");
            builder.HasIndex(w => w.Status);
            builder.HasIndex(w => w.WorkType);
            builder.HasIndex(w => w.ApprovedById);

            // Check constraints
            builder.ToTable(t => {
                t.HasCheckConstraint("CK_WorkLog_TotalHours", "[TotalHours] >= 0 AND [TotalHours] <= 24");
                t.HasCheckConstraint("CK_WorkLog_RegularHours", "[RegularHours] >= 0 AND [RegularHours] <= 24");
                t.HasCheckConstraint("CK_WorkLog_OvertimeHours", "[OvertimeHours] >= 0 AND [OvertimeHours] <= 24");
                t.HasCheckConstraint("CK_WorkLog_BreakDuration", "[BreakDurationMinutes] >= 0 AND [BreakDurationMinutes] <= 480");
            });
        }
    }
}
