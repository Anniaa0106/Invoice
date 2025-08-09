using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InvoiceDbContext>(opt => opt.UseInMemoryDatabase("InvoiceDb"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceCreateDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Invoice API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/invoices", async (
    InvoiceCreateDto invoiceDto,
    IValidator<InvoiceCreateDto> validator,
    IUnitOfWork uow) =>
{
    var validationResult = await validator.ValidateAsync(invoiceDto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    var invoice = new Invoice
    {
        CustomerName = invoiceDto.CustomerName,
        InvoiceDate = invoiceDto.InvoiceDate,
        Lines = invoiceDto.Lines.Select(l => new InvoiceLine
        {
            Description = l.Description,
            Quantity = l.Quantity,
            UnitPrice = l.UnitPrice
        }).ToList()
    };

    await uow.InvoiceRepository.AddAsync(invoice);
    await uow.CommitAsync();

    return Results.Created($"/invoices/{invoice.Id}", invoice);
});

app.Run();
public partial class Program { }