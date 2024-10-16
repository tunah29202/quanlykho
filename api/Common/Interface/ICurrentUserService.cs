namespace Common
{
    public interface ICurrentUserService
    {
        public Guid? user_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
    }
}
