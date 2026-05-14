namespace DPP.InternalWebhookHost.Application.Validators;
public static class SharedValidatorExtensions
{
    public static void ApplyDateRules<T>(this AbstractValidator<T> v) where T : IDateRange
    {
        v.RuleFor(x => x.FromDate)
           .NotEmpty().WithMessage("FromDate is mandatory.");

        v.RuleFor(x => x.ToDate)
            .NotEmpty().WithMessage("ToDate is mandatory.")
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate != default)
            .WithMessage("ToDate cannot be earlier than FromDate.");
    }

    public static void ApplyPagingRules<T>(this AbstractValidator<T> v) where T : IPagingParameter
    {
        v.RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page Number Should be proper .");
        v.RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page Size Should be proper .");
    }
}