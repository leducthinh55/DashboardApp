using DashboardApp.Application.Common.Interfaces;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Application.Contacts.Queries.ExportContact;

namespace DashboardApp.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildContactRecordFile(IEnumerable<ContactRecord> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}
