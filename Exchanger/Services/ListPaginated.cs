using Microsoft.EntityFrameworkCore;

namespace Exchanger.Services
{
    public class ListPaginated<T> : List<T>
    {
        public int Index { get; private set; }
        public int Total { get; private set; }

        public ListPaginated(List<T> items, int count, int index, int size)
        {
            Index = index;
            Total = (int)Math.Ceiling(count / (double)size);

            AddRange(items);
        }

        public bool HasPreviousPage => Index > 1;

        public bool HasNextPage => Index < Total;

        public static async Task<ListPaginated<T>> CreateAsync(IQueryable<T> source, int index, int size = 50)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((index - 1) * size).Take(size).ToListAsync();
            return new ListPaginated<T>(items, count, index, size);
        }
    }
}
