public interface IUnitOfWork
{
    IInvoiceRepository InvoiceRepository { get; }
    Task<int> CommitAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly InvoiceDbContext _context;
    private IInvoiceRepository? _invoiceRepository;

    public UnitOfWork(InvoiceDbContext context)
    {
        _context = context;
    }

    public IInvoiceRepository InvoiceRepository => _invoiceRepository ??= new InvoiceRepository(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}