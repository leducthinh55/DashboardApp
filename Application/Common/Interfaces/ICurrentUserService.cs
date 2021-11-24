using System;

namespace DashboardApp.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid AccountId { get; }
    }
}
