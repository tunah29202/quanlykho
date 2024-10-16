namespace Common
{
    public class PagedRequest
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public bool get_all { get; set; } = false;
    }
}
