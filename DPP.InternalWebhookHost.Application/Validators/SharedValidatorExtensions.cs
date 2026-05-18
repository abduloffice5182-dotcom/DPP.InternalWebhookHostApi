namespace DPP.InternalWebhookHost.Application.Validators;
public static class SharedValidatorExtensions
{
    public static void ApplyDateRules<T>(this AbstractValidator<T> v) where T : IDateRange
    {
        v.RuleFor(x => x.FromDate)
           .NotEmpty().WithMessage("FromDate is mandatory.");

        v.RuleFor(x => x.ToDate)
            .NotEmpty().WithMessage("ToDate is mandatory.");

		v.RuleFor(x => x.ToDate) 
		 .GreaterThanOrEqualTo(x => x.FromDate) 
		 .WithMessage("ToDate cannot be earlier than FromDate.");
	}

    public static void ApplyPagingRules<T>(this AbstractValidator<T> v) where T : IPagingParameter
    {
        v.RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page Number Should be proper .");
        v.RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page Size Should be proper .");
    }
}