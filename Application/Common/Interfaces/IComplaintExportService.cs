using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IComplaintExportService
    {
        Task<string> ExportToExcelAsync(IEnumerable<Complaint> complaints);
    }
}
