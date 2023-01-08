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

            try
            {
                AddRange(items);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public bool HasPagePrevious => Index > 1;
        public bool HasPageNext => Index < Total;

        public static async Task<ListPaginated<T>> CreateAsync(IQueryable<T> source, int index, int size = 50)
        {
            List<T> items = new();
            var count = 0;

            try
            {
                count = await source.CountAsync();
                items = await source.Skip((index - 1) * size).Take(size).ToListAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }


            return new ListPaginated<T>(items, count, index, size);
        }
    }
}
