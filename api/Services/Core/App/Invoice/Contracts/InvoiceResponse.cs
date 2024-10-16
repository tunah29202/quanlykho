using Database.Entities;
namespace Services.Core.Contracts
{
    public class InvoiceResponse
    {
        public string invoice_no { get; set; }

        public DateTime? invoice_date { get; set; }
        public string? shipped_per { get; set; }

        public DateTime? shipped_date { get; set; }

        public string? total_weight { get; set; }
        public double? total_volumn { get; set; }

        public Guid shipper_id { get; set; }

        public Shipper shipper { get; set; }

        public Guid consignee_id { get; set; }

        public Consignee consignee { get; set; }

        public int status { get; set; }

        public string? notes { get; set; }

        public Guid carton_id { get; set; }

        public Carton carton { get; set; }

        public DateTime? payment_date { get; set; }

        public Guid? payment_method_id { get; set; }
        public PaymentMethod payment_method { get; set; }
        public Guid? bank_account_id { get; set; }
        public BankAccount bank_account { get; set; }

        public Guid warehouse_id { get; set; }
        public Warehouse warehouse { get; set; }
    }
}