using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Position : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxSalary { get; set; }

        [Range(0, 50)]
        public int? RequiredExperience { get; set; } // Years

        [StringLength(1000)]
        public string? Requirements { get; set; }

        [StringLength(1000)]
        public string? Responsibilities { get; set; }

        [StringLength(50)]
        public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, etc.

        [StringLength(50)]
        public string? Level { get; set; } // Junior, Mid, Senior, Manager, etc.

        public bool IsAvailable { get; set; } = true;

        // Navigation property - Positions can have many people assigned
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();
    }
}
