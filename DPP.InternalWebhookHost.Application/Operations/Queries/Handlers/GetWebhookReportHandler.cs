using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;
namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, ApiResponse<object>>
{

    private readonly IWebhookRepository repository;

    public GetWebhookReportHandler(IWebhookRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ApiResponse<object>> Handle(GetWebhookReportQuery req, CancellationToken ct)
    {
        var endOfDay = req.ToDate?.TimeOfDay == TimeSpan.Zero
         ? req.ToDate.Value.Date.AddDays(1).AddTicks(-1)
         : req.ToDate;

        var (total, items) = await repository.GetWebhookReportAsync(
            req.FromDate, endOfDay, req.PageNumber, req.PageSize, ct);

        return new ApiResponse<object>(true, 200, "Success", new
        {
            TotalCount = total,
            Items = items,
            req.PageNumber,
            req.PageSize
        });
    }

}
