using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FrameworkBaseService.Tools
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage {
            get {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage {
            get {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageSize > 0)
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(e => 0).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
            else
            {
                var count = await source.CountAsync();
                var items = await source.OrderBy(e => 0).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, -1);
            }
        }

        public static Task<PaginatedList<T>> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (pageSize > 0)
            {
                var count = source.Count();
                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(e => 0).ToList();
                return Task.FromResult(new PaginatedList<T>(items, count, pageIndex, pageSize));
            }
            else
            {
                var count = source.Count();
                var items = source.ToList();
                return Task.FromResult(new PaginatedList<T>(items, count, pageIndex, pageSize));
            }
        }
    }
}