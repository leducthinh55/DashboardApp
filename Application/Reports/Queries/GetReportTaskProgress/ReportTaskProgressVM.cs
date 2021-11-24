using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetReportTaskProgress
{
    public class ReportTaskProgressVM
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int InCompleted { get; set; }
    }
}
