namespace UXComex.GerenciadorPedidos.Domain.Models
{
    /// <summary>
    /// A generic class to hold paginated data.
    /// </summary>
    /// <typeparam name="T">The type of data being paginated.</typeparam>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}