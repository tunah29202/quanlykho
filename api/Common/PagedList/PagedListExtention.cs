using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Common
{
    public static class PagedListExtention
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int size, CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((page - 1) * size)
                                    .Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                page = page,
                size = size,
                total = count,
                data = items,
            };

            return pagedList;
        }

        public static PagedList<T> ToAllPageList<T>(this IQueryable<T> source)
        {
            var pagedList = new PagedList<T>()
            {
                total = source.Count(),
                data = source.ToList(),
            };

            return pagedList;
        }
    }
}