using FluentValidation;
using System;
namespace Services.Core.Contracts
{
    public class InvoicePagedRequest
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public bool get_all { get; set; }

        public Guid warehouse_id { get; set; }
    }
}
