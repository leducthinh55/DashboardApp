using Application.Contacts.Queries.ExportContact;
using DashboardApp.Application.Common;
using System.Collections.Generic;

namespace DashboardApp.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildContactRecordFile(IEnumerable<ContactRecord> records);
    }
}
