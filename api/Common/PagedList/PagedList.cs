using Microsoft.EntityFrameworkCore;
namespace Common
{
    public class PagedList<T>
    {
        public int page { get; set; }
        public int size { get; set; }
        public int total { get; set; }
        public IList<T> data { get; set; }

        internal PagedList(IEnumerable<T> source, int _page, int _size)
        {
            if(data is IQueryable<T> queryable)
            {
                page = _page;
                size = _size;
                total = source.Count();

                data = queryable.Skip((page - 1) * size).Take(size).ToList();
            }
            else
            {
                page = _page;
                size = _size;
                total = source.Count();

                data = source.Skip((page - 1) * size).Take(size).ToList();
            }
        }
        internal PagedList() => data = new T[0];
    }
}