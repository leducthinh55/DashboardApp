using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboards
{
    public abstract class DashboardDTO
    {
        public string Title { get; set; }

        public string LayoutType { get; set; }
    }
}
