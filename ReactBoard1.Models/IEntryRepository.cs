using System.Threading.Tasks;
using Dul.Articles;
using Dul.Domain.Common;

namespace ReactBoard1.Models
{
    public interface IEntryRepository : IRepositoryBase<Entry, long, long>
    {
        // Install package Dul
        Task<PagingResult<Entry>> GetAllPageAsync(int pageIndex, int pageSize);
    }
}