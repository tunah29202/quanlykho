using Database.Entities;

namespace Services.Core.Contracts
{
    public class UserResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? user_name { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? role_cd {  get; set; }
        public string? role_name { get; set;}
        public List<string>? permissions { get; set; } 
        public List<Guid>? warehouse_ids { get; set; }
        public List<string> warehouse_names { get; set; }
    }
}