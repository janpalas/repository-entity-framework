using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Pally.Model.EntityFramework.Core
{
    public class PagedList<T>
    {
        public IReadOnlyList<T> Items { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public static class PagedList
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0!");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0!");

            return ToPagedList(queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, pageSize,
                queryable.Count());
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0!");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0!");

            List<T> items = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            int count = await queryable.CountAsync();

            return ToPagedList(items, page, pageSize, count);
        }

        private static PagedList<T> ToPagedList<T>(IList<T> items, int page, int pageSize, int totalItems)
        {
            return new PagedList<T>
            {
                Items = new ReadOnlyCollection<T>(items),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = GetTotalPagesCount(totalItems, pageSize)
            };
        }

        private static int GetTotalPagesCount(int totalItems, int pageSize)
        {
            if (totalItems == 0)
                return 1;

            decimal total = totalItems;
            decimal size = pageSize;

            return (int) Math.Ceiling(total/size);
        }
    }
}
