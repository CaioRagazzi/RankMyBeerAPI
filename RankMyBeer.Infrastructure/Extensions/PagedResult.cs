using Microsoft.EntityFrameworkCore;
using RankMyBeerDomain.Models;

namespace RankMyBeerInfrastructure.Extensions;

public static class PagedResultExtension
{
    public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, int? page, int? pageSize) where T : class
    {
        var currentPageComputed = page ?? 1;
        var pageSizeComputed = pageSize ?? 50;
        var result = new PagedResult<T>
        {
            CurrentPage = currentPageComputed,
            PageSize = pageSizeComputed,
            RowCount = await query.CountAsync()
        };


        var pageCount = (double)result.RowCount / pageSizeComputed;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (currentPageComputed - 1) * pageSizeComputed;
        result.Results = await query.Skip(skip).Take(pageSizeComputed).ToListAsync();

        return result;
    }
}

