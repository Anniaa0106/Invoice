using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoiceData)
    {
        // The invoice date is set on server side
        var invoice = new Invoice
        {
            CustomerName = invoiceData.CustomerName,
            InvoiceDate = DateTime.UtcNow,
            Lines = invoiceData.Lines.Select(l => new InvoiceLine
            {
                ProductName = l.ProductName,
                Quantity = l.Quantity,
                Price = l.Price
            }).ToList()
        };

        await _unitOfWork.Invoices.AddAsync(invoice);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var invoice = await _unitOfWork.Invoices.GetByIdAsync<Invoice>(id);
        if (invoice == null)
            return NotFound();
        return Ok(invoice);
    }
}
