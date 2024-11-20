namespace Services.Core.Contracts
{
    public class CustomerResponse
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string company_name { get; set; }
        public string company_type { get; set; }
        public string? address { get; set; }
        public string? tax { get; set; }
        public string? tel { get; set; }
        public string? email { get; set; }
    }
}