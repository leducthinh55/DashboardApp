using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboards
{
    public class WidgetDTO
    {
        public string Title { get; set; }

        public string WidgetType { get; set; }

        public int MinWidth { get; set; }

        public int MinHeight { get; set; }
    }
}
