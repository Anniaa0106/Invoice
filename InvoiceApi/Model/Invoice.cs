using System;
using System.Collections.Generic;

public class Invoice
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public List<InvoiceLine> Lines { get; set; } = new();
}
