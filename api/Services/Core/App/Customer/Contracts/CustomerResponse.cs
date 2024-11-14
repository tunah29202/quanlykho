namespace Services.Core.Contracts
{
    public class CustomerResponse
    {
        public Guid id { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string? address { get; set; }

        public string? tel { get; set; }
    }
}