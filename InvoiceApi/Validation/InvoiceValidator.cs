using FluentValidation;


public class InvoiceCreateDto
{
    public string CustomerName { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public List<InvoiceLineDto> Lines { get; set; } = new();
}
public class InvoiceCreateDtoValidator : AbstractValidator<InvoiceCreateDto>
{
    public InvoiceCreateDtoValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required");

        RuleFor(x => x.InvoiceDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("InvoiceDate cannot be in the future");

        RuleForEach(x => x.Lines).SetValidator(new InvoiceLineDtoValidator());

        RuleFor(x => x.Lines)
            .NotEmpty().WithMessage("Invoice must have at least one line");
    }
}