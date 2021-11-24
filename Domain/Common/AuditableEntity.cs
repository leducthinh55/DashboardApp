using DashboardApp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApp.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }

        [ForeignKey("Account")]
        public Guid CreatedBy { get; set; }
        public Account Account { get; set; }
    }
}
