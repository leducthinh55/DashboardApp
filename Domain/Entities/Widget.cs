using DashboardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Widget
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string WidgetType { get; set; }

        [Required]
        public int MinWidth { get; set; }

        [Required]
        public int MinHeight { get; set; }

        public Guid DashBoardId { get; set; }
        public Dashboard DashBoard { get; set; }
    }
}
