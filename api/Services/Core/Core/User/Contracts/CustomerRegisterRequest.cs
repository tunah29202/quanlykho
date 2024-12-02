namespace Services.Core.Contracts
{
    public class CustomerRegisterRequest
    {
        public string? user_name { get; set; }
        public string? password { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? company_name { get; set; }
        public string? company_type { get; set; }
        public string? address { get; set; }
        public string? tax { get; set; } 

    }
}
