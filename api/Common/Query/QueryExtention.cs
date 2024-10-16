using System;
using System.Linq;

namespace Common
{
    public static class QueryExtention
    {
        public static IQueryable<T> ExcludeSoftDeleted<T>(this IQueryable<T> source) where T : IEntity
        {
            return source.Where(x => !x.del_flg);
        }
        public static IQueryable<T> FilterById<T>(this IQueryable<T> source, Guid id) where T : IEntity
        {
            return source.Where(x => x.id == id);
        }
        public static IQueryable<T> FindActiveById<T>(this IQueryable<T> source, Guid id) where T : IEntity
        {
            return source.FilterById(id).ExcludeSoftDeleted();
        }
        public static IQueryable<T> FilterByIds<T>(this IQueryable<T> source, Guid[] ids) where T : IEntity
        {
            return ids != null ? source.Where(x => ids.Contains(x.id)) : source;
        }
    }
}