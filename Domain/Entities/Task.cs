using DashboardApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Domain.Entities
{
    public class Task : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string TaskName { get; set; }

        public bool IsCompleted { get; set; }
    }
}
