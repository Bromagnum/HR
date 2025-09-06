using DAL.Entities;

namespace BLL.DTOs;

public class LeaveListDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsActive { get; set; }
    public bool RequiresDocument { get; set; }
    public bool HasDocument { get; set; }
    public int DaysUntilStart { get; set; } // Calculated field
    public bool IsUrgent { get; set; } // Calculated field
}

public class LeaveDetailDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public string LeaveTypeColor { get; set; } = string.Empty;
    public bool LeaveTypeRequiresDocument { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? DocumentPath { get; set; }
    public LeaveStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    
    // Approval Information
    public int? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovalNotes { get; set; }
    public string? RejectionReason { get; set; }
    
    // Emergency Contact
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    
    // Handover Information
    public string? HandoverNotes { get; set; }
    public int? HandoverToPersonId { get; set; }
    public string? HandoverToPersonName { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    
    // Related Information
    public decimal RemainingBalance { get; set; }
    public List<LeaveListDto> OverlappingLeaves { get; set; } = new();
}

public class LeaveCreateDto
{
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public string? HandoverNotes { get; set; }
    public int? HandoverToPersonId { get; set; }
    
    // Calculated field (set by business logic)
    public int TotalDays { get; set; }
}

public class LeaveUpdateDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public string? HandoverNotes { get; set; }
    public int? HandoverToPersonId { get; set; }
    public int TotalDays { get; set; }
}

public class LeaveApprovalDto
{
    public int Id { get; set; }
    public int ApprovedById { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovalNotes { get; set; }
    public string? RejectionReason { get; set; }
}

public class LeaveFilterDto
{
    public int? PersonId { get; set; }
    public int? LeaveTypeId { get; set; }
    public int? DepartmentId { get; set; }
    public LeaveStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Year { get; set; } = DateTime.Now.Year;
    public bool? RequiresApproval { get; set; }
    public bool? HasDocument { get; set; }
    public string? SearchTerm { get; set; }
}

public class LeaveCalendarDto
{
    public int Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string LeaveTypeName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public LeaveStatus Status { get; set; }
    public string Title => $"{PersonName} - {LeaveTypeName}";
    public string Tooltip => $"{PersonName}\n{LeaveTypeName}\n{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}\n{TotalDays} gün";
}
