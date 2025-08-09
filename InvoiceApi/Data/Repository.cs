public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice);
}

public class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoiceDbContext _context;
    public InvoiceRepository(InvoiceDbContext context) => _context = context;

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
    }
}