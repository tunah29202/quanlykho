namespace Services.Core.Contracts
{
    public class ShipperResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }

        public string address { get; set; }

        public string tax { get; set; }

        public string? email { get; set; }

        public string tel { get; set; }

        public string? fax { get; set; }
    }
}