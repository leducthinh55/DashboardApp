using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class ResultModel
    {
        public int ProcessStatus { get; private set; }

        public object Data { get; private set; }

        public void Succeeded(object data)
        {
            ProcessStatus = (int)Domain.Enums.ProcessStatus.Success;
            Data = data;
        }

        public void Succeeded()
        {
            ProcessStatus = (int)Domain.Enums.ProcessStatus.Success;
        }

        public void Failure(int processStatus)
        {
            ProcessStatus = processStatus;
        }

        public void Failure(int processStatus, object data)
        {
            ProcessStatus = processStatus;
            Data = data;
        }

        public bool IsSucceeded()
        {
            return ProcessStatus == (int)Domain.Enums.ProcessStatus.Success;
        }

        public bool IsFailue()
        {
            return ProcessStatus != (int)Domain.Enums.ProcessStatus.Success;
        }
    }
}
