using Database.Entities;
namespace Services.Core.Contracts
{
    public class FunctionResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string name { get; set; }
        public string? url { get; set; }
        public string? method { get; set; }
        public string? parent_cd { get; set; }
        public Guid? parent_id { get; set; }
        public List<FunctionResponse>? items { get; set; }
    }
}