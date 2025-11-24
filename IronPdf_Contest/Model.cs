namespace IronPdf_Contest
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string YourReference { get; set; }
        public string ProjectName { get; set; }
        public string Notes { get; set; }

        public CompanyInfo From { get; set; }    // Your company
        public CompanyInfo To { get; set; }      // Customer company

        public List<InvoiceItem> Items { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public PaymentTerms Terms { get; set; }
    }


    public class CompanyInfo
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string CVR { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }


    public class InvoiceItem
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Price => (Quantity * UnitPrice) - Discount;
    }

    public class PaymentTerms
    {
        public string TextLine1 { get; set; }
        public string BankInfo { get; set; }
        public string ExtraInfo { get; set; }
    }
}
