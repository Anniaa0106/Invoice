using FluentValidation;


public class InvoiceLineDto
{
    public string Description { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
public class InvoiceLineDtoValidator : AbstractValidator<InvoiceLineDto>
{
    public InvoiceLineDtoValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("UnitPrice must be greater than zero");
    }
}