using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Domain.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string FullName { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RevokeTime { get; set; }

        public bool IsVerificationEmail { get; set; }

        public ICollection<Contact> Contacts { get; set; }

        public ICollection<Dashboard> Dashboards { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
