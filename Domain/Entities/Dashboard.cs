using DashboardApp.Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Domain.Entities
{
    public class Dashboard : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string LayoutType { get; set; }

        public ICollection<Widget> Widgets { get; set; }
    }
}
