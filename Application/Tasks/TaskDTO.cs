using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks
{
    public abstract class TaskDTO
    {
        public string TaskName { get; set; }

        public bool IsCompleted { get; set; }
    }
}
