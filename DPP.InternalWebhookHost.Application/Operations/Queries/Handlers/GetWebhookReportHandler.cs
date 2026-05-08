using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;
namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, ApiResponse>
{

    private readonly IWebhookRepository repository;

    public GetWebhookReportHandler(IWebhookRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ApiResponse> Handle(GetWebhookReportQuery req, CancellationToken ct) =>
     await repository.GetWebhookReportAsync(
         req.FromDate == default ? null : req.FromDate,
         req.ToDate == default ? null : req.ToDate,
         req.PageNumber, req.PageSize, ct)
     .ContinueWith(t => new ApiResponse(true, 200, "Success", new
     {
         t.Result.TotalCount,
         Items = t.Result.Items.Select(x => new { x.Id, x.ReceivedAt, x.Data }),
         req.PageNumber,
         req.PageSize
     }));

}
